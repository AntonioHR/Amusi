using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizePulser : MonoBehaviour {
    public float boost = .25f;
    public float duration = .25f;
    public AnimationCurve curve;
    
    Vector3 baseScale;
    private void Start()
    {
        baseScale = transform.localScale;
    }
    public void DoPulse()
    {
        StartCoroutine(PulseCoroutine());
    }

    private IEnumerator PulseCoroutine()
    {
        float startTime = Time.time;
        float endTime = startTime + duration;

        while(Time.time < startTime + duration)
        {
            float lerp = (Time.time - startTime) / duration;

            transform.localScale = baseScale * (1 + (curve.Evaluate(lerp) * boost));
            yield return null;
        }
    }
}
