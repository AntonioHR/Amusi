using UnityEngine;
using System.Collections;
using System;

namespace AntonioHR.BeatFW.Internal
{
	public class BeatCounter
    {
        [Serializable]
        public class Settings
        {
            /// <summary>
            /// The Counter update ratio in milisseconds
            /// </summary>
            public float updateRatio = 30f;
            public MeasureSignature measureSignature;
        }

        Settings settings;


        #region Events
		public event Action<BeatCounter> OnBeat;
        public event Action<BeatCounter> OnHalfBeat;
        public event Action<BeatCounter> OnEigthBeat;
        public event Action<BeatCounter> OnMeasure;
        #endregion
        
        
        #region Public properties
            public int CompletedMeasures
            {
                get { return (int)(currentBeat / settings.measureSignature.MeasureSize()); }
            }
            public int CompletedBeatsInMeasure
            {
                get { return (int)currentBeat % settings.measureSignature.MeasureSize(); }
            }
            public int CompletedBeats
            {
                get { return (int)currentBeat; }
            }
            public float Progress
            {
                get
                {
                    return currentBeat;
                }
            }
        #endregion


        private MusicController musicController;

        private float beatStart;
        
        private float currentSample;
        private float currentBeat;


        /// <summary>
        /// The amount of samples between each beat
        /// </summary>
        private float beatSamplePeriod;



        public BeatCounter(Settings settings, MusicController musicController)
        {
            this.settings = settings;
            this.musicController = musicController;
        }

        #region Public Functions
        public bool HasStarted
        {
            get
            {
                return musicController.HasStarted;
            }
        }
        public float GetFullProgress()
        {
            return currentBeat;
        }
        public float GetBeatProgress()
        {
            return currentBeat - CompletedBeats;
        }
        public float GetBeatProgress(int beat, float max = Mathf.Infinity)
        {
            return Math.Min(currentBeat - beat, max);
        }
        
        public float GetMeasureProgress()
        {
            return currentBeat / settings.measureSignature.MeasureSize() - CompletedMeasures;
        }
        public float GetMeasureProgress(int msr, float max = Mathf.Infinity)
        {
            return Math.Min(currentBeat / settings.measureSignature.MeasureSize() - msr, max);
        }

        public float GetProgress(BeatUnit unit)
        {
            switch(unit)
            {
                case BeatUnit.BEAT:
                    return GetBeatProgress();
                case BeatUnit.MEASURE:
                    return GetMeasureProgress();
                default:
                    throw new NotImplementedException();
            }
        }
        public float GetProgress(BeatUnit unit, int val, float max = Mathf.Infinity)
        {
            switch (unit)
            {
                case BeatUnit.BEAT:
                    return GetBeatProgress(val, max);
                case BeatUnit.MEASURE:
                    return GetMeasureProgress(val, max);
                default:
                    throw new ArgumentException();
            }
        }
            
        
        public int GetCompletedUnits(BeatUnit unit)
        {
            switch (unit)
            {
                case BeatUnit.BEAT:
                    return CompletedBeats;
                case BeatUnit.MEASURE:
                    return CompletedMeasures;
                default:
                    throw new ArgumentException();
            }
        }

       
        #endregion

        private void InitBeatVariables(double syncTime)
        {
            float bpm = musicController.BPM;
            float beatSecondsPeriod = 60f / bpm;
            beatSamplePeriod = beatSecondsPeriod * musicController.CurrentPatch.frequency;
            beatStart = (float)(syncTime * musicController.CurrentPatch.frequency);
        }

        public IEnumerator BeatCountCoroutine(double synctime)
		{
            InitBeatVariables(synctime);
			float eightBeatTarget = 0.25f;
			float halfBeatTarget = .5f;
            while(musicController.IsPlaying)
            {

                int beat = CompletedBeats;
                int measure = CompletedMeasures;
                currentSample = (float)AudioSettings.dspTime * musicController.CurrentPatch.frequency - beatStart;
                currentBeat = currentSample / beatSamplePeriod;


                //Events
                if (beat != CompletedBeats)
                {
					Debug.Assert(beat == 0 || CompletedBeats == beat + 1, String.Format("Completed beats has unexpected value of {0}, expected value is {1}", CompletedBeats, beat+1));
                    if(OnBeat != null)
                    {
                        OnBeat(this);
                    }
                } 
				if (currentBeat > halfBeatTarget) {
					halfBeatTarget = (halfBeatTarget + .5f);
					if (OnHalfBeat != null)
						OnHalfBeat (this);
				}
				if (currentBeat > eightBeatTarget) {
					eightBeatTarget = (eightBeatTarget + .25f);
					if (OnEigthBeat != null)
						OnEigthBeat (this);
				}
                if (measure != CompletedMeasures)
                {
                    Debug.Assert(CompletedMeasures == measure + 1);
                    if (OnMeasure != null)
                    {
                        OnMeasure(this);
                    }
                }


                yield return new WaitForSeconds(settings.updateRatio / 1000f);
            }
            
        }
	
    }
}
