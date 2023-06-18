using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class NoiseData : UpdatableData
{
    public NormalizeMode normalizedMode = NormalizeMode.Global;

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

    protected override void OnValidate()
    {
        if (lacunarity < 1) lacunarity = 1;
        if (octaves < 0) octaves = 0;
        base.OnValidate();
    }
}
