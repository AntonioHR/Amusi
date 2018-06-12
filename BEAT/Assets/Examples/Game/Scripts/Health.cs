using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent Died;
    public UnityEvent TookDamage;

    public int Max = 3;
    public bool invulnerable = false;

    int Current;
    bool dead = false;

    public float HealthPercent
    {
        get
        {
            return (float)Current / (float)Max;
        }
    }

    private void Start()
    {
        Current = Max;
    }

    public void Damage(int damage)
    {
        if (dead || invulnerable)
            return;

        Current -= damage;

        TookDamage.Invoke();

        if (Current <= 0)
            Die();
    }

    private void Die()
    {
        dead = true;
        Died.Invoke();
    }
}