using AntonioHR.Amusi.BeatSynchronization;
using UnityEngine;


[AddComponentMenu("Amusic/Musical Particle System")]
[RequireComponent(typeof(ParticleSystem))]
public class MusicalParticleSystem : MonoNoteEventListener
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
    }
    //This is Called whenever a note starts playing
    protected override void OnNoteStart()
    {
        particles.Play();
        lastTime = 0;
    }
    //Use this is called every frame while a note plays. 
    //Progress goes from 0 to 1 as the note progresses
    protected override void OnNoteUpdate(float progress)
    {
        float delta = (progress - lastTime) * dur;
        //Debug.LogFormat ("{0}, {1}, {2}", progress, delta, progress - lastTime);
        particles.Simulate(delta, true, false);
        lastTime = progress;
    }

    protected override void OnNoteEnd()
    {
        particles.Stop();
    }
}