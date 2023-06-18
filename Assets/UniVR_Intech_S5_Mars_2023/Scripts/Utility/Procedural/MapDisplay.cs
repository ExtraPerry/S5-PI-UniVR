using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MapDisplay : MonoBehaviour
{
    public MeshFilter meshFilter;
    public Renderer textureRenderer;

    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        DrawTexture(texture);
    }
    public void DrawMesh(Mesh mesh, Texture2D texture)
    {
        meshFilter.sharedMesh = mesh;
        DrawTexture(texture);
    }
}
