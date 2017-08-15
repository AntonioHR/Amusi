using UnityEngine;
using System.Collections;
using UnityEngine.Playables;
using BeatFW.Engine;

namespace BeatFW
{
    /// <summary>
    /// Synchs an animators animation to the beat of a beatCounter
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class BeatAnimator : MonoBehaviour
    {
        private Animator animator;
        private BeatManager beatManager;

        [SerializeField]
        private bool countFromStart = false;

        private int start;
		private float startProgress;


        void Awake()
        {
            this.beatManager = FindObjectOfType<BeatManager>();
        }
        void Start()
        {
            animator = GetComponent<Animator>();
            start = beatManager.CompletedBeats;
            startProgress = beatManager.GetBeatProgress();
        }



        void Update()
        {
            //animator.SetTime(beatManager.GetBeatProgress(countFromStart?start:0) - (countFromStart?startProgress:0));
        }
    }
}
