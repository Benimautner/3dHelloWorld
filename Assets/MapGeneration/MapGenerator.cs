using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap,
        ColorMap,
        Mesh
    }

    public DrawMode drawMode;
    public const int MapChunkSize = 241;
    public FeatureGenerator featureGenerator;
    [Range(0, 6)] public int editorLevelOfDetail;
    
    public float scale = 1;
    public bool autoUpdate;
    public int octaves = 3;
    public float meshHeightMultiplier = 5;
    [Range(0, 1)] public float persistance = 1f;
    
    public int seed;
    public Vector2 offset;
    public float lacunarity = 0.9f;
    public List<TerrainType> regions;
    public AnimationCurve meshHeightCurve;
    public ChunkProperties[] chunkPropertiesArray;
    
    private readonly Queue<GameObjectQueueObject> _gameObjectQueue = new Queue<GameObjectQueueObject>();
    private readonly Queue<MapThreadInfo<MapData>> _mapDataThreadQueue = new Queue<MapThreadInfo<MapData>>();
    private readonly Queue<MapThreadInfo<MeshData>> _meshDataThreadQueue = new Queue<MapThreadInfo<MeshData>>();



    private void Update()
    {
        lock (_mapDataThreadQueue) {
            if (_mapDataThreadQueue.Count > 0) {
                for (var i = 0; i < _mapDataThreadQueue.Count; i++) {
                    var threadInfo = _mapDataThreadQueue.Dequeue();
                    threadInfo.callback(threadInfo.parameter);
                }
            }
        }

        lock (_meshDataThreadQueue) {
            if (_meshDataThreadQueue.Count > 0) {
                for (var i = 0; i < _meshDataThreadQueue.Count; i++) {
                    var threadInfo = _meshDataThreadQueue.Dequeue();
                    threadInfo.callback(threadInfo.parameter);
                }
            }
        }

        lock (_gameObjectQueue) {
            if (_gameObjectQueue.Count > 0) {
                for (int i = 0; i < _gameObjectQueue.Count; i++) {
                    var gameObject = _gameObjectQueue.Dequeue();
                    Instantiate(gameObject.gameObject, gameObject.pos, gameObject.quaternion);
                }
            }
        }
    }

    private void OnValidate()
    {
        if (lacunarity < 1) lacunarity = 1;
        if (octaves < 0) octaves = 0;
        persistance = Mathf.Clamp(persistance, 0, 1);
    }


    public void DrawMapInEditor()
    {
        var mapData = GenerateMapData(Vector2.zero);
        var display = FindObjectOfType<MapDisplay>();

        if (drawMode == DrawMode.NoiseMap)
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        else if (drawMode == DrawMode.ColorMap)
            display.DrawTexture(TextureGenerator.TextureFromColorMap(mapData.colorMap, MapChunkSize, MapChunkSize));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(
                MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve,
                    editorLevelOfDetail),
                TextureGenerator.TextureFromColorMap(mapData.colorMap, MapChunkSize, MapChunkSize));
    }

    private MapData GenerateMapData(Vector2 center)
    {
        var noiseMap = Noise.GeneratePerlinNoiseMap(MapChunkSize, MapChunkSize, seed, scale,
            octaves, persistance, lacunarity, center + offset, Noise.NormalizeMode.Global);

        var colorMap = new Color[MapChunkSize * MapChunkSize];
        for (var y = 0; y < MapChunkSize; y++){
            for (var x = 0; x < MapChunkSize; x++) {
                var currHeight = noiseMap[x, y];
                TerrainType currRegion = GetTerrainTypeByHeight(currHeight);
                colorMap[y * MapChunkSize + x] = currRegion.color;
            }
        }

        return new MapData(noiseMap, colorMap, meshHeightMultiplier);
    }

    public void RequestGameObjectData(GameObjectThreadInfo info, Action<List<GameObject>> callback)
    {
        ThreadStart threadStart = delegate { GameObjectThread(info, callback); };
        new Thread(threadStart).Start();
    }

    private void GameObjectThread(GameObjectThreadInfo info, Action<List<GameObject>> callback)
    {
        List<GameObjectQueueObject> trees = info.featureGenerator.GenerateTrees(info.vertices, info.size, info.heightMultiplier, info.mapGenerator, info.position - offset);
        var newTrees = trees;

        lock (_gameObjectQueue) {
            trees.ForEach(tree => _gameObjectQueue.Enqueue(tree));
        }
    }

    public void RequestMapData(Vector2 center, Action<MapData> callback)
    {
        ThreadStart threadStart = delegate { MapDataThread(center, callback); };
        new Thread(threadStart).Start();
    }

    private void MapDataThread(Vector2 center, Action<MapData> callback)
    {
        var mapData = GenerateMapData(center);
        lock (_mapDataThreadQueue) {
            _mapDataThreadQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    }

    public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate { MeshDataThread(mapData, lod, callback); };
        new Thread(threadStart).Start();
    }

    private void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback)
    {
        var meshData =
            MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, lod);
        lock (_meshDataThreadQueue) {
            _meshDataThreadQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
        }
    }

    public TerrainType GetTerrainTypeByHeight(float height)
    {
        return regions.Find(region => region.height > height);
    }
}
