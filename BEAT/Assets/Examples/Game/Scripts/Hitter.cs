using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hitter : MonoBehaviour {

    public Team team;

    public UnityEvent HitSomething;
    public UnityEvent Deflected;

    void OnTriggerEnter(Collider other)
    {
        var hittable = other.GetComponent<Hittable>();
        if(hittable != null && CanHit(hittable))
        {
            if (hittable.deflect)
                OnDeflect(hittable);
            else
                Hit(hittable);
        }
    }

    private void OnDeflect(Hittable hittable)
    {
        hittable.Deflect(this);
        Deflected.Invoke();
    }

    private void Hit(Hittable hittable)
    {
        hittable.OnHit(this);
        HitSomething.Invoke();
    }

    private bool CanHit(Hittable hittable)
    {
        return hittable.team != this.team;
    }
}
