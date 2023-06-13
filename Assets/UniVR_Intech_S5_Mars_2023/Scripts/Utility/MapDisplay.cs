using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MapDisplay : MonoBehaviour
{
    public MeshFilter meshFilter;
    public Renderer textureRenderer;

    private void Awake()
    {
        meshFilter = gameObject.GetComponent<MeshFilter>();
        textureRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    public void DrawNoiseMap(float[,] noiseMap, FilterMode filterMode)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] coulourMap = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                coulourMap[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }
        texture.SetPixels(coulourMap);
        texture.filterMode = filterMode;
        texture.Apply();
        
        textureRenderer.sharedMaterial.mainTexture = texture;
    }
}
