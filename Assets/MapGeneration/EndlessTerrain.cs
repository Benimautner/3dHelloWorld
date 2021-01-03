using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    public static float maxViewDistance = 450;
    public static Vector2 viewerPosition;
    private static MapGenerator mapGenerator;
    public LodInfo[] LodInfos;

    public Transform viewer;
    public Material mapMaterial;
    private int chunkSize;
    private int chunksVisibleInView;
    private readonly Dictionary<Vector2, TerrainChunk> TerrainChunks = new Dictionary<Vector2, TerrainChunk>();
    private readonly List<TerrainChunk> terrainChunksVisible = new List<TerrainChunk>();

    private void Start()
    {
        maxViewDistance = LodInfos[LodInfos.Length - 1].visibleDstThreshold;
        mapGenerator = FindObjectOfType<MapGenerator>();
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunksVisibleInView = Mathf.RoundToInt(maxViewDistance / chunkSize);
    }

    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();
    }

    private void UpdateVisibleChunks()
    {
        for (var i = 0; i < terrainChunksVisible.Count; i++) terrainChunksVisible[i].SetVisible(false);

        var currChunkCordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        var currChunkCordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (var yOffset = -chunksVisibleInView; yOffset <= chunksVisibleInView; yOffset++)
            for (var xOffset = -chunksVisibleInView; xOffset <= chunksVisibleInView; xOffset++) {
                var viewedChunkCoord = new Vector2(currChunkCordX + xOffset, currChunkCordY + yOffset);
                if (TerrainChunks.ContainsKey(viewedChunkCoord)) {
                    TerrainChunks[viewedChunkCoord].UpdateChunk();
                    if (TerrainChunks[viewedChunkCoord].isVisible()) {
                        terrainChunksVisible.Add(TerrainChunks[viewedChunkCoord]);
                    }
                }
                else {
                    TerrainChunks.Add(viewedChunkCoord,
                        new TerrainChunk(viewedChunkCoord, chunkSize, LodInfos, transform, mapMaterial));
                }
        }
    }
    
    public TerrainType GetTerrainTypeOfPosition(Vector2 pos)
    {
        if (!IsOnLoadedChunk(pos)) return new TerrainType(); //TODO load specific chunk of map
        TerrainChunk chunk = GetChunkByLocation(pos);
        throw new NotImplementedException();
        return new TerrainType();
    }

    private bool IsOnLoadedChunk(Vector2 pos)
    {
        var currChunkCordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        var currChunkCordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);
        return TerrainChunks.ContainsKey(new Vector2(currChunkCordX, currChunkCordY));
    }
    
    private TerrainChunk GetChunkByLocation(Vector2 pos)
    {
        var currChunkCordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        var currChunkCordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);
        return TerrainChunks[new Vector2(currChunkCordX, currChunkCordY)];
    }

    public class TerrainChunk
    {
        private readonly LODMesh[] _lodMeshes;

        private MapData _mapData;
        private Bounds bounds;
        private readonly LodInfo[] detailLevels;
        private bool mapDataReceived;
        private readonly MeshCollider meshCollider;
        private readonly MeshFilter meshFilter;
        private readonly GameObject meshObject;

        private readonly MeshRenderer meshRenderer;
        private readonly Vector2 position;
        private int prevLodIndex = -1;

        public TerrainChunk(Vector2 coord, int size, LodInfo[] detailLevels, Transform parent, Material material)
        {
            this.detailLevels = detailLevels;
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            var positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshCollider = meshObject.AddComponent<MeshCollider>();
            meshRenderer.material = material;

            meshObject.transform.position = positionV3;
            meshObject.transform.parent = parent;
            SetVisible(false);

            _lodMeshes = new LODMesh[detailLevels.Length];
            for (var i = 0; i < detailLevels.Length; i++) _lodMeshes[i] = new LODMesh(detailLevels[i].lod);

            mapGenerator.RequestMapData(position, OnMapDataReceived);
        }

        private void OnMapDataReceived(MapData mapData)
        {
            _mapData = mapData;
            mapDataReceived = true;

            var texture = TextureGenerator.TextureFromColorMap(mapData.colorMap, MapGenerator.mapChunkSize,
                MapGenerator.mapChunkSize);
            meshRenderer.material.mainTexture = texture;
        }
        

        public void UpdateChunk()
        {
            if (!mapDataReceived) return;
            var viewerDst = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            var visible = viewerDst <= maxViewDistance;

            if (visible) {
                var lodIndex = 0;
                for (var i = 0; i < detailLevels.Length - 1; i++)
                    if (viewerDst > detailLevels[i].visibleDstThreshold) lodIndex = i + 1;
                    else break;

                if (lodIndex != prevLodIndex) {
                    var lodMesh = _lodMeshes[lodIndex];
                    if (lodMesh.hasMesh) {
                        prevLodIndex = lodIndex;
                        meshFilter.mesh = lodMesh.mesh;
                        if (detailLevels[lodIndex].useForCollider) {
                            print("Collision test");
                            meshCollider.sharedMesh = lodMesh.mesh;
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
            meshObject.SetActive(val);
        }

        public bool isVisible()
        {
            return meshObject.activeSelf;
        }
    }

    public class LODMesh
    {
        public bool hasMesh;
        private readonly int lod;
        public Mesh mesh;
        public bool requestedMesh;

        public LODMesh(int lod)
        {
            this.lod = lod;
        }

        private void onMeshDataReceived(MeshData meshData)
        {
            mesh = meshData.CreateMesh();
            hasMesh = true;
        }

        public void RequestMesh(MapData mapData)
        {
            requestedMesh = true;
            mapGenerator.RequestMeshData(mapData, lod, onMeshDataReceived);
        }
    }

    [Serializable]
    public struct LodInfo
    {
        public int lod;
        public float visibleDstThreshold;
        public bool useForCollider;
    }
}