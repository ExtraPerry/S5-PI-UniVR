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

	public static float[,] GenerateNoiseMap(int widthSize, int heightSize, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
	{
		float[,] noiseMap = new float[widthSize, heightSize];

		System.Random prng = new System.Random(seed);
		Vector2[] octaveOffsets = new Vector2[octaves];
		for (int i = 0; i < octaves; i++)
		{
			float offsetX = prng.Next(-100000, 100000) + offset.x;
			float offsetY = prng.Next(-100000, 100000) + offset.y;
			octaveOffsets[i] = new Vector2(offsetX, offsetY);
		}

		if (scale <= 0)
		{
			scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = widthSize / 2f;
		float halfHeight = heightSize / 2f;


		for (int y = 0; y < heightSize; y++)
		{
			for (int x = 0; x < widthSize; x++)
			{

				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int i = 0; i < octaves; i++)
				{
					float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
					float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

					float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;
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

		for (int y = 0; y < heightSize; y++)
		{
			for (int x = 0; x < widthSize; x++)
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
