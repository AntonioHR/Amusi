﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour {
    
    public void Kill()
    {
        GameObject.Destroy(gameObject);
    }
}
