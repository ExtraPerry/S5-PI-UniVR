using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapDisplay))]
public class MapGenerator : MonoBehaviour
{
    public DrawMode drawMode = DrawMode.HeatMap;
    public MeshMode meshMode = MeshMode.Default;
    public FilterMode filterMode = FilterMode.Bilinear;

    [HideInInspector]
    public const int mapChunkSize = 241;
    [Range(0, 6)]
    public int levelOfDetail = 6;
    public int seed = 158;
    public float scale = 25;
    public int octaves = 5;
    public float persistance = 0.5f;
    public float lacunarity = 2;
    public Vector2 offset = new Vector2(0, 0);

    [Min(1)]
    public float amplitude = 1;
    public AnimationCurve meshAmplitudeCurve;

    public TerrainType[] regions = new TerrainType[8]
    {
        new TerrainType("Water Deep", 0.3f, new Color(0.2039216f, 0.3764706f, 0.737255f, 1f)),
        new TerrainType("Water Shallow", 0.4f, new Color(0.2f, 0.3882353f, 0.764706f, 1f)),
        new TerrainType("Sand", 0.45f, new Color(0.8117648f, 0.8156863f, 0.490196f, 1f)),
        new TerrainType("Grass", 0.55f, new Color(0.3372549f, 0.5921568f, 0.07843135f, 1f)),
        new TerrainType("Forest", 0.6f, new Color(0.2588235f, 0.4156863f, 0.0941176f, 1f)),
        new TerrainType("Mountain", 0.7f, new Color(0.3529412f, 0.2666667f, 0.2352941f, 1f)),
        new TerrainType("Mountain High", 0.9f, new Color(0.2745098f, 0.2196078f, 0.2039216f, 1f)),
        new TerrainType("Snow", 1f, new Color(1f, 1f, 1f, 1f))
    };

    public MapDisplay display;

    private void OnValidate()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, scale, octaves, persistance, lacunarity, offset);

        Texture2D texture;

        switch (drawMode)
        {
            case DrawMode.NoiseMap:
                texture = TextureGenerator.TextureFromHeightMap(noiseMap);
                break;

            case DrawMode.ColourMap:
                Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
                for (int y = 0; y < mapChunkSize; y++)
                {
                    for (int x = 0; x < mapChunkSize; x++)
                    {
                        float currentHeight = noiseMap[x, y];
                        for (int i = 0; i < regions.Length; i++)
                        {
                            if (currentHeight <= regions[i].height)
                            {
                                colourMap[y * mapChunkSize + x] = regions[i].colour;
                                break;
                            }
                        }
                    }
                }
                texture = TextureGenerator.TextureFromColourMap(colourMap, mapChunkSize, mapChunkSize);
                break;

            case DrawMode.HeatMap:
                texture = TextureGenerator.TextureFromGradientHeight(noiseMap, Noise.GetHeatMapGradient());
                break;

            default:
                texture = TextureGenerator.TextureBlankWhite();
                break;
        }

        texture.filterMode = filterMode;
        texture.wrapMode = TextureWrapMode.Clamp;

        switch (meshMode)
        {
            case MeshMode.Terrain:
                display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, amplitude, meshAmplitudeCurve, levelOfDetail), texture);
                break;

            default:
                display.DrawMesh(MeshGenerator.GeneratePlaneMesh(mapChunkSize, mapChunkSize), texture);
                break;
        }

        
        
    }
}

public enum DrawMode
{
    White,
    NoiseMap,
    HeatMap,
    ColourMap
}

public enum MeshMode
{
    Default,
    Terrain
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;

    public TerrainType(string name, float height, Color colour)
    {
        this.name = name;
        this.height = height;
        this.colour = colour;
    }
}
