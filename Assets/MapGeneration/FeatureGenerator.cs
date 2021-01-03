using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class FeatureGenerator : MonoBehaviour
{
    [SerializeField] public GameObject treeTemplate;
    [SerializeField] private float chanceOfTree;
    public List<GameObjectQueueObject> GenerateTrees(float[,] noiseMap, MapGenerator mapGenerator)
    {
        List<GameObjectQueueObject> trees = new List<GameObjectQueueObject>();
        Random pRandom = new Random();
        for (int y = 0; y < noiseMap.GetLength(0); y++) {
            for (int x = 0; x < noiseMap.GetLength(1); x++) {
                if (pRandom.NextDouble() <= chanceOfTree &&
                    mapGenerator.GetTerrainTypeByHeight(noiseMap[x, y]).name.Equals("Land")) {
                    print("instantiating");
                    GameObjectQueueObject tree = new GameObjectQueueObject(treeTemplate, new Vector3(x, noiseMap[x, y], y));
                    trees.Add(tree);
                }
            }
        }
        return trees;
    }
}
