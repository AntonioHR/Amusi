using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitter : MonoBehaviour {

    public Team team;

    void OnTriggerEnter(Collider other)
    {
        var hittable = other.GetComponent<Hittable>();
        if(hittable != null && CanHit(hittable))
        {
            Hit(hittable);
        }
    }

    private void Hit(Hittable hittable)
    {
        hittable.OnHit(this);
    }

    private bool CanHit(Hittable hittable)
    {
        return hittable.team != this.team;
    }
}
