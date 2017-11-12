using UnityEngine;
using UnityEditor;
using AntonioHR.Amusi;

public class PlayerLaser : MonoDancer
{

    public MeshRenderer renderer;
    public Collider col;


    //Use this instead of the MonoBehaviour Start Function
    protected override void Init()
    {
        // TODO
    }
    //This is Called whenever a note starts playing
    protected override void OnNoteStart()
    {
        renderer.enabled = true;
        col.enabled = true;
    }
    //Use this is called every frame while a note plays. 
    //Progress goes from 0 to 1 as the note progresses
    protected override void OnNoteUpdate(float progress)
    {
        // TODO
    }

    protected override void OnNoteEnd()
    {
        renderer.enabled = false;
        col.enabled = false;
    }
}