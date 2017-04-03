using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EditInWindowAttribute))]
class EditInWindowPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label)
    {
        Rect r1 = position, r2 = position;
        r1.width =  r1.width * .5f;
        r2.x = r1.x + r1.width;
        r2.width =  r2.width * .5f;

        GUI.Label(r1, label);
        if (GUI.Button(r2, "Edit"))
        {
            SimplePropertyWindow.ShowWith(property);
        }
    }
}

