using UnityEngine;
using System.Collections;
using System;

namespace BeatFW
{
    [RequireComponent(typeof(MusicController))]
	public class BeatCounter : MonoBehaviour
    {
        #region Editor variables
            /// <summary>
            /// The Counter update ratio in milisseconds
            /// </summary>
            [SerializeField]
            private float updateRatio = 30f;
            [SerializeField]
            private MeasureSignature measureSignature;
        #endregion


        #region Events
		public event EventHandler<BeatEventArgs> OnBeat;
		public event EventHandler<BeatEventArgs> OnHalfBeat;
		public event EventHandler<BeatEventArgs> OnEigthBeat;
        public event EventHandler<BeatEventArgs> OnMeasure;
        #endregion
        
        
        #region Public properties
            public int CompletedMeasures
            {
                get { return (int)(currentBeat / measureSignature.MeasureSize()); }
            }
            public int CompletedBeatsInMeasure
            {
                get { return (int)currentBeat % measureSignature.MeasureSize(); }
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


        private MusicController beatMusicController;

        private float beatStart;
        
        private float currentSample;
        private float currentBeat;
		private int pastTrackBeats = 0;


        /// <summary>
        /// The amount of samples between each beat
        /// </summary>
        private float beatSamplePeriod;


        #region Public Functions
        public bool HasStarted
        {
            get
            {
                return beatMusicController.HasStarted;
            }
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
            return currentBeat / measureSignature.MeasureSize() - CompletedMeasures;
        }
        public float GetMeasureProgress(int msr, float max = Mathf.Infinity)
        {
            return Math.Min(currentBeat / measureSignature.MeasureSize() - msr, max);
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
        
        //public void StartCounting()
        //{
            
        //}
       
		public void Init(MusicController controller)
        {
            beatMusicController = controller; 
            float bpm = beatMusicController.BPM;
            float beatSecondsPeriod = 60f / bpm;
            beatSamplePeriod = beatSecondsPeriod * beatMusicController.CurrentPatch.frequency;
        }
        public void StartBeatUpdate(double syncTime)
        {
            beatStart = (float)(syncTime * beatMusicController.CurrentPatch.frequency);
            StartCoroutine(BeatUpdate());
        }
        #endregion
        

        private IEnumerator BeatUpdate ()
		{
			float eightBeatTarget = 0.25f;
			float halfBeatTarget = .5f;
            while(beatMusicController.IsPlaying)
            {

                int beat = CompletedBeats;
                int measure = CompletedMeasures;
                currentSample = (float)AudioSettings.dspTime * beatMusicController.CurrentPatch.frequency - beatStart;
                currentBeat = currentSample / beatSamplePeriod;
                if(beat != CompletedBeats)
                {
					Debug.Assert(beat == 0 || CompletedBeats == beat + 1, String.Format("Completed beats has unexpected value of {0}, expected value is {1}", CompletedBeats, beat+1));
                    if(OnBeat != null)
                    {
                        OnBeat(this, new BeatEventArgs());
                    }
                } 
				if (currentBeat > halfBeatTarget) {
					halfBeatTarget = (halfBeatTarget + .5f);
					if (OnHalfBeat != null)
						OnHalfBeat (this, new BeatEventArgs ());
				}
				if (currentBeat > eightBeatTarget) {
					eightBeatTarget = (eightBeatTarget + .25f);
					if (OnEigthBeat != null)
						OnEigthBeat (this, new BeatEventArgs ());
				}
                if (measure != CompletedMeasures)
                {
                    Debug.Assert(CompletedMeasures == measure + 1);
                    if (OnMeasure != null)
                    {
                        OnMeasure(this, new BeatEventArgs());
                    }
                }
                yield return new WaitForSeconds(updateRatio / 1000f);
            }
            
        }
	
		void OnBeatTrackChange(object sender, ClipEventArgs e)
		{
			pastTrackBeats = Mathf.CeilToInt (currentBeat);
			beatStart = (float)e.Time;
		}
    
    }
}
