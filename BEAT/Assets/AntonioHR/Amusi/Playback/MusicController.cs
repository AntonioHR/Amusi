using System;
using System.Collections.Generic;
using UnityEngine;

namespace AntonioHR.Amusi.Playback
{
    [System.Serializable]
	public class MusicController
	{
		public enum ControllerState
		{
			IDLE, START, PLAYING,
			ENDED
		}

		#region public Properties
		public bool IsPlaying { get { return State == ControllerState.PLAYING || State == ControllerState.START; } }
		public int Frequency { get { return CurrentClip.frequency; } }
		public AudioSource CurrentAudioSource{ get { return audioSources [audioSourceIndex]; } }
		public AudioSource NextAudioSource{ get { return audioSources [(audioSourceIndex + 1) % audioSources.Length]; } }

		public AudioClip CurrentClip { get; private set;}
		public AudioClip NextPatch { get; private set; }
		public ControllerState State { get; private set; }

		public double CurrentClipStartDSPTme { get; private set; }
		
		public bool HasStarted
		{
			get
			{
				return State == ControllerState.PLAYING || State == ControllerState.ENDED;
			}
		}

		#endregion
		
		public event Action OnNewClipStart;
		public event Action OnClipCloseToEnd;
		public event Action OnFirstClipStart;

		private float closeToEndMargin = 0.2f;


		private int audioSourceIndex = 0;
		private double firstClipStartTime;
		private double currentClipEndTime;
		private bool triggeredCloseToEndMargin = false;
		private bool hasNextScheduled = false;

		private Queue<AudioClip> patchQueue;
		private AudioSource[] audioSources;

		public MusicController(AudioSource[] audioSources)
		{
			this.audioSources = audioSources;
			Debug.Assert(audioSources.Length == 2);
			patchQueue = new Queue<AudioClip>();
			State = ControllerState.IDLE;
		}
		
		public double Init(AudioClip startPatch, float timeToStart = 3)
		{
			Debug.Assert (State != ControllerState.START);

			State = ControllerState.START;

			double initTime = AudioSettings.dspTime;
			CurrentClip = startPatch;
			CurrentAudioSource.clip = CurrentClip;
			firstClipStartTime = initTime + timeToStart;

			CurrentAudioSource.PlayScheduled (firstClipStartTime);
			return firstClipStartTime;
		}

		public void EnqueueClip(AudioClip clip)
		{
			Debug.LogFormat("Enqueueing {0}", clip);
			patchQueue.Enqueue (clip);
			if(!hasNextScheduled)
			{
				hasNextScheduled = ScheduleNextAudioClip();
			}
		}
		private bool ScheduleNextAudioClip()
		{
			if (patchQueue.Count == 0)
				return false;
			NextPatch = patchQueue.Dequeue ();
			NextAudioSource.clip = NextPatch;
			NextAudioSource.PlayScheduled (currentClipEndTime);
			return true;
		}
		
		public void Step()
		{
			if(State == ControllerState.START)
			{
				PreStartStep();
			} else if(IsPlaying)
			{
				PlayingStep();
			}
		}

		private void PreStartStep()
		{
			if (AudioSettings.dspTime > firstClipStartTime)
			{
				State = ControllerState.PLAYING;
				currentClipEndTime = firstClipStartTime + CurrentClip.length;
				CurrentClipStartDSPTme = firstClipStartTime;

				if (OnFirstClipStart != null)
					OnFirstClipStart();
				if(OnNewClipStart != null)
					OnNewClipStart();
				hasNextScheduled = ScheduleNextAudioClip();
			}
		}

		private void PlayingStep()
		{
			if (!triggeredCloseToEndMargin && IsOnCloseToEndMargin())
			{
				triggeredCloseToEndMargin = true;
				if (OnClipCloseToEnd != null)
					OnClipCloseToEnd();
			}
			if (IsCurrentClipOver())
			{
				if (hasNextScheduled)
				{
					triggeredCloseToEndMargin = false;
					PerformSwitch();
				} else
				{
					State = ControllerState.ENDED;
				}
			}
		}
		private void PerformSwitch()
		{
			if (NextAudioSource.isPlaying)
			{
				CurrentClipStartDSPTme = currentClipEndTime;
				currentClipEndTime = currentClipEndTime + NextPatch.length;

				SwitchAudioClips();

				if (OnNewClipStart != null)
					OnNewClipStart();

				hasNextScheduled = ScheduleNextAudioClip();
			}
		}
		private void SwitchAudioClips()
		{
			audioSourceIndex = (audioSourceIndex + 1) % audioSources.Length;
			CurrentClip = NextPatch;
			NextPatch = null;
			NextAudioSource.clip = null;
		}

		private bool IsCurrentClipOver()
		{
			return AudioSettings.dspTime > currentClipEndTime;
		}
		private bool IsOnCloseToEndMargin()
		{
			return (currentClipEndTime - AudioSettings.dspTime) < closeToEndMargin/ 1000f;
		}

		
	}
}

