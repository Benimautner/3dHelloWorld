using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject cubeTemplate;
    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < 5; i++)
        {
            for (var j = 0; j < 5; j++)
            {
                for (var k = 0; k < 5; k++)
                {
                    Instantiate(cubeTemplate, new Vector3(i, 5, k), Quaternion.identity);
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
