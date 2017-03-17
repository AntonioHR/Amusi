using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
namespace BeatFW
{
    [RequireComponent (typeof (AudioSource))]
    [RequireComponent(typeof(BeatCounter))]
	public class BeatMusicController:MonoBehaviour
	{
        [SerializeField]
		private float bpm;
		[SerializeField]
		private float updateRatio = 30f;
		[SerializeField]
		private float closeToEndMargin = 5;
		[SerializeField]
		private BeatPatchData beatClipDataObject;
	
		public bool IsPlaying { get { return state == ControllerState.PLAYING || state == ControllerState.PLAYING_LAST || state == ControllerState.START; } }
        public float BPM { get { return bpm; } }
		public float BPS { get { return bpm/ 60; } }
		public AudioSource CurrentAudioSource{ get { return audioSources [audioSourceIndex]; } }
		public AudioSource NextAudioSource{ get { return audioSources [(audioSourceIndex + 1) % audioSources.Length]; } }

		public AudioClip CurrentClip { get{ return CurrentAudioSource.clip; }}
		public string CurrentPatch { get; private set;}
		public AudioClip NextClip { get{ return NextAudioSource.clip; } }
		public string NextPatch { get; private set;}

		private bool d_init = false;
		private int audioSourceIndex = 0;
		private double firstClipStartTime;
		private double currentClipEndTime;
		private Dictionary<string, AudioClip> beatPatches;

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

		private Queue<string> patchQueue;
		private AudioSource[] audioSources;

		public event EventHandler<ClipEventArgs> OnClipChange;
		public event EventHandler<ClipEventArgs> OnClipCloseToEnd;
		public event EventHandler<ClipEventArgs> OnFirstClipStart;



        void Awake()
        {
			audioSources = GetComponents<AudioSource> ();
			Debug.Assert (audioSources.Length == 2);
			patchQueue = new Queue<string> ();
			beatPatches = beatClipDataObject.CreateDictionary ();
			state = ControllerState.IDLE;
        }

		public void Init(string startPatch, int beatsToStart = 3)
		{
			Debug.Assert (state != ControllerState.START);

			state = ControllerState.START;

			double initTime = AudioSettings.dspTime;
			CurrentPatch = startPatch;
			CurrentAudioSource.clip = beatPatches[startPatch];
			firstClipStartTime = initTime + beatsToStart / BPS;

			CurrentAudioSource.PlayScheduled (firstClipStartTime);

			StartCoroutine (ClipCheck ());

			var beatCounter = GetComponent<BeatCounter>();
			beatCounter.Init(this);
			beatCounter.StartBeatUpdate(firstClipStartTime);
		}

		public void QueueUpPatch(string patch)
		{
			Debug.Assert (beatPatches.ContainsKey (patch), "does not contain key "+patch);
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
			NextAudioSource.clip = beatPatches[NextPatch];
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
		private IEnumerator ClipCheck ()
		{
			while (state == ControllerState.START) {
				if (AudioSettings.dspTime > firstClipStartTime) {
					state = ControllerState.PLAYING_LAST;
					currentClipEndTime = firstClipStartTime + CurrentClip.length;

					if (OnFirstClipStart != null)
						OnFirstClipStart (this, new ClipEventArgs (firstClipStartTime, CurrentClip, CurrentPatch));
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
					OnClipCloseToEnd (this, new ClipEventArgs (AudioSettings.dspTime, CurrentClip,CurrentPatch));
				}

				while (AudioSettings.dspTime < currentClipEndTime) {

					yield return new WaitForSeconds (updateRatio / 1000f);
				}


				if (NextAudioSource.isPlaying) {
					var clipTime = currentClipEndTime;
					currentClipEndTime = currentClipEndTime + NextClip.length;

					switchClips ();
					state = ControllerState.PLAYING_LAST;

					if (OnClipChange != null)
						OnClipChange (this, new ClipEventArgs (clipTime, CurrentClip, CurrentPatch));

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

