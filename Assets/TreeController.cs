using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class TreeController : MonoBehaviour
{
    [SerializeField] private GameObject TreeTemplate;
    [SerializeField] private MeshGenerator _meshGenerator;
    private Mesh _mesh;
    private bool InstantiatedTrees = false;
    // Start is called before the first frame update
    void Start()
    {
    }
    

    // Update is called once per frame
    void Update()
    {
        if (!InstantiatedTrees)
        {
            _mesh = _meshGenerator.mesh;
            
            int width = _meshGenerator.zSize;
            int depth = _meshGenerator.xSize;
            
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < depth; j++)
                {
                    if(Random.value > 0.01) continue;
                    
                    Vector3 vertex = _mesh.vertices[i * width + j];
                    Vector3 newPos = new Vector3(vertex.x, vertex.y, vertex.z);
                    Instantiate(TreeTemplate, newPos, new Quaternion());
                }
            }
            TreeTemplate.SetActive(false);
            InstantiatedTrees = true;
        }
    }
}
