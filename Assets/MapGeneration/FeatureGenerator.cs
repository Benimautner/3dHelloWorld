using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = System.Random;

public class FeatureGenerator : MonoBehaviour
{
    [SerializeField] public GameObject treeTemplate;
    [SerializeField] private float chanceOfTree;
    /*public List<GameObjectQueueObject> GenerateTrees(Vector3[] vertices, MapGenerator mapGenerator, Vector2 offset, ChunkProperties chunkProperties)
    {
        List<GameObjectQueueObject> trees = new List<GameObjectQueueObject>();
        Random pRandom = new Random();
        foreach (var vertex in vertices) {
            var scaledHeight = chunkProperties.invertedCurve.Evaluate(vertex.y/chunkProperties.scale);
            string terrName = GetTerrainTypeByHeight(scaledHeight, SharedInfo.chunkPropertiesList.Find(n => n.name.Equals("Land"))).name;
            if (terrName == null) { continue; }
            
            if (terrName.Equals("T_Land")) {
                if (pRandom.NextDouble() <= chanceOfTree) {
                    GameObjectQueueObject tree =
                        new GameObjectQueueObject(treeTemplate, new Vector3(vertex.x + offset.x, vertex.y, vertex.z + offset.y));
                    trees.Add(tree);
                }
            }
        }
        return trees;
    }*/

    public List<GameObject> GenerateTrees(TerrainData terrainData)
    {
        List<GameObject> trees = new List<GameObject>();

        for (int y = 0; y < terrainData.alphamapHeight; y++) {
            for (int x = 0; x < terrainData.alphamapWidth; x++) {
            }
        }


        return trees;
    }
    
    
    
}
