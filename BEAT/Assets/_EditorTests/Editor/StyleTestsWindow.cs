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
    float dummy;
    void OnGUI()
    {
        //Vector2 margin = 20 * Vector2.one;
        //EditorGUI.DrawRect(position.AtOrigin().MinusMargin(margin), Color.gray);
        Rect inside = position.AtOrigin().Resized(Vector2.one * .75f);
        EditorGUI.DrawRect(inside, Color.gray);

        arr = (SimpleArray)EditorGUI.ObjectField(new Rect(inside.position - Vector2.up * 2 * EditorGUIUtility.singleLineHeight, new Vector2(100, EditorGUIUtility.singleLineHeight)), arr, typeof(SimpleArray));

        if (arr != null)
        {
            int length = arr.data.Length;
            float spacing = 10;
            float slotWidth = 30;
            var gridArea = inside.MinusMargin(0, EditorGUIUtility.singleLineHeight, 0, 0);

            Vector2 margin = Vector2.one * 10;
            float viewWidth = gridArea.width;
            float totalWidth = (spacing * (length - 1)) + slotWidth * length + margin.x;
            totalWidth = totalWidth > viewWidth ? totalWidth : viewWidth;
            arr.offset = GUI.HorizontalScrollbar(new Rect(inside.position.x, inside.position.y, inside.width, EditorGUIUtility.singleLineHeight), arr.offset, viewWidth, 0, totalWidth);

            var grid = gridArea.GetMaskedHorizontalGridInside(arr.offset, length, slotWidth, spacing, margin);
            for (int i = 0; i < length; i++)
            {
                if (grid.isValid[i])
                    arr.data[i] = GUI.Toggle(grid.Slots[i], arr.data[i], GUIContent.none, toggles);
            }
        }
    }
}
