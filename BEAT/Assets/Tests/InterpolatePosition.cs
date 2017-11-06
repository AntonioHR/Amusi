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
    public bool yoyo = true;

    private bool goingBack = false;

    public void OnNoteEnd()
    {
        Debug.Log("Note end!");
        if (!yoyo)
        {
            transform.localPosition = pos2;
        } else
        {
            transform.localPosition = goingBack? pos1: pos2;
            goingBack = !goingBack;
        }
    }

    public void OnNoteStart()
    {
        Debug.Log("Note start!");
        if (!yoyo)
        {
            transform.localPosition = pos1;
        } else
        {
            transform.localPosition = goingBack ? pos2 : pos1;
        }
    }

    public void OnNoteUpdate(float i)
    {
        Debug.Log(i);
        lerp = yoyo && goingBack?(1-i) : i;
        transform.localPosition = Vector3.Lerp(pos1, pos2, lerp);
    }

    void Start () {
        MusicTreePlayer player = FindObjectOfType<MusicTreePlayer>();
        player.AddListener(track, subtrack, this);
	}
	
}
