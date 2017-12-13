using AntonioHR.Amusi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicTreeVariableSlider : MonoBehaviour {
    Slider slider;
    public string var;
    public MusicTreePlayer treePlayer;
    public Type varType;
    public float min;
    public float max;

    public enum Type { Float, Int}


	// Use this for initialization
	void Start () {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderChanged);
        slider.wholeNumbers = varType == Type.Int;
        slider.minValue = min;
        slider.maxValue = max;
        switch (varType)
        {
            case Type.Float:
                slider.value = treePlayer.GetFloatValue(var);
                break;
            case Type.Int:
                slider.value = treePlayer.GetIntValue(var);
                break;
            default:
                break;
        }
	}

    private void OnSliderChanged(float value)
    {
        switch (varType)
        {
            case Type.Float:
                treePlayer.SetFloatValue(var, value);
                break;
            case Type.Int:
                treePlayer.SetIntValue(var, (int)value);
                break;
            default:
                break;
        }
    }
}
