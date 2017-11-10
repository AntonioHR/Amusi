﻿using UnityEngine;

namespace AntonioHR.Amusi.BeatSynchronization
{
	public class BeatCounter
	{
		
		private double clipStart;
		private int clipFrequency;
		
		private double currentSample;
		private double beatSamplePeriod;

		
		public double Progress { get; private set; }

		

		public BeatCounter()
		{
		}

		public void UpdateClipVariables(double syncTime, float bpm, int frequency)
		{
			double beatSecondsPeriod = 60.0 / bpm;
			beatSamplePeriod = beatSecondsPeriod * frequency;
			clipStart = (syncTime * frequency);
			clipFrequency = frequency;
		}

		public void Step()
		{
			currentSample = AudioSettings.dspTime * clipFrequency - clipStart;
			Progress = currentSample / beatSamplePeriod;
		}

		
	
	}
}
