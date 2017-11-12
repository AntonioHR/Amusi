using AntonioHR.Amusi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpinningEnemy : MonoDancer {

    public float angleStep = 90;

    private Quaternion startAngle;
    private Quaternion endAngle;

    protected override void Init()
    {
        
    }

    protected override void OnNoteEnd()
    {
        transform.rotation = endAngle;
    }

    protected override void OnNoteStart()
    {
        startAngle = transform.rotation;
        endAngle = Quaternion.AngleAxis(transform.eulerAngles.y + angleStep, Vector3.up);
    }

    protected override void OnNoteUpdate(float progress)
    {
        transform.rotation = Quaternion.Lerp(startAngle, endAngle, progress);
    }
}
