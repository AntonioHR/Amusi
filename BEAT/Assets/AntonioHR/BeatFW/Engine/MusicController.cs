using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
namespace AntonioHR.BeatFW.Engine
{
    [System.Serializable]
	public class MusicController
	{
        [Serializable]
        public class Settings
        {
            public float bpm;
            public float updateRatio = 30f;
            public float closeToEndMargin = 5;
        }
        Settings settings;
	
		public bool IsPlaying { get { return state == ControllerState.PLAYING || state == ControllerState.PLAYING_LAST || state == ControllerState.START; } }
        public float BPM { get { return settings.bpm; } }
		public float BPS { get { return settings.bpm/ 60; } }
		public AudioSource CurrentAudioSource{ get { return audioSources [audioSourceIndex]; } }
		public AudioSource NextAudioSource{ get { return audioSources [(audioSourceIndex + 1) % audioSources.Length]; } }

		public AudioClip CurrentPatch { get; private set;}
		public AudioClip NextPatch { get; private set;}

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

		public event Action OnClipChange;
        public event Action OnClipCloseToEnd;
        public event Action OnFirstClipStart;



        public MusicController(AudioSource[] audioSources, Settings settings)
        {
            this.audioSources = audioSources;
            this.settings = settings;
            Debug.Assert(audioSources.Length == 2);
            patchQueue = new Queue<AudioClip>();
            state = ControllerState.IDLE;
        }



		public double Init(AudioClip startPatch, int beatsToStart = 4)
		{
			Debug.Assert (state != ControllerState.START);

			state = ControllerState.START;

			double initTime = AudioSettings.dspTime;
			CurrentPatch = startPatch;
            CurrentAudioSource.clip = CurrentPatch;
			firstClipStartTime = initTime + beatsToStart / BPS;

			CurrentAudioSource.PlayScheduled (firstClipStartTime);
            return firstClipStartTime;
            //return initTime;
		}

        public void EnqueuePatch(AudioClip patch)
		{
            Debug.LogFormat("Enqueueing {0}", patch);
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
                        OnFirstClipStart();
					if (state == ControllerState.PLAYING_LAST) {
						if (ScheduleNextClip ())
							state = ControllerState.PLAYING;
					}
				}
				yield return new WaitForSeconds (settings.updateRatio / 1000f);

			}
			while (IsPlaying) {
                while ((currentClipEndTime - AudioSettings.dspTime) > settings.closeToEndMargin * settings.updateRatio / 1000f)
                {
                    yield return new WaitForSeconds(settings.updateRatio / 1000f);
				}

				if (OnClipCloseToEnd != null) {
                    OnClipCloseToEnd();
				}

				while (AudioSettings.dspTime < currentClipEndTime) {

                    yield return new WaitForSeconds(settings.updateRatio / 1000f);
				}


				if (NextAudioSource.isPlaying) {
					currentClipEndTime = currentClipEndTime + NextPatch.length;

					switchClips ();
					state = ControllerState.PLAYING_LAST;

					if (OnClipChange != null)
                        OnClipChange();

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

