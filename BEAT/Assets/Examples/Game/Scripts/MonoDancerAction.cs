using UnityEngine;
using UnityEditor;
using AntonioHR.Amusi;
using UnityEngine.Events;

public class MonoDancerAction : MonoDancer
{
    public UnityEvent Triggered;
    [Range(0, 1)]
    public float triggerPoint;

    bool hasTriggered;

    protected override void Init()
    {
        // TODO
    }
    //This is Called whenever a note starts playing
    protected override void OnNoteStart()
    {
        hasTriggered = false;
    }
    //Use this is called every frame while a note plays. 
    //Progress goes from 0 to 1 as the note progresses
    protected override void OnNoteUpdate(float progress)
    {
        if (progress > triggerPoint)
            Triggered.Invoke();
    }

    protected override void OnNoteEnd()
    {
        // TODO
    }
}