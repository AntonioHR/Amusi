using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntonioHR.Amusi;
using System;

public class Spawner : MonoDancer{

    public GameObject bulletPrefab;
    public float spawnOffset = 1f;

    public Transform spawnPoint;

    protected override void Init()
    {
        
    }

    protected override void OnNoteEnd()
    {
        var spawnPosition = spawnPoint.position + transform.forward * spawnOffset;
        var bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.LookRotation(transform.forward));
    }

    protected override void OnNoteStart()
    {
        
    }

    protected override void OnNoteUpdate(float progress)
    {
        
    }
}
