using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class FeatureGenerator : MonoBehaviour
{
    [SerializeField] public GameObject treeTemplate;
    [SerializeField] private float chanceOfTree;
    public List<GameObjectQueueObject> GenerateTrees(Vector3[] vertices, int size, float scale, MapGenerator mapGenerator, Vector2 offset)
    {
        List<GameObjectQueueObject> trees = new List<GameObjectQueueObject>();
        Random pRandom = new Random();

        foreach (var vertex in vertices) {
            if (mapGenerator.GetTerrainTypeByHeight(vertex.y/scale).name.Equals("Water")) {
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
