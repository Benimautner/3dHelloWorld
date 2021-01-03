using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LodInfo
{
    public int lod;
    public bool useForCollider;
    public float visibleDstThreshold;
}

[Serializable]
public struct TerrainType
{
    public string name;
    public Color color;
    public float height;
}

public struct MapData
{
    public Color[] colorMap;
    public float[,] heightMap;

    public MapData(float[,] heightMap, Color[] colorMap)
    {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }
}

public struct MapThreadInfo<T>
{
    public readonly T parameter;
    public readonly Action<T> callback;

    public MapThreadInfo(Action<T> callback, T parameter)
    {
        this.callback = callback;
        this.parameter = parameter;
    }
}

public struct GameObjectQueueObject
{
    public Vector3 pos;
    public Quaternion quaternion;
    public readonly GameObject gameObject;

    public GameObjectQueueObject(GameObject gameObject, Vector3 pos, Quaternion quaternion = new Quaternion())
    {
        this.gameObject = gameObject;
        this.pos = pos;
        this.quaternion = quaternion;
    }
}

[Serializable]
public struct ChunkProperties
{
    public string name;
    public float scale;
    public int octaves;
    public float lacunarity;
    public float persistance;
}

[Serializable]
public struct WorldProperties
{
    public int seed;
}