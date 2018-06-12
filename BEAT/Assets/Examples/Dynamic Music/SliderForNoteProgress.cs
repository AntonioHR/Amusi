using UnityEngine;
using UnityEditor;
using AntonioHR.Amusi;
using UnityEngine.UI;

//[RequireComponent]
[RequireComponent(typeof(Slider))]
public class SliderForNoteProgress : MonoDancer
{
    Slider slider;
    //Use this instead of the MonoBehaviour Start Function
    protected override void Init()
    {
        slider = GetComponent<Slider>();
        slider.value = 0;
        slider.maxValue = 1;
        slider.minValue = 0;
    }
    //This is Called whenever a note starts playing
    protected override void OnNoteStart()
    {
        slider.value = 0;
    }
    //Use this is called every frame while a note plays. 
    //Progress goes from 0 to 1 as the note progresses
    protected override void OnNoteUpdate(float progress)
    {
        slider.value = progress;
    }

    protected override void OnNoteEnd()
    {
        slider.value = 1;
    }
}