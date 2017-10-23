using AntonioHR.MusicTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using System;
using UnityEngine.Playables;
using AntonioHR.MusicTree.BeatSync;

public class InterpolatePosition : MonoBehaviour, INoteEventListener {

    public Vector3 pos1, pos2;
    public string track;
    public int subtrack;
    public float lerp;

    public void OnNoteEnd()
    {
        transform.position = pos2;
    }

    public void OnNoteStart()
    {
        transform.position = pos1;
    }

    public void OnNoteUpdate(float i)
    {
        lerp = i;
        transform.position = Vector3.Lerp(pos1, pos2, i);
    }

    void Start () {
        MusicTreePlayer player = FindObjectOfType<MusicTreePlayer>();
        player.AddListener(track, subtrack, this);
	}
	
}
