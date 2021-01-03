using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ChunkProperties
{
    public string name;
    public float scale;
    public int octaves;
    public float persistance;
    public float lacunarity;
}

[Serializable]
public struct WorldProperties
{
    public int seed;
}