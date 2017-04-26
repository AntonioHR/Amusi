using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BeatFW.Util;

public class StyleTestWindow : EditorWindow {


    SimpleArray arr;
    GUISkin skin;
    GUIStyle toggles;

    [MenuItem("Window/StyleTests")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(StyleTestWindow));
    }

    void Awake()
    {
        skin = Resources.Load<GUISkin>("TestSkin");
        toggles = skin.button;
    }

    void OnGUI()
    {
        //Vector2 margin = 20 * Vector2.one;
        //EditorGUI.DrawRect(position.AtOrigin().MinusMargin(margin), Color.gray);
        Rect inside = position.AtOrigin().Resized(Vector2.one * .75f);
        EditorGUI.DrawRect(inside, Color.gray);

        arr = (SimpleArray)EditorGUI.ObjectField(new Rect(inside.position - Vector2.up * 2 * EditorGUIUtility.singleLineHeight, new Vector2(100, EditorGUIUtility.singleLineHeight)), arr, typeof(SimpleArray));
        arr.offset = EditorGUI.FloatField(new Rect(inside.position - Vector2.up * EditorGUIUtility.singleLineHeight, new Vector2(100, EditorGUIUtility.singleLineHeight)), "Offset", arr.offset);

        int length = arr.data.Length;
        var grid = inside.GetMaskedHorizontalGridInside(arr.offset, length, 30, 10, Vector2.one * 10);
        for (int i = 0; i < length; i++)
        {
            if(grid.isValid[i])
                arr.data[i] = GUI.Toggle(grid.Slots[i], arr.data[i], GUIContent.none, toggles);
        }
    }
}
