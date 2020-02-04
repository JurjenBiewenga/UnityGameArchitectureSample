using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

[System.Serializable]
public class FalloffCurve
{
    public AnimationCurve Curve;
    public float SoftCap;

    public float Eval(float value)
    {
        float eval = Curve.UnclampedEval(value / SoftCap);
        return eval * SoftCap;
    }
}
