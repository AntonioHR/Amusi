using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Hittable : MonoBehaviour {
    public Team team;

    public UnityEvent Hit;

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
}
