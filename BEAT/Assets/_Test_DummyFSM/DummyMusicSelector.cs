using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeatFW;

public class DummyMusicSelector : MonoBehaviour {
    public BeatMusicController musicController;
    public string patchToUse = "a.0";
	// Use this for initialization
    void Start()
    {
        musicController.Init(patchToUse);
        musicController.OnClipCloseToEnd += MusicController_OnClipCloseToEnd;
	}

    private void MusicController_OnClipCloseToEnd(object sender, ClipEventArgs e)
    {
        musicController.QueueUpPatch(patchToUse);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
