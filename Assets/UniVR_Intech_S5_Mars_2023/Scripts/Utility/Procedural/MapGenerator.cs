using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

[RequireComponent(typeof(MapDisplay))]
public class MapGenerator : MonoBehaviour
{
    public MeshMode meshMode = MeshMode.Default;
    public NormalizeMode normalizedMode = NormalizeMode.Global;
    public DrawMode drawMode = DrawMode.HeatMap;
    public FilterMode filterMode = FilterMode.Point;

    [HideInInspector]
    public const int mapChunkSize = 241;
    [Range(0, 6)]
    public int editorLevelOfDetail = 6;
    public int seed = 158;
    [Min(1)]
    public float scale = 25;
    [Min(1)]
    public int octaves = 5;
    [Range(0, 1)]
    public float persistance = 0.5f;
    [Min(0)]
    public float lacunarity = 2;
    public Vector2 offset = new Vector2(0, 0);

    public bool useFallOff = true;
    private float[,] fallOffMap;

    [Min(0)]
    public float amplitude = 25;
    public AnimationCurve meshAmplitudeCurve = new AnimationCurve(new Keyframe[3]
    {
        new Keyframe(0, 0, 0, 0, 0, 0),
        new Keyframe(0.5061418f, 0.08748782f, 0.04075397f, 0.04075397f, 0.196721f, 0.3302161f),
        new Keyframe(1.1f, 1.1f, 2, 2, 0, 0),
    });

    public TerrainType[] regions = new TerrainType[8]
    {
        new TerrainType("Water Deep", 0f, new Color(0.6449218f, 0.65f, 0.3960937f, 1f)),
        new TerrainType("Water Shallow", 0.4f, new Color(0.7463942f, 0.75f, 0.4507211f, 1f)),
        new TerrainType("Sand", 0.45f, new Color(0.8117648f, 0.8156863f, 0.490196f, 1f)),
        new TerrainType("Grass", 0.55f, new Color(0.3372549f, 0.5921568f, 0.07843135f, 1f)),
        new TerrainType("Forest", 0.6f, new Color(0.2588235f, 0.4156863f, 0.0941176f, 1f)),
        new TerrainType("Mountain", 0.7f, new Color(0.3529412f, 0.2666667f, 0.2352941f, 1f)),
        new TerrainType("Mountain High", 0.9f, new Color(0.2745098f, 0.2196078f, 0.2039216f, 1f)),
        new TerrainType("Snow", 1f, new Color(1f, 1f, 1f, 1f))
    };

    private Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    private Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();
    public MapDisplay display;

    private GameObject waterDisplay;
    public Material waterMaterial;

    private void OnValidate()
    {
        DrawMap();

        if (waterDisplay == null)
        {
            waterDisplay = new GameObject("Water Layer Display");
            MeshRenderer waterMeshRenderer = waterDisplay.AddComponent<MeshRenderer>();
            MeshFilter waterMeshFilter = waterDisplay.AddComponent<MeshFilter>();
            MeshData waterMeshData = MeshGenerator.GeneratePlaneMeshData(mapChunkSize + 1, mapChunkSize + 1);

            waterMeshFilter.mesh = waterMeshData.CreateMesh();
            waterMeshRenderer.material = waterMaterial;

            waterDisplay.transform.parent = gameObject.transform;
            waterDisplay.transform.position = gameObject.transform.position + new Vector3(0, 2, 0);
        }
    }

    public void DrawMap()
    {
        MapData mapData = GenerateMapData(Vector2.zero);

        Texture2D texture = TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize);
        texture.filterMode = filterMode;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        switch (meshMode)
        {
            case MeshMode.Terrain:
                display.DrawMesh(MeshGenerator.GenerateTerrainMeshData(mapData.noiseMap, amplitude, meshAmplitudeCurve, editorLevelOfDetail), texture);
                break;

            default:
                display.DrawMesh(MeshGenerator.GeneratePlaneMeshData(mapChunkSize, mapChunkSize), texture);
                break;
        }
    }

    public void RequestMapData(Vector2 center, Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(center, callback);
        };

        new Thread(threadStart).Start();
    }

    private void MapDataThread(Vector2 center, Action<MapData> callback)
    {
        MapData mapData = GenerateMapData(center);
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    }

    public void RequestMeshData(MapData mapData, int lod, Action<MeshData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData, lod, callback);
        };

        new Thread(threadStart).Start();
    }

    private void MeshDataThread(MapData mapData, int lod, Action<MeshData> callback)
    {
        MeshData meshData = MeshGenerator.GenerateTerrainMeshData(mapData.noiseMap, amplitude, meshAmplitudeCurve, lod);
        lock (meshDataThreadInfoQueue)
        {
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callback, meshData));
        }
    }

    private void Update()
    {
        if (mapDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }

        if (meshDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < meshDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callback(threadInfo.parameter);
            }
        }
    }

    private MapData GenerateMapData(Vector2 center)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, scale, octaves, persistance, lacunarity, center + offset, normalizedMode);

        if (fallOffMap == null)
        {
            fallOffMap = FalloffGenerator.GenerateFalloffMap(mapChunkSize);
        }

        Color[] colourMap;

        switch (drawMode)
        {
            case DrawMode.Falloff:
                colourMap = TextureGenerator.ColourMapFromBlackAndWhite(fallOffMap);
                break;

            case DrawMode.NoiseMap:
                colourMap = TextureGenerator.ColourMapFromBlackAndWhite(noiseMap);
                break;

            case DrawMode.HeatMap:
                colourMap = TextureGenerator.ColourMapFromGradient(noiseMap, Noise.GetHeatMapGradient());
                break;

            case DrawMode.ColourMap:
                colourMap = new Color[mapChunkSize * mapChunkSize];
                for (int y = 0; y < mapChunkSize; y++)
                {
                    for (int x = 0; x < mapChunkSize; x++)
                    {
                        if (useFallOff)
                        {
                            noiseMap[x, y] = Mathf.Clamp(noiseMap[x, y] - fallOffMap[x, y], 0, 1);
                        }
                        float currentHeight = noiseMap[x, y];
                        for (int i = 0; i < regions.Length; i++)
                        {
                            if (currentHeight >= regions[i].height)
                            {
                                colourMap[y * mapChunkSize + x] = regions[i].colour;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                break;

            default:
            case DrawMode.White:
                colourMap = TextureGenerator.ColourMapBlankWhite(noiseMap);
                break;
        }

        return new MapData(noiseMap, colourMap);
    }

    private struct MapThreadInfo<T>
    {
        public readonly Action<T> callback;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callback, T parameter)
        {
            this.callback = callback;
            this.parameter = parameter;
        }

    }
}

public enum DrawMode
{
    White,
    Falloff,
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
    [SerializeField]
    public readonly string name;
    public float height;
    public Color colour;

    public TerrainType(string name, float height, Color colour)
    {
        this.name = name;
        this.height = height;
        this.colour = colour;
    }
}

[System.Serializable]
public struct MapData
{
    public readonly float[,] noiseMap;
    public readonly Color[] colourMap;

    public MapData(float[,] noiseMap, Color[] colourMap)
    {
        this.noiseMap = noiseMap;
        this.colourMap = colourMap;
    }
}
