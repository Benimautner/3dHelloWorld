using System;
using System.Collections.Generic;
using System.Threading;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

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

    public int octaves = 3;
    public float meshHeightMultiplier = 5;
    [Range(0, 1)] public float persistance = 1f;

    public IslandType islandType;

    public int seed;
    public Vector2 offset;

    public float lacunarity = 0.9f;

    [FormerlySerializedAs("chunkPropertiesList")]
    public List<ChunkProperties> locChunkPropertiesList;
    
    private void Start()
    {
        SharedInfo.chunkPropertiesList = locChunkPropertiesList;

        List<ChunkProperties> cpl = SharedInfo.chunkPropertiesList;

        for (int ctr = 0; ctr < locChunkPropertiesList.Count; ctr++) {
            ChunkProperties chunkProperty = locChunkPropertiesList[ctr];
            var inverseCurve = new AnimationCurve();
            for (int j = 0; j < chunkProperty.curve.length; j++) {
                Keyframe inverseKey = new Keyframe(chunkProperty.curve.keys[j].value, chunkProperty.curve.keys[j].time);
                inverseCurve.AddKey(inverseKey);
            }

            ChunkProperties x = SharedInfo.chunkPropertiesList[ctr];
            x.invertedCurve = (inverseCurve);
            SharedInfo.chunkPropertiesList[ctr] = x;
        }


        for (int i = 0; i < cpl.Count; i++) {
            var meshHeightCurve = cpl[i].curve;
            var inverseCurve = new AnimationCurve();

            for (int j = 0; j < meshHeightCurve.length; j++) {
                Keyframe inverseKey = new Keyframe(meshHeightCurve.keys[j].value, meshHeightCurve.keys[j].time);
                inverseCurve.AddKey(inverseKey);
            }

            SharedInfo.chunkPropertiesList[i].SetInvertedCurve(inverseCurve);
            locChunkPropertiesList[i].SetInvertedCurve(inverseCurve);
        }

        SharedInfo.initialized = true;
    }
}

