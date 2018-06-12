using AntonioHR.Amusi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
using System;

public class SelectAndPlayAnimationsOnBeat : MonoBehaviour {

    [Serializable]
    public class ClipBinding
    {
        public AnimationClip clip;
        public NoteEventBinding binding;
    }

    [SerializeField]
    ClipBinding[] clipBindings;

    Animator animator;

    PlayableGraph playableGraph;

    AnimationMixerPlayable mixer;

    AnimationClipPlayable[] playableClips;
    public bool stayOnAnimation = false;

    float lastProg = 0;


    // Use this for initialization
    void Start () {
        for (int i = 0; i < clipBindings.Length; i++)
        {
            int indx = i;
            clipBindings[indx].binding.Init();
            clipBindings[indx].binding.Bind(() => StartClip(indx), (f) => UpdateClip(indx, f), () => EndClip(indx));
        }


        animator = GetComponent<Animator>();

        playableGraph = PlayableGraph.Create();
        

        var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Anim", animator);

        mixer = AnimationMixerPlayable.Create(playableGraph, clipBindings.Length);

        playableOutput.SetSourcePlayable(mixer);


        playableClips = new AnimationClipPlayable[clipBindings.Length];

        for (int i = 0; i < playableClips.Length; i++)
        {
            playableClips[i] = AnimationClipPlayable.Create(playableGraph, clipBindings[i].clip);

            playableGraph.Connect(playableClips[i], 0, mixer, i);
        }
    }

    private void StartClip(int i)
    {
        playableGraph.Play();
        playableClips[i].SetPlayState(PlayState.Paused);
        playableClips[i].SetTime(0);

        for (int j = 0; j < clipBindings.Length; j++)
        {
            mixer.SetInputWeight(j, j == i? 1:0);
        }

        lastProg = 0;
    }

    private void UpdateClip(int i, float f)
    {
        
        playableClips[i].SetTime(clipBindings[i].clip.length * f);
    }

    private void EndClip(int i)
    {
        playableClips[i].SetTime(clipBindings[i].clip.length);
        if (!stayOnAnimation)
            mixer.SetInputWeight(i, 0);
    }
    
}
