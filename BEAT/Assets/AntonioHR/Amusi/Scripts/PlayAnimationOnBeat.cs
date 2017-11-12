using UnityEngine;
using UnityEditor;
using AntonioHR.Amusi;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]
public class PlayAnimationOnBeat : MonoDancer
{
    public AnimationClip clip;

    Animator animator;

    PlayableGraph playableGraph;

    AnimationClipPlayable playableClip;
    AnimationPlayableOutput playableOutput;

    float lastProg = 0;

    //Use this instead of the MonoBehaviour Start Function
    protected override void Init()
    {
        animator = GetComponent<Animator>();

        playableGraph = PlayableGraph.Create();
        //playableGraph.SetTimeUpdateMode(DirectorUpdateMode.Manual);

        playableOutput = AnimationPlayableOutput.Create(playableGraph, "Anim", animator);

        playableClip = AnimationClipPlayable.Create(playableGraph, clip);

        playableOutput.SetSourcePlayable(playableClip);

    }
    //This is Called whenever a note starts playing
    protected override void OnNoteStart()
    {
        playableGraph.Play();
        playableClip.SetPlayState(PlayState.Paused);
        lastProg = 0;
    }
    //Use this is called every frame while a note plays. 
    //Progress goes from 0 to 1 as the note progresses
    protected override void OnNoteUpdate(float progress)
    {
        playableClip.SetTime(clip.length * progress);
    }

    protected override void OnNoteEnd()
    {
        playableClip.SetTime(clip.length);
        //playableGraph.Stop();
    }
    private void OnDestroy()
    {
        playableGraph.Destroy();
    }
}