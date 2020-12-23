using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneController : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private MeshFilter _filter;
    [SerializeField] private MeshCollider _collider;
    [SerializeField] private float mapScale;
    [SerializeField] private float heightMultiplier;
    void Start()
    {
        GenerateFloor(new Vector2(0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateFloor(Vector2 offset)
    {
        Vector3[] mesh = _filter.mesh.vertices;
        int depth = (int) Mathf.Sqrt(mesh.Length);
        int width = depth;

        float[,] heightMap = GenerateNoiseMap(depth, width, mapScale);
        Texture2D floorTexture = BuildTexture(heightMap);
        _renderer.material.mainTexture = floorTexture;

        int id_vertex = 0;
        for (int id_z = 0; id_z < depth; id_z++)
        {
            for (int id_x = 0; id_x < width; id_x++)
            {
                float height = heightMap[id_z, id_x];
                Vector3 vertex = mesh[id_vertex];
                mesh[id_vertex] = new Vector3(vertex.x, height * heightMultiplier, vertex.z);
                id_vertex++;
            }
        }

        _filter.mesh.vertices = mesh;
        _filter.mesh.RecalculateBounds();
        _filter.mesh.RecalculateNormals();

        _collider.sharedMesh = _filter.mesh;
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

                float noise = Mathf.PerlinNoise(sampleX, sampleZ);
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
