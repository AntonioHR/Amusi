using UnityEngine;
using UnityEditor;
using AntonioHR.Amusi;
using AntonioHR.Amusi.BeatSynchronization;

[AddComponentMenu("Amusic/Musical Position Lerp")]
public class MusicalPositionLerp : MonoNoteEventListener
{
    public Vector3 pos1, pos2;
    public bool yoyo = true;

    private float lerp;
    private bool goingBack = false;


    //Use this instead of the MonoBehaviour Start Function
    protected override void Init()
    {
        // TODO
    }
    //This is Called whenever a note starts playing
    protected override void OnNoteStart()
    {
        if (!yoyo)
        {
            transform.localPosition = pos1;
        }
        else
        {
            transform.localPosition = goingBack ? pos2 : pos1;
        }
    }
    //Use this is called every frame while a note plays. 
    //Progress goes from 0 to 1 as the note progresses
    protected override void OnNoteUpdate(float i)
    {
        lerp = yoyo && goingBack ? (1 - i) : i;
        transform.localPosition = Vector3.Lerp(pos1, pos2, lerp);
    }

    protected override void OnNoteEnd()
    {
        if (!yoyo)
        {
            transform.localPosition = pos2;
        }
        else
        {
            transform.localPosition = goingBack ? pos1 : pos2;
            goingBack = !goingBack;
        }
    }
}