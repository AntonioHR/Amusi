using UnityEngine;
using UnityEditor;
using AntonioHR.MusicTree;
using AntonioHR.MusicTree.BeatSync;


[RequireComponent(typeof(ParticleSystem))]
public class LoopParticlesToBeat : MonoNoteEventListener
{
    private ParticleSystem particles;
    float dur;

    float lastTime;

    //Use this instead of the MonoBehaviour Start Function
    protected override void Init()
    {
        particles = GetComponent<ParticleSystem>();
        var main = particles.main;
        dur = main.duration;
        //main.simulationSpeed = 0.0f;
    }
    //This is Called whenever a note starts playing
    protected override void OnNoteStart()
    {
        particles.Play();
        //particles.time = 0;
        //particles.time = 0;
        lastTime = 0;
    }
    //Use this is called every frame while a note plays. 
    //Progress goes from 0 to 1 as the note progresses
    protected override void OnNoteUpdate(float progress)
    {
        float delta = (progress - lastTime) * dur;
        Debug.LogFormat ("{0}, {1}, {2}", progress, delta, progress - lastTime);
        particles.Simulate(delta, true, false);
        //particles.time = progress * dur;
        lastTime = progress;
    }

    protected override void OnNoteEnd()
    {
        particles.Stop();
        // TODO
    }
}