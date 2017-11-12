using AntonioHR.Amusi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RamielLaserAim : MonoBehaviour {
    public Transform beam;

    PlayerController player;
    public LineRenderer rend;

    public NoteEventBinding binding;

    public Color c1;
    public Color c2;

    public AnimationCurve colorCurve;

    // Use this for initialization
    void Start () {
        player = FindObjectOfType<PlayerController>();

        rend.enabled = false;
        rend.SetPosition(0, transform.position);
        binding.Init();
        binding.Bind(OnNoteStart, OnNoteUpdate, OnNoteEnd);
	}

    // Update is called once per frame
    void Update () {
	}




    private void OnNoteStart()
    {
        rend.enabled = true;
        rend.SetPosition(1, player.transform.position);

        var pos = player.transform.position;
        pos.y = beam.transform.position.y;
        beam.transform.LookAt(pos);

        rend.material.SetColor("_TintColor", Color.Lerp(c1, c2, colorCurve.Evaluate(0)));
    }

    private void OnNoteUpdate(float i)
    {
        rend.material.SetColor("_TintColor", Color.Lerp(c1, c2, colorCurve.Evaluate(i)));
    }

    private void OnNoteEnd()
    {
        rend.enabled = false;
    }
    private void OnDestroy()
    {
        binding.Cleanup();
    }
}

