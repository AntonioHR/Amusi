using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.BeatFW.Editor
{
    [CustomPropertyDrawer(typeof(BeatPatternAsset))]
    public class BeatPatternDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 3 * EditorGUIUtility.singleLineHeight;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
            Rect r1 = position;
            r1.height /= 3;
            EditorGUI.ObjectField(r1, property);
            Rect r2 = r1;
            r2.y += r1.height;
            var pattAsset = (BeatPatternAsset)property.objectReferenceValue;
            EditorGUI.BeginDisabledGroup(pattAsset == null);
            if (GUI.Button(r2, "Edit"))
            {
                BeatPatternWindow.Show(pattAsset);
            }

            Rect r3 = r2;
            r3.y += r2.height;
            if (property.objectReferenceValue != null)
            {
                BeatPatternWindow.DrawPattern(r3, pattAsset.pattern);
            }
            EditorGUI.EndDisabledGroup();

        }
    }
}
