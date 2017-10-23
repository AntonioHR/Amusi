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




        
        private Settings settings;
        
        private float clipStart;
        private int clipFrequency;
        
        private float currentSample;
        private float beatSamplePeriod;

        
        public float Progress { get; private set; }

        

        public BeatCounter(Settings settings)
        {
            this.settings = settings;
        }

        public void UpdateClipVariables(double syncTime, float bpm, int frequency)
        {
            float beatSecondsPeriod = 60f / bpm;
            beatSamplePeriod = beatSecondsPeriod * frequency;
            clipStart = (float)(syncTime * frequency);
            clipFrequency = frequency;
        }

        public void Step()
        {
            currentSample = (float)AudioSettings.dspTime * clipFrequency - clipStart;

            Progress = currentSample / beatSamplePeriod;
        }

        
	
    }
}
