using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreeController : MonoBehaviour
{
    [SerializeField] private GameObject treeTemplate;
    [SerializeField] private MeshGenerator meshGenerator;
    private Mesh _mesh;
    private bool _instantiatedTrees = false;
    // Start is called before the first frame update
    void Start()
    {
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!_instantiatedTrees)
        {
            _mesh = meshGenerator.mesh;
            
            int width = meshGenerator.zSize;
            int depth = meshGenerator.xSize;
            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < depth; j++)
                {
                    if(Random.value > 0.01) continue;
                    
                    Vector3 vertex = _mesh.vertices[i * width + j];
                    Vector3 newPos = new Vector3(vertex.x, vertex.y, vertex.z);
                    Instantiate(treeTemplate, newPos, new Quaternion());
                }
            }
            treeTemplate.SetActive(false);
            _instantiatedTrees = true;
        }
    }
}
