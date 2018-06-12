using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntonioHR.Amusi;
using System;

public class PlayerCrossXShooter :  MultiMonoDancer
{
    public PlayerCoreColor Core;

    [Serializable]
    public class Pattern
    {
        public Vector3[] directions;
    }
    public GameObject bulletPrefab;


    int index;


    public Pattern[] patterns;

    protected override void Init()
    {
    }

    protected override void OnNoteEnd(int i)
    {
    }

    protected override void OnNoteStart(int i)
    {
        foreach (var dir in patterns[index].directions)
        {
            var shootDir = dir.normalized;
            var bullet = Instantiate(bulletPrefab).GetComponent<PlayerBullet>();
            bullet.SetColor(Core.CoreColor());
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, shootDir);
            bullet.Direction = shootDir;
        }

        index++;
        index = index % patterns.Length;
    }

    protected override void OnNoteUpdate(int i, float progress)
    {
    }

}
