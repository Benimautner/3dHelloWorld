using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MapManager : MonoBehaviour
{
    public GameObject cubeTemplate;
    
    private Random _random = new Random();
    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < 50; i++)
        {
            for (var j = 0; j < 50; j++)
            {
                for (var k = 0; k < 50; k++)
                {
                    int x = _random.Next(0, 1);
                    Instantiate(cubeTemplate, new Vector3(i, x, k), Quaternion.identity);
                    print("x: " + i + " y: " + j + " z: " + k);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
