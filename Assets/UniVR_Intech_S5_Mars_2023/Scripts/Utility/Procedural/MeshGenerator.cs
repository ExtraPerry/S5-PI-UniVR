using UnityEngine;
using System.Collections;

public static class MeshGenerator
{

	public static MeshData GenerateTerrainMeshData(float[,] heightMap, float amplitude, AnimationCurve _meshAmplitudeCurve, int levelOfDetail)
	{
		AnimationCurve meshAmplitudeCurve = new AnimationCurve(_meshAmplitudeCurve.keys);

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

	public static MeshData GeneratePlaneMeshData(int width, int height)
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

	private Vector3[] CalculateNormals()
    {
		Vector3[] vertexNormals = new Vector3[vertices.Length];
		int triangleCount = triangles.Length / 3;
		for (int i = 0; i < triangleCount; i++)
		{
			int normalTriangleIndex = i * 3;
			int vertexIndexA = triangles[normalTriangleIndex];
			int vertexIndexB = triangles[normalTriangleIndex + 1];
			int vertexIndexC = triangles[normalTriangleIndex + 2];

			Vector3 triangleNormal = SurfaceNormalFromIndices(vertexIndexA, vertexIndexB, vertexIndexC);
			vertexNormals[vertexIndexA] += triangleNormal;
			vertexNormals[vertexIndexB] += triangleNormal;
			vertexNormals[vertexIndexC] += triangleNormal;
		}

		for (int i = 0; i < vertexNormals.Length; i++)
		{
			vertexNormals[i].Normalize();
		}

		return vertexNormals;
	}

	private Vector3 SurfaceNormalFromIndices(int indexA, int indexB, int indexC)
    {
		Vector3 pointA = vertices[indexA];
		Vector3 pointB = vertices[indexB];
		Vector3 pointC = vertices[indexB];

		Vector3 sideAB = pointB - pointA;
		Vector3 sideAC = pointC - pointA;

		return Vector3.Cross(sideAB, sideAC).normalized;
	}

	public Mesh CreateMesh()
	{
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.normals = CalculateNormals();
		return mesh;
	}

}