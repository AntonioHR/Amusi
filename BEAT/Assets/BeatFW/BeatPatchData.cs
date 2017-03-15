using System;
using UnityEngine;
using System.Collections.Generic;

namespace BeatFW
{
	[CreateAssetMenu()]
	public class BeatPatchData:ScriptableObject
	{
		public BeatPatch[] patches;

        public UnityEditorInternal.ReorderableList patchesL;

		public Dictionary<string, AudioClip> CreateDictionary()
		{
			Dictionary<string, AudioClip> dict = new Dictionary<string, AudioClip> ();
			foreach (var clip in patches) {
				dict.Add (clip.name, clip.clip);
			}
			return dict;
		}
	}
	[Serializable]
	public class BeatPatch
	{
		public string name;
		public AudioClip clip;
	}
}

