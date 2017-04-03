using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[System.Serializable]
public class ScaledCurve
{
    public float scale;
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

    public void Test(float f)
    {
        curve.Evaluate(f);
    }
}
