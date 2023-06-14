using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NoiseMapParams
{
	public int mapWidth;
	public int mapHeight;
	public int seed;
	[Min(1)]
	public float scale;
	[Min(0)]
	public int octaves;
	[Range(0, 1)]
	public float persistance;
	[Min(1)]
	public float lacunarity;
	public Vector2 offset;

	public NoiseMapParams(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
		this.mapWidth = mapWidth;
		this.mapHeight = mapHeight;
		this.seed = seed;
		this.scale = scale;
		this.octaves = octaves;
		this.persistance = persistance;
		this.lacunarity = lacunarity;
		this.offset = offset;
	}
}

public static class Noise
{
	public static Gradient GetHeatMapGradient()
    {
		Gradient heatMapGradient = new Gradient();
		heatMapGradient.SetKeys(
			new GradientColorKey[5]
			{
				new GradientColorKey(new Color(0f, 0f, 1f), 0f),
				new GradientColorKey(new Color(0f, 1f, 0f), 0.25f),
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
		return heatMapGradient;
	}

	public static float[,] GenerateNoiseMap(NoiseMapParams parameters)
	{
		float[,] noiseMap = new float[parameters.mapWidth, parameters.mapHeight];

		System.Random prng = new System.Random(parameters.seed);
		Vector2[] octaveOffsets = new Vector2[parameters.octaves];
		for (int i = 0; i < parameters.octaves; i++)
		{
			float offsetX = prng.Next(-100000, 100000) + parameters.offset.x;
			float offsetY = prng.Next(-100000, 100000) + parameters.offset.y;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);
		}

		if (parameters.scale <= 0)
		{
			parameters.scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = parameters.mapWidth / 2f;
		float halfHeight = parameters.mapHeight / 2f;


		for (int y = 0; y < parameters.mapHeight; y++)
		{
			for (int x = 0; x < parameters.mapWidth; x++)
			{

				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < parameters.octaves; i++)
				{
					float sampleX = (x - halfWidth) / parameters.scale * frequency + octaveOffsets[i].x;
					float sampleY = (y - halfHeight) / parameters.scale * frequency + octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= parameters.persistance;
					frequency *= parameters.lacunarity;
				}

				if (noiseHeight > maxNoiseHeight)
				{
					maxNoiseHeight = noiseHeight;
				}
				else if (noiseHeight < minNoiseHeight)
				{
					minNoiseHeight = noiseHeight;
				}
				noiseMap[x, y] = noiseHeight;
			}
		}

		for (int y = 0; y < parameters.mapHeight; y++)
		{
			for (int x = 0; x < parameters.mapWidth; x++)
			{
				noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
			}
		}

		return noiseMap;
	}

	public static float GenerateNoise(int x, int y, float scale)
    {
        float sampleX = x / scale;
        float sampleY = y / scale;

        return Mathf.PerlinNoise(sampleX, sampleY);


    }
}
