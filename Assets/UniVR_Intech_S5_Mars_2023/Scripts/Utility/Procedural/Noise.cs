using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode)
	{
		float[,] noiseMap = new float[mapWidth, mapHeight];

		System.Random prng = new System.Random(seed);
		Vector2[] octaveOffsets = new Vector2[octaves];

		float maxPossibleHeight = 0;
		float amplitude = 1;
		float frequency = 1;

		for (int i = 0; i < octaves; i++)
		{
			float offsetX = prng.Next(-100000, 100000) + offset.x;
			float offsetY = prng.Next(-100000, 100000) - offset.y;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);

			maxPossibleHeight += amplitude;
			amplitude *= persistance;
		}

		if (scale <= 0)
		{
			scale = 0.0001f;
		}

		float maxLocalNoiseHeight = float.MinValue;
		float minLocalNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;


		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++)
				{
					float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency ;
					float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;
				}

				if (noiseHeight > maxLocalNoiseHeight)
				{
					maxLocalNoiseHeight = noiseHeight;
				}
				else if (noiseHeight < minLocalNoiseHeight)
				{
					minLocalNoiseHeight = noiseHeight;
				}
				noiseMap[x, y] = noiseHeight;
			}
		}

		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				switch (normalizeMode)
                {
					case NormalizeMode.Local:
						noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
						break;

					default:
					case NormalizeMode.Global:
						float normalizedHeight = (noiseMap[x, y] + 1) / (2f * maxPossibleHeight) / 1.75f;
						noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
						break;
                }
				
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

public enum NormalizeMode
{
	Local,
	Global
}
