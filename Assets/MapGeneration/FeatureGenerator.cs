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

        foreach (var vertex in vertices) {
            float scaledHeight = SharedInfo.inverseHeightCurve.Evaluate(vertex.y/meshData.heightMultiplier);
            

            if (mapGenerator.GetTerrainTypeByHeight(scaledHeight).name.Equals("Land")) {
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
