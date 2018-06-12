using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoreColor : MonoBehaviour {
    public Color c1;
    public Color c2;
    public Color invulnerableColor;

    public MeshRenderer core;
    public Health playerHealth;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        core.material.SetColor("_EmissionColor", CoreColor());
    }

    public Color CoreColor()
    {
        if (playerHealth.invulnerable)
            return invulnerableColor;
        return Color.Lerp(c1, c2, 1 - playerHealth.HealthPercent);
    }
}
