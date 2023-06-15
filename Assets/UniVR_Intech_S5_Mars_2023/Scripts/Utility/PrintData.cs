using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrintData
{
    public static void printAnimationCurveKeys(AnimationCurve animCurve)
    {
        int i = 0;
        foreach (Keyframe key in animCurve.keys)
        {
            Debug.Log(
                "KeyNumber : " + i + "\n" +
                "Time : " + key.time.ToString() + "\n" +
                "Value : " + key.value.ToString() + "\n" +
                "inTangent : " + key.inTangent.ToString() + "\n" +
                "outTangent : " + key.outTangent.ToString() + "\n" +
                "inWeight : " + key.inWeight.ToString() + "\n" +
                "outWeight : " + key.outWeight.ToString() + "\n" +
                "."
            );
            i++;
        }
    }
}
