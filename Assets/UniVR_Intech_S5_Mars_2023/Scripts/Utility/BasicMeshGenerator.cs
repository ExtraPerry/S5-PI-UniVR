using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class BasicMeshGenerator : MonoBehaviour
{
    private Mesh mesh;

    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;
    private Color[] colors;

    [Min(0)]
    public int xSize = 20;
    [Min(0)]
    public int zSize = 20;
    public float scale = 1;

    public float minTerrainHeight = -2f;
    public float maxTerrainHeight = 5f;
    

    public Gradient gradient;
    public bool generateHeatMapGradient = false;
            
    

    // Start is called before the first frame update
    void Start()
    {
        if (generateHeatMapGradient)
        {
            gradient.SetKeys(
                new GradientColorKey[5]
                {
                    new GradientColorKey(new Color(0f, 0f, 1f), 0f),
                    new GradientColorKey(new Color(0f, 1f, 1f), 0.25f),
                    new GradientColorKey(new Color(1f, 1f, 0f), 0.5f),
                    new GradientColorKey(new Color(1f, 0.5f, 0f), 0.75f),
                    new GradientColorKey(new Color(1f, 0f, 0f), 1f)
                },
                new GradientAlphaKey[2]
                {
                    new GradientAlphaKey(1f, 1f),
                    new GradientAlphaKey(1f, 1f)
                }
                );
        }
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
    }

    private void Update()
    {
        UpdateMesh();
    }

    private void OnValidate()
    {
        CreateShape();
    }

    public void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.Lerp(minTerrainHeight, maxTerrainHeight, Noise.GenerateNoise(x, z, scale));
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        uvs = new Vector2[vertices.Length];
        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);

                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    private void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        if (vertices is null) return;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}
