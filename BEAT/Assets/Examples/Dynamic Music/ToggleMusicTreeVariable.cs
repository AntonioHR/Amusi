using AntonioHR.Amusi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMusicTreeVariable : MonoBehaviour {
    Toggle toggle;
    public string var;
    public MusicTreePlayer treePlayer;

	// Use this for initialization
	void Start () {
        toggle = GetComponent<Toggle>();
        toggle.isOn = treePlayer.GetBoolValue(var);
        toggle.onValueChanged.AddListener(OnToggleVarChanged);
	}

    private void OnToggleVarChanged(bool value)
    {
        treePlayer.SetBoolValue(var, value);
    }
}
