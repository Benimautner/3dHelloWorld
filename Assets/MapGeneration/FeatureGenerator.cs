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
    public List<GameObjectQueueObject> GenerateTrees(Vector3[] vertices, MapGenerator mapGenerator, Vector2 offset, MeshData meshData)
    {
        List<GameObjectQueueObject> trees = new List<GameObjectQueueObject>();
        Random pRandom = new Random();
        //print(SharedInfo.chunkPropertiesList.Find(n=> n.name.Equals("Land")).invertedCurve.Evaluate(0.5f));
        foreach (var vertex in vertices) { 
            float scaledHeight = SharedInfo.chunkPropertiesList.Find(n=> n.name.Equals("Land")).invertedCurve.Evaluate(vertex.y/meshData.heightMultiplier);
            
            string terrName = mapGenerator.GetTerrainTypeByHeight(scaledHeight, SharedInfo.chunkPropertiesList.Find(n => n.name.Equals("Land"))).name;
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
    }
}
