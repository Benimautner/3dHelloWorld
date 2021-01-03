using UnityEngine;

public class TreeController : MonoBehaviour
{
    [SerializeField] private GameObject treeTemplate;
    [SerializeField] private MapGenerator meshGenerator;
    private bool _instantiatedTrees = false;

    private Mesh _mesh;

    // Start is called before the first frame update
    private void Start()
    {
    }


    // Update is called once per frame
    private void Update()
    {
        //    if (!_instantiatedTrees)
        //    {
        //        _mesh = meshGenerator.mesh;
        //        
        //        int width = meshGenerator.getChunkSize();
        //        int depth = meshGenerator.getChunkSize();
        //        
        //        for (int i = 0; i < width; i++)
        //        {
        //            for (int j = 0; j < depth; j++)
        //            {
        //                if(Random.value > 0.01) continue;
        //                
        //                Vector3 vertex = _mesh.vertices[i * width + j];
        //                Vector3 newPos = new Vector3(vertex.x, vertex.y, vertex.z);
        //                Instantiate(treeTemplate, newPos, new Quaternion());
        //            }
        //        }
        //        treeTemplate.SetActive(false);
        //        _instantiatedTrees = true;
        //    }
    }
}