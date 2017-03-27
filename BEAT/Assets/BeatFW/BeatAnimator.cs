using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;
using Zenject;
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
        private BeatCounter beatCounter;

        [SerializeField]
        private bool countFromStart = false;

        private int start;
		private float startProgress;



        [Inject]
        void Construct(BeatCounter beatCounter)
        {
            this.beatCounter = beatCounter;
            start = beatCounter.CompletedBeats;
            startProgress = beatCounter.GetBeatProgress();
        }

        void Start()
        {
            animator = GetComponent<Animator>();
        }



        void Update()
        {

			animator.SetTime(beatCounter.GetBeatProgress(countFromStart?start:0) - (countFromStart?startProgress:0));


        }
    }
}
