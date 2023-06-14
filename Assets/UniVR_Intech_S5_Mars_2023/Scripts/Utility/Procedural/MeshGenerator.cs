using UnityEngine;
using System.Collections;

public static class MeshGenerator
{

	public static MeshData GenerateTerrainMesh(float[,] heightMap, float amplitude, AnimationCurve meshAmplitudeCurve, int levelOfDetail)
	{
		int width = heightMap.GetLength(0);
		int height = heightMap.GetLength(1);
		float topLeftX = (width - 1) / -2f;
		float topLeftZ = (height - 1) / 2f;

		int meshSimplificationIncrement = (levelOfDetail == 0) ? 1 : levelOfDetail * 2;
		int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;

		MeshData meshData = new MeshData(verticesPerLine, verticesPerLine);
		int vertexIndex = 0;

		for (int y = 0; y < height; y += meshSimplificationIncrement)
		{
			for (int x = 0; x < width; x += meshSimplificationIncrement)
			{

				meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, meshAmplitudeCurve.Evaluate(heightMap[x, y]) * amplitude, topLeftZ - y);
				meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

				if (x < width - 1 && y < height - 1)
				{
					meshData.AddTriangle(vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
					meshData.AddTriangle(vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
				}

				vertexIndex++;
			}
		}

		return meshData;

	}

	public static MeshData GeneratePlaneMesh(int width, int height)
    {
		float topLeftX = (width - 1) / -2f;
		float topLeftZ = (height - 1) / 2f;

		MeshData meshData = new MeshData(1, 1);

		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(topLeftX, 0, -topLeftZ),
			new Vector3(-topLeftX, 0, -topLeftZ),
			new Vector3(topLeftX, 0, topLeftZ),
			new Vector3(-topLeftX, 0, topLeftZ)
		};
		meshData.vertices = vertices;

		int[] triangles = new int[6]
		{
			0, 2, 1,
			2, 3, 1
		};
		meshData.triangles = triangles;

		Vector2[] uvs = new Vector2[4]
		{
			new Vector3(0, 0),
			new Vector3(1, 0),
			new Vector3(0, 1),
			new Vector3(1, 1)
		};
		meshData.uvs = uvs;

		return meshData;
	}
}

public class MeshData
{
	public Vector3[] vertices;
	public int[] triangles;
	public Vector2[] uvs;

	int triangleIndex;

	public MeshData(int meshWidth, int meshHeight)
	{
		vertices = new Vector3[meshWidth * meshHeight];
		uvs = new Vector2[meshWidth * meshHeight];
		triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
	}

	public void AddTriangle(int a, int b, int c)
	{
		triangles[triangleIndex] = a;
		triangles[triangleIndex + 1] = b;
		triangles[triangleIndex + 2] = c;
		triangleIndex += 3;
	}

	public Mesh CreateMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals();
		return mesh;
	}

}