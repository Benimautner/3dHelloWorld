using UnityEngine;
using Random = System.Random;

public static class Noise
{
    public enum NormalizeMode
    {
        Local,
        Global
    }

    public static float[,] GeneratePerlinNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves,
        float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode)
    {
        scale = Mathf.Clamp(scale, 0.0001f, scale + 1f);
        float amplitude = 1;
        float frequency = 1;
        var noiseMap = new float[mapWidth, mapHeight];
        var prng = new Random(seed);
        var octaveOffset = new Vector2[octaves];

        float maxPossibleHeight = 0;

        for (var i = 0; i < octaves; i++) {
            var offsetX = prng.Next(-100000, 100000) + offset.x;
            var offsetY = prng.Next(-100000, 100000) - offset.y;
            octaveOffset[i] = new Vector2(offsetX, offsetY);
            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        var maxNoiseHeight = float.MinValue;
        var minNoiseHeight = float.MaxValue;

        var halfWidth = mapWidth / 2f;
        var halfHeight = mapHeight / 2f;


        for (var y = 0; y < mapHeight; y++) {
            for (var x = 0; x < mapWidth; x++) {
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (var i = 0; i < octaves; i++) {
                    var sampleX = (x - halfWidth + octaveOffset[i].x) / scale * frequency;
                    var sampleZ = (y - halfHeight + octaveOffset[i].y) / scale * frequency;
                    var perlinNoise = Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1;
                    noiseHeight += perlinNoise * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight) maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight) minNoiseHeight = noiseHeight;
                noiseMap[x, y] = noiseHeight;
            }
        }


        for (var y = 0; y < mapHeight; y++) {
            for (var x = 0; x < mapWidth; x++)
                switch (normalizeMode) {
                    case NormalizeMode.Local:
                        noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                        break;
                    case NormalizeMode.Global:
                        noiseMap[x, y] = (noiseMap[x, y] + 1) / (2f * maxPossibleHeight / 1.1f);
                        break;
                }
        }
        return noiseMap;
    }
}