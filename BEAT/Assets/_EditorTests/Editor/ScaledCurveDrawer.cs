using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ScaledCurve))]
public class ScaledCurveDrawer : PropertyDrawer {
    const int curveWidth = 50;
    const float min = 0;
    const float max = 1;

    public override void OnGUI(UnityEngine.Rect pos, SerializedProperty prop, UnityEngine.GUIContent label)
    {

        SerializedProperty scale = prop.FindPropertyRelative("scale");
        SerializedProperty curve = prop.FindPropertyRelative("curve");

        //Draw Scale
        var r = new Rect(pos.x, pos.y, pos.width - (20 + curveWidth ), pos.height);
        EditorGUI.Slider(r, scale, min, max, label);

        //Draw Curve
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        var r2 = new Rect (pos.width - curveWidth, pos.y, curveWidth, pos.height);
        EditorGUI.PropertyField(r2, curve, GUIContent.none);
        EditorGUI.indentLevel = indent;
    }
}
