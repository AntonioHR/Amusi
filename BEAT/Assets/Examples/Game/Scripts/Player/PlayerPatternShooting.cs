using AntonioHR.Amusi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerPatternShooting : MonoBehaviour {
    public NoteEventBinding[] bindings;
    public Vector3[] directions;

    public GameObject bulletPrefab;
	
	void Start () {
        for (int i = 0; i < bindings.Length; i++)
        {
            int indx = i;
            bindings[i].Init();
            bindings[i].Bind(()=>Shoot(indx), (f)=> { }, ()=> { });
        }
	}

    private void Shoot(int i)
    {
        var bullet = Instantiate(bulletPrefab).GetComponent<PlayerBullet>();
        bullet.transform.position = transform.position;
        //bullet.Binding.subtrack = 0;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, directions[i]);
        bullet.Direction = directions[i];
    }

    void Update () {
		
	}
}
