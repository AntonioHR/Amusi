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
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
            var r1 = position;
            r1.width *= .3f;
            GUI.Label(r1, label);
            var r2 = new Rect(position.x + r1.width, position.y, position.width - r1.width, r1.height);
            if (GUI.Button(r2, new GUIContent("Edit")))
            {
                BeatPatternWindow.Show(property);
            }
        }
    }
}
