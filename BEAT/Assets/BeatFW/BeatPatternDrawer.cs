using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace BeatFW.Editor
{
    [CustomPropertyDrawer(typeof(BeatPattern))]
    public class BeatPatternDrawer : PropertyDrawer
    {
        void OnGUI(Rect position, SerializedProperty property, GUIContent label){
            
            if(GUILayout.Button("Test"))
            {
                Debug.Log("Yass!");
            }
        }
    }
}
