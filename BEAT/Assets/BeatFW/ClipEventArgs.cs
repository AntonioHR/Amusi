using System;
using UnityEngine;

namespace BeatFW
{
	public class ClipEventArgs:EventArgs
	{
		public double Time {get; private set;}
		public AudioClip Clip {get; private set;}
		public string Patch {get;private set;}
		public ClipEventArgs (double time, AudioClip clip, string patch)
		{
			this.Time = time;
			this.Clip = clip;
			this.Patch = patch;
		}
	}
}

