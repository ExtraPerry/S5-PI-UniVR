using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapDisplay))]
public class MapGenerator : MonoBehaviour
{
    public FilterMode textureFilterMode = FilterMode.Point;

    public int mapWidth = 256;
    public int mapHeight = 256;
    public float noiseScale = 3;

    public int octaves = 4;
    public float persistence = 0.5f;
    public float lacunarity = 2;

    public bool autoUpdate = true;

    public MapDisplay display;

    private void Awake()
    {
        display = gameObject.GetComponent<MapDisplay>();
    }

    private void OnValidate()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        display.DrawNoiseMap(
            Noise.GenerateNoiseMap(mapWidth, mapHeight, noiseScale, octaves, persistence, lacunarity),
            textureFilterMode
        );
    }
}
