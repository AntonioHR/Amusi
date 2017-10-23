using UnityEngine;
using System.Collections;
using System;

namespace AntonioHR.MusicTree.BeatSync.Internal
{
	public class BeatCounter
    {
        
        private float clipStart;
        private int clipFrequency;
        
        private float currentSample;
        private float beatSamplePeriod;

        
        public float Progress { get; private set; }

        

        public BeatCounter()
        {
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
