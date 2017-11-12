using AntonioHR.Amusi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletMovement : MonoDancer {
    public float stepSize;
    public int lifetime;

    private Vector3 origin;
    private Vector3 direction;
    private Vector3 sourcePoint;
    private Vector3 targetPoint;
    private int step;

    protected override void Init()
    {
        origin = transform.position;
        direction = transform.forward;
        step = 0;
    }

    protected override void OnNoteEnd()
    {
        if (step == 0)
            return;

        transform.position = targetPoint;
    }

    protected override void OnNoteStart()
    {
        step++;
        if (step >= lifetime)
        {
            GameObject.Destroy(gameObject);
        }

        sourcePoint = transform.position;
        targetPoint = origin + (direction * step * stepSize);
    }

    protected override void OnNoteUpdate(float progress)
    {
        if (step == 0)
            return;

        transform.position = Vector3.Lerp(sourcePoint, targetPoint, progress);
    }

 //   void Start () {
 //       GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Impulse);
	//}
	
	//void Update () {
		
	//}
}
