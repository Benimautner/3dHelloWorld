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

    public FeatureGenerator featureGenerator;

    public const int mapChunkSize = 241;

    public DrawMode drawMode;
    
    [Range(0, 6)] public int editorLevelOfDetail;

    public float scale = 1;
    public bool autoUpdate;
    public int octaves = 3;
    public float meshHeightMultiplier = 5;

    [Range(0, 1)] public float persistance = 1f;

    public float lacunarity = 0.9f;
    public int seed;
    public Vector2 offset;
    public List<TerrainType> regions;
    public ChunkProperties[] chunkPropertiesArray;

    public AnimationCurve meshHeightCurve;

    private readonly Queue<MapThreadInfo<MapData>> _mapDataThreadQueue = new Queue<MapThreadInfo<MapData>>();
    private readonly Queue<MapThreadInfo<MeshData>> _meshDataThreadQueue = new Queue<MapThreadInfo<MeshData>>();

    private readonly Queue<GameObjectQueueObject> _gameObjectQueue = new Queue<GameObjectQueueObject>();


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
            display.DrawTexture(TextureGenerator.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize));
        else if (drawMode == DrawMode.Mesh)
            display.DrawMesh(
                MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve,
                    editorLevelOfDetail),
                TextureGenerator.TextureFromColorMap(mapData.colorMap, mapChunkSize, mapChunkSize));
    }

    private MapData GenerateMapData(Vector2 center)
    {
        var noiseMap = Noise.GeneratePerlinNoiseMap(mapChunkSize, mapChunkSize, seed, scale,
            octaves, persistance, lacunarity, center + offset, Noise.NormalizeMode.Global);

        var colorMap = new Color[mapChunkSize * mapChunkSize];
        for (var y = 0; y < mapChunkSize; y++){
            for (var x = 0; x < mapChunkSize; x++) {
                var currHeight = noiseMap[x, y];
                TerrainType currRegion = GetTerrainTypeByHeight(currHeight);
                colorMap[y * mapChunkSize + x] = currRegion.color;
            }
        }

        List<GameObjectQueueObject> trees = featureGenerator.GenerateTrees(noiseMap, this);
        trees.ForEach(tree => _gameObjectQueue.Enqueue(tree));

        return new MapData(noiseMap, colorMap);
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
        return regions.Find(type => type.height >= height);
    }
}

[Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}

public struct MapData
{
    public float[,] heightMap;
    public Color[] colorMap;

    public MapData(float[,] heightMap, Color[] colorMap)
    {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }
}

public struct MapThreadInfo<T>
{
    public readonly Action<T> callback;
    public readonly T parameter;

    public MapThreadInfo(Action<T> callback, T parameter)
    {
        this.callback = callback;
        this.parameter = parameter;
    }
}

public struct GameObjectQueueObject
{
    public GameObject gameObject;
    public Vector3 pos;
    public Quaternion quaternion;

    public GameObjectQueueObject(GameObject _gameObject, Vector3 _pos, Quaternion _quat = new Quaternion())
    {
        gameObject = _gameObject;
        pos = _pos;
        quaternion = _quat;
    }
}