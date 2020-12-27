using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    [SerializeField] private GameObject TreeTemplate;

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
            GameObject plane = GameObject.Find("Plane");
            PlaneController planeController = plane.GetComponent(typeof(PlaneController)) as PlaneController;

            Vector3[] mesh = planeController.mesh;
            int width = (int) Mathf.Sqrt(mesh.Length);
            int depth = width;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < depth; j++)
                {
                    Vector3 vertex = mesh[i * width + j];
                    Vector3 newPos = new Vector3(vertex.x * 10, vertex.y, vertex.z * 10);
                    Instantiate(TreeTemplate, newPos, new Quaternion());
                }
            }
            TreeTemplate.SetActive(false);
            InstantiatedTrees = true;
        }
    }
}
