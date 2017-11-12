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

    //public Color c1;
    //public Color c2;

    //public AnimationCurve colorCurve;

    Vector3 storedPlayerPos;

    

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

        storedPlayerPos = player.transform.position;
        storedPlayerPos.y = 0;

        var targetPos = player.transform.position;
        targetPos.y = beam.transform.position.y;
        beam.transform.LookAt(targetPos);
        
        

        //rend.material.SetColor("_TintColor", Color.Lerp(c1, c2, colorCurve.Evaluate(0)));
    }

    private void OnNoteUpdate(float i)
    {
        //rend.material.SetColor("_TintColor", Color.Lerp(c1, c2, colorCurve.Evaluate(i)));

        Vector3 start = transform.position;
        start.y = 0;

        Vector3 end = player.transform.position;
        rend.SetPosition(1, Vector3.Lerp(start, storedPlayerPos, i));

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

