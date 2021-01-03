using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve locHeightCurve,
        int levelOfDetail)
    {
        var heightCurve = new AnimationCurve(locHeightCurve.keys);
        var width = heightMap.GetLength(0);
        var height = heightMap.GetLength(1);
        var topLeftX = (width - 1) / -2f;
        var topLeftZ = (height - 1) / 2f;

        var meshIncrement = levelOfDetail == 0 ? 1 : levelOfDetail * 2;
        var verticesPerLine = (width - 1) / meshIncrement + 1;

        var meshData = new MeshData(verticesPerLine, verticesPerLine);
        var vertexIndex = 0;
        for (var y = 0; y < height; y += meshIncrement){
            for (var x = 0; x < width; x += meshIncrement) {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x,
                    heightCurve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float) width, y / (float) height);
                if (x < width - 1 && y < height - 1) {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
                    meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData
{
    public Vector2[] uvs;
    public int[] triangles;
    public Vector3[] vertices;
    private int _triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        uvs = new Vector2[meshHeight * meshWidth];
        vertices = new Vector3[meshHeight * meshWidth];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[_triangleIndex] = a;
        triangles[_triangleIndex + 1] = b;
        triangles[_triangleIndex + 2] = c;
        _triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}