using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
namespace BeatFW
{
    [RequireComponent (typeof (AudioSource))]
	public class MusicController:MonoBehaviour
	{
        [SerializeField]
		private float bpm;
		[SerializeField]
		private float updateRatio = 30f;
		[SerializeField]
		private float closeToEndMargin = 5;
	
		public bool IsPlaying { get { return state == ControllerState.PLAYING || state == ControllerState.PLAYING_LAST || state == ControllerState.START; } }
        public float BPM { get { return bpm; } }
		public float BPS { get { return bpm/ 60; } }
		public AudioSource CurrentAudioSource{ get { return audioSources [audioSourceIndex]; } }
		public AudioSource NextAudioSource{ get { return audioSources [(audioSourceIndex + 1) % audioSources.Length]; } }

		public AudioClip CurrentPatch { get; private set;}
		public AudioClip NextPatch { get; private set;}

		private bool d_init = false;
		private int audioSourceIndex = 0;
		private double firstClipStartTime;
		private double currentClipEndTime;

		public enum ControllerState
		{
			IDLE, START, PLAYING, PLAYING_LAST, ENDED
		}
		public ControllerState state { get; private set; }
        public bool HasStarted
        {
            get
            {
                return state == ControllerState.PLAYING || state == ControllerState.PLAYING_LAST || state == ControllerState.ENDED;
            }
        }

        private Queue<AudioClip> patchQueue;
		private AudioSource[] audioSources;

		public event EventHandler<ClipEventArgs> OnClipChange;
		public event EventHandler<ClipEventArgs> OnClipCloseToEnd;
		public event EventHandler<ClipEventArgs> OnFirstClipStart;



        void Awake()
        {
			audioSources = GetComponents<AudioSource> ();
			Debug.Assert (audioSources.Length == 2);
            patchQueue = new Queue<AudioClip>();
			state = ControllerState.IDLE;
        }

		public double Init(AudioClip startPatch, int beatsToStart = 3)
		{
			Debug.Assert (state != ControllerState.START);

			state = ControllerState.START;

			double initTime = AudioSettings.dspTime;
			CurrentPatch = startPatch;
            CurrentAudioSource.clip = CurrentPatch;
			firstClipStartTime = initTime + beatsToStart / BPS;

			CurrentAudioSource.PlayScheduled (firstClipStartTime);

            return initTime;
		}

        public void EnqueuePatch(AudioClip patch)
		{
			patchQueue.Enqueue (patch);
			if (state == ControllerState.PLAYING_LAST) {
				if (ScheduleNextClip ())
					state = ControllerState.PLAYING;
			}
		}


		private bool ScheduleNextClip()
		{
			if (patchQueue.Count == 0)
				return false;
			//currentClipEndTime = currentClipEndTime + CurrentClip.length;
			NextPatch = patchQueue.Dequeue ();
            NextAudioSource.clip = NextPatch;
			NextAudioSource.PlayScheduled (currentClipEndTime);
			return true;
		}
        
		private void switchClips()
		{
			audioSourceIndex = (audioSourceIndex + 1) % audioSources.Length;
			CurrentPatch = NextPatch;
			NextPatch = null;
			NextAudioSource.clip = null;
		}
		public IEnumerator ClipCheck ()
		{
			while (state == ControllerState.START) {
				if (AudioSettings.dspTime > firstClipStartTime) {
					state = ControllerState.PLAYING_LAST;
                    currentClipEndTime = firstClipStartTime + CurrentPatch.length;

					if (OnFirstClipStart != null)
                        OnFirstClipStart(this, new ClipEventArgs(firstClipStartTime, CurrentPatch));
					if (state == ControllerState.PLAYING_LAST) {
						if (ScheduleNextClip ())
							state = ControllerState.PLAYING;
					}
				}
				yield return new WaitForSeconds (updateRatio / 1000f);

			}
			while (IsPlaying) {
				while ((currentClipEndTime - AudioSettings.dspTime)> closeToEndMargin * updateRatio/1000f ) {
					yield return new WaitForSeconds (updateRatio / 1000f);
				}

				if (OnClipCloseToEnd != null) {
                    OnClipCloseToEnd(this, new ClipEventArgs(AudioSettings.dspTime, CurrentPatch));
				}

				while (AudioSettings.dspTime < currentClipEndTime) {

					yield return new WaitForSeconds (updateRatio / 1000f);
				}


				if (NextAudioSource.isPlaying) {
					var clipTime = currentClipEndTime;
					currentClipEndTime = currentClipEndTime + NextPatch.length;

					switchClips ();
					state = ControllerState.PLAYING_LAST;

					if (OnClipChange != null)
                        OnClipChange(this, new ClipEventArgs(clipTime, CurrentPatch));

					if (ScheduleNextClip ())
						state = ControllerState.PLAYING;
				} else {
					state = ControllerState.ENDED;
					yield break;
				}
			}

		}
    }
}

