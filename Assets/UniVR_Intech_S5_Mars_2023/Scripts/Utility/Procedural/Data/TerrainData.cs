using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainData : UpdatableData
{
    public float uniformScale = 1f;

    public bool useFlatShading = false;
    public bool useFallOff = false;
    
    [Min(0)]
    public float amplitude = 25;
    public AnimationCurve meshAmplitudeCurve = new AnimationCurve(new Keyframe[3]
    {
        new Keyframe(0, 0, 0, 0, 0, 0),
        new Keyframe(0.5061418f, 0.08748782f, 0.04075397f, 0.04075397f, 0.196721f, 0.3302161f),
        new Keyframe(1.1f, 1.1f, 2, 2, 0, 0),
    });
}
