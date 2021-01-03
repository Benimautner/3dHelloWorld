using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;

public class EndlessTerrain : MonoBehaviour
{
    public static Vector2 viewerPosition;
    private static MapGenerator _mapGenerator;
    public static float maxViewDistance = 450;
    [FormerlySerializedAs("LodInfos")] public LodInfo[] lodInfos;

    private int _chunkSize;
    public Transform viewer;
    public Material mapMaterial;
    private int _chunksVisibleInView;
    private readonly List<TerrainChunk> _terrainChunksVisible = new List<TerrainChunk>();
    private readonly Dictionary<Vector2, TerrainChunk> _terrainChunks = new Dictionary<Vector2, TerrainChunk>();

    private void Start()
    {
        maxViewDistance = lodInfos[lodInfos.Length - 1].visibleDstThreshold;
        _mapGenerator = FindObjectOfType<MapGenerator>();
        _chunkSize = MapGenerator.MapChunkSize - 1;
        _chunksVisibleInView = Mathf.RoundToInt(maxViewDistance / _chunkSize);
    }

    private void Update()
    {
        var viewPos = viewer.position;
        viewerPosition = new Vector2(viewPos.x, viewPos.z);
        UpdateVisibleChunks();
    }

    private void UpdateVisibleChunks()
    {
        for (var i = 0; i < _terrainChunksVisible.Count; i++) _terrainChunksVisible[i].SetVisible(false);

        var currChunkCordX = Mathf.RoundToInt(viewerPosition.x / _chunkSize);
        var currChunkCordY = Mathf.RoundToInt(viewerPosition.y / _chunkSize);

        for (var yOffset = -_chunksVisibleInView; yOffset <= _chunksVisibleInView; yOffset++)
            for (var xOffset = -_chunksVisibleInView; xOffset <= _chunksVisibleInView; xOffset++) {
                var viewedChunkCoord = new Vector2(currChunkCordX + xOffset, currChunkCordY + yOffset);
                if (_terrainChunks.ContainsKey(viewedChunkCoord)) {
                    _terrainChunks[viewedChunkCoord].UpdateChunk();
                    if (_terrainChunks[viewedChunkCoord].IsVisible()) {
                        _terrainChunksVisible.Add(_terrainChunks[viewedChunkCoord]);
                    }
                }
                else {
                    _terrainChunks.Add(viewedChunkCoord,
                        new TerrainChunk(viewedChunkCoord, _chunkSize, lodInfos, transform, mapMaterial));
                }
        }
    }
    
    public TerrainType GetTerrainTypeOfPosition(Vector2 pos)
    {
        if (!IsOnLoadedChunk(pos)) return new TerrainType(); //TODO load specific chunk of map
        TerrainChunk chunk = GetChunkByLocation(pos);
        throw new NotImplementedException();
        //return new TerrainType();
    }

    private bool IsOnLoadedChunk(Vector2 pos)
    {
        var currChunkCordX = Mathf.RoundToInt(viewerPosition.x / _chunkSize);
        var currChunkCordY = Mathf.RoundToInt(viewerPosition.y / _chunkSize);
        return _terrainChunks.ContainsKey(new Vector2(currChunkCordX, currChunkCordY));
    }
    
    private TerrainChunk GetChunkByLocation(Vector2 pos)
    {
        var currChunkCordX = Mathf.RoundToInt(viewerPosition.x / _chunkSize);
        var currChunkCordY = Mathf.RoundToInt(viewerPosition.y / _chunkSize);
        return _terrainChunks[new Vector2(currChunkCordX, currChunkCordY)];
    }

    public class TerrainChunk
    {

        private Bounds _bounds;
        private MapData _mapData;
        private bool _mapDataReceived;
        private int _prevLodIndex = -1;
        private readonly Vector2 _position;
        private readonly LODMesh[] _lodMeshes;
        private readonly MeshFilter _meshFilter;
        private readonly GameObject _meshObject;
        private readonly LodInfo[] _detailLevels;
        private readonly MeshCollider _meshCollider;
        private readonly MeshRenderer _meshRenderer;

        public TerrainChunk(Vector2 coord, int size, LodInfo[] detailLevels, Transform parent, Material material)
        {
            _detailLevels = detailLevels;
            _position = coord * size;
            _bounds = new Bounds(_position, Vector2.one * size);
            var positionV3 = new Vector3(_position.x, 0, _position.y);

            _meshObject = new GameObject("Terrain Chunk");
            _meshObject.transform.position = positionV3;
            _meshObject.transform.parent = parent;
            
            _meshRenderer = _meshObject.AddComponent<MeshRenderer>();
            _meshFilter = _meshObject.AddComponent<MeshFilter>();
            _meshCollider = _meshObject.AddComponent<MeshCollider>();
            
            _meshRenderer.material = material;
            SetVisible(false);

            _lodMeshes = new LODMesh[detailLevels.Length];
            for (var i = 0; i < detailLevels.Length; i++) _lodMeshes[i] = new LODMesh(detailLevels[i].lod);

            _mapGenerator.RequestMapData(_position, OnMapDataReceived);
        }
        

        private void OnMapDataReceived(MapData mapData)
        {
            _mapData = mapData;
            _mapDataReceived = true;

            var texture = TextureGenerator.TextureFromColorMap(mapData.colorMap, MapGenerator.MapChunkSize,
                MapGenerator.MapChunkSize);
            _meshRenderer.material.mainTexture = texture;
        }
        

        public void UpdateChunk()
        {
            if (!_mapDataReceived) return;
            var viewerDst = Mathf.Sqrt(_bounds.SqrDistance(viewerPosition));
            var visible = viewerDst <= maxViewDistance;

            if (visible) {
                var lodIndex = 0;
                for (var i = 0; i < _detailLevels.Length - 1; i++)
                    if (viewerDst > _detailLevels[i].visibleDstThreshold) lodIndex = i + 1;
                    else break;

                if (lodIndex != _prevLodIndex) {
                    var lodMesh = _lodMeshes[lodIndex];
                    if (lodMesh.hasMesh) {
                        _prevLodIndex = lodIndex;
                        _meshFilter.mesh = lodMesh.mesh;
                        if (_detailLevels[lodIndex].useForCollider) {
                            print("Collision test");
                            _meshCollider.sharedMesh = lodMesh.mesh;
                        }
                    }
                    else if (!lodMesh.requestedMesh) {
                        lodMesh.RequestMesh(_mapData);
                    }
                }
            }

            SetVisible(visible);
        }

        public void SetVisible(bool val)
        {
            _meshObject.SetActive(val);
        }

        public bool IsVisible()
        {
            return _meshObject.activeSelf;
        }
    }

    public class LODMesh
    {
        public Mesh mesh;
        public bool hasMesh;
        private readonly int _lod;
        public bool requestedMesh;

        public LODMesh(int lod)
        {
            _lod = lod;
        }

        private void OnMeshDataReceived(MeshData meshData)
        {
            mesh = meshData.CreateMesh();
            hasMesh = true;
        }

        public void RequestMesh(MapData mapData)
        {
            requestedMesh = true;
            _mapGenerator.RequestMeshData(mapData, _lod, OnMeshDataReceived);
        }
    }
    
}