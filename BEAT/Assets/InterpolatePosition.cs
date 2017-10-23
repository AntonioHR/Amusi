using AntonioHR.MusicTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using System;
using UnityEngine.Playables;

public class InterpolatePosition : MonoBehaviour, INoteEventListener {
    //public PlayableDirector dir;

    public Vector3 pos1, pos2;
    //double timeFactor;
    public string track;
    public int subtrack;
    public float lerp;

    public void OnNoteEnd()
    {
        //dir.Stop();
        transform.position = pos2;
    }

    public void OnNoteStart()
    {
        transform.position = pos1;
        //dir.Play();
    }

    public void OnNoteUpdate(float i)
    {
        lerp = i;
        transform.position = Vector3.Lerp(pos1, pos2, i);
        //dir.time = i * timeFactor;
    }

    void Start () {
        MusicTreePlayer player = FindObjectOfType<MusicTreePlayer>();
        player.AddListener(track, subtrack, this);
        //timeFactor = 1/dir.duration;
	}
	
}
