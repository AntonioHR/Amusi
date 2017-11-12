﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Serializable]
    public class Settings
    {
        public float speed;
    }
    [SerializeField]
    Settings settings;
    [SerializeField]
    Rigidbody body;

    public float HorizontalInput { get { return Input.GetAxis("Horizontal"); } }
    public float VerticalInput { get { return Input.GetAxis("Vertical"); } }
    
    void Start () {
		
	}
	
	void Update () {
        var move = new Vector3(HorizontalInput, 0, VerticalInput);
        body.velocity = move * settings.speed; ;
        
	}
}
