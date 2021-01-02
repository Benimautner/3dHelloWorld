using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class PlaneController : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] private new MeshRenderer renderer;
    [SerializeField] private MeshFilter filter;
    [SerializeField] private new MeshCollider collider;
    [SerializeField] private float mapScale;
    [SerializeField] public float heightMultiplier;
    [SerializeField] public Vector3[] mesh;
    [SerializeField] public Vector3 offset;
    [SerializeField] public GameObject player;
    void Start()
    {
        GenerateFloor();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GenerateFloor()
    {
        Vector3[] locMesh = filter.mesh.vertices;
        int depth = (int) Mathf.Sqrt(locMesh.Length);
        int width = depth;

        float[,] heightMap = GenerateNoiseMap(depth, width, mapScale);
        Texture2D floorTexture = BuildTexture(heightMap);
        renderer.material.mainTexture = floorTexture;

        int idVertex = 0;
        for (int idZ = 0; idZ < depth; idZ++)
        {
            for (int idX = 0; idX < width; idX++)
            {
                float height = heightMap[idZ, idX];
                Vector3 vertex = locMesh[idVertex];
                locMesh[idVertex] = new Vector3(vertex.x, height * heightMultiplier, vertex.z);
                idVertex++;
            }
        }

        mesh = locMesh;
        filter.mesh.vertices = locMesh;
        filter.mesh.RecalculateBounds();
        filter.mesh.RecalculateNormals();

        collider.sharedMesh = filter.mesh;
    }

    
    float[,] GenerateNoiseMap(int depth, int width, float scale)
    {
        float[,] noiseMap = new float[depth, width];
        for (int id_z = 0; id_z < depth; id_z++)
        {
            for (int id_x = 0; id_x < width; id_x++)
            {
                float sampleX = id_x / scale;
                float sampleZ = id_z / scale;

                float noise = Mathf.PerlinNoise(sampleX + offset.x, sampleZ + offset.y);
                noiseMap[id_z, id_x] = noise;
            }
        }

        return noiseMap;
    }

    private Texture2D BuildTexture(float[,] heightMap)
    {
        int depth = heightMap.GetLength(0);
        int width = heightMap.GetLength(1);

        Color[] colorMap = new Color[depth * width];
        for (int id_z = 0; id_z < depth; id_z++)
        {
            for (int id_x = 0; id_x < width; id_x++)
            {
                int clrIndex = id_z * width + id_x;
                float height = heightMap[id_z, id_x];
                colorMap[clrIndex] = Color.Lerp(Color.black, Color.white, height);
            }
        }

        Texture2D returnTexture = new Texture2D(width, depth);
        returnTexture.wrapMode = TextureWrapMode.Clamp;
        returnTexture.SetPixels(colorMap);
        returnTexture.Apply();
        return returnTexture;
    }
}
