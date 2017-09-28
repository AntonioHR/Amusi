using AntonioHR.BeatFW;
using AntonioHR.MusicTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class BeatPatternListener : MonoBehaviour
{
    public UnityEvent OnNoteEvent;

    public BeatPatternAsset pattern;

    IBeatManager beatManager;

    float lastVal;


    public void Awake()
    {
        beatManager = FindObjectOfType<MusicTreePlayer>();
    }

    public void Update()
    {
        float curr = beatManager.BeatProgressFull;
        if (curr < 0)
            return;
        var diff = pattern.NotesBetween(lastVal, curr);
        for (int i = 0; i < diff; i++)
        {
            OnNote();
        }

        lastVal = curr;
    }

    public virtual void OnNote()
    {
        OnNoteEvent.Invoke();
    }
}
