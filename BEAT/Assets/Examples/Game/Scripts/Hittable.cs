using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hittable : MonoBehaviour {
    public Team team;

    public UnityEvent Hit;
    public UnityEvent Deflected;

    [SerializeField]
    private bool deflect;

    public bool Deflect
    {
        get
        {
            return deflect;
        }

        set
        {
            deflect = value;
        }
    }

    internal void OnHit(Hitter hitter)
    {
        Hit.Invoke();
        var dmg = hitter.GetComponent<Damage>();
        var health = GetComponent<Health>();
        if (dmg != null && health != null)
        {
            health.Damage(dmg.damage);       
        }
    }

    internal void DoDeflect(Hitter hitter)
    {
        Deflected.Invoke();
    }
}
