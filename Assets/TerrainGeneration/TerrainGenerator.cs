using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private TerrainData _terrainData;
    private bool _initialized = false;
    [SerializeField] private Terrain terrain;
    [SerializeField] private MapGenerator mapGenerator;
    private List<ChunkProperties> _chunkPropertiesList;
    private void Start()
    {
        _chunkPropertiesList = mapGenerator.locChunkPropertiesList;
    }

    private void Update()
    {
        if (!_initialized && SharedInfo.initialized) {
            _terrainData = terrain.terrainData;
            var heights = Noise.GeneratePerlinNoiseMap(_terrainData.heightmapResolution,
                _terrainData.heightmapResolution, mapGenerator.seed, mapGenerator.meshHeightMultiplier, 
                mapGenerator.octaves, mapGenerator.persistance, mapGenerator.lacunarity, new Vector2(0, 0),
                Noise.NormalizeMode.Local);

            ChunkProperties islandType = _chunkPropertiesList.Find(n => n.islandType == mapGenerator.islandType);

            for (var y = 0; y < _terrainData.heightmapResolution; y++) {
                for (var x = 0; x < _terrainData.heightmapResolution; x++) {
                    heights[x, y] = islandType.curve.Evaluate(heights[x, y]) / 3;
                    heights[x, y] *= islandType.dropoffCurve.Evaluate(Vector2.Distance(new Vector2(250, 250), new Vector2(x,y))/50);
                }
            }

            terrain.terrainData.SetHeights(0, 0, heights);


            
            float[, ,] splatmapData = new float[_terrainData.alphamapWidth, _terrainData.alphamapHeight, _terrainData.alphamapLayers];
         
        for (int y = 0; y < _terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < _terrainData.alphamapWidth; x++)
            {
                // Normalise x/y coordinates to range 0-1 
                float x1 = x/(float)_terrainData.alphamapWidth;
                float y1 = y/(float)_terrainData.alphamapHeight;
                 
                float height = _terrainData.GetHeight(Mathf.RoundToInt(y1 * _terrainData.heightmapResolution),
                    Mathf.RoundToInt(x1 * _terrainData.heightmapResolution) );
                
                Vector3 normal = _terrainData.GetInterpolatedNormal(y1,x1);
                
                float steepness = _terrainData.GetSteepness(y1,x1);
                 
                float[] splatWeights = new float[_terrainData.alphamapLayers];
                
                splatWeights[0] = 0.3f;

                
                var low = 0.0f;
                var high = islandType.terrainType[0].maxHeight;
                for (int i = 0; i < _terrainData.alphamapLayers; i++) {
                    low = islandType.terrainType[i].minHeight - 5;
                    high = islandType.terrainType[i].maxHeight + 5;
                    if (height > low && height < high) {
                        splatWeights[i] = Mathf.Clamp01((height-low)/(high-low));
                    }
                }
                
                /*
                int low = 0;
                int high = 35;
                if (height > 0.0f && height < high) {
                    splatWeights[1] = Mathf.Clamp01(height/high);
                }

                low = high - 10;
                high = 45;
                if (height > low && height < high) {
                    splatWeights[2] = Mathf.Clamp01((height-low)/(high-low));
                }

                low = high;
                if (height > low) {
                    splatWeights[3] = Mathf.Clamp01((height-low));
                }*/

                float z = splatWeights.Sum();
                 
                for(int i = 0; i<_terrainData.alphamapLayers; i++){
                     
                    splatWeights[i] /= z;
                     
                    splatmapData[x, y, i] = splatWeights[i];
                }
            }
        }
      
        // Finally assign the new splatmap to the terrainData:
        _terrainData.SetAlphamaps(0, 0, splatmapData);

        _initialized = true;
        
        }
    }

    private void OnApplicationQuit()
    {
        terrain.terrainData.SetHeights(0,0,new float[513,513]);
        }
}
