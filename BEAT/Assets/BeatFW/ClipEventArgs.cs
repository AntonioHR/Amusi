using System;
using UnityEngine;

namespace BeatFW
{
	public class ClipEventArgs:EventArgs
	{
		public double Time {get; private set;}
		public AudioClip Clip {get; private set;}
		public ClipEventArgs (double time, AudioClip clip)
		{
			this.Time = time;
			this.Clip = clip;
		}
	}
}

