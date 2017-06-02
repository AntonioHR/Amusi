using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using BeatFW.Util;

public class GridWindow : EditorWindow
{
    //public Vector2 drag;
    [NonSerialized]
    bool changedGrid = true;

    public Grid grid;
    Vector2 gridScroll;

    static Rect gridviewRect = new Rect(-500, -500, 1000, 1000);
    GUIContent gridFieldContent = new GUIContent("Grid");


    [MenuItem("Window/GridWindow")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(GridWindow));
    }


    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        grid = (Grid)EditorGUI.ObjectField(GUILayoutUtility.GetRect(gridFieldContent, EditorStyles.objectField), grid, typeof(Grid));
        if (EditorGUI.EndChangeCheck())
            changedGrid = true;

        var  offset = new Vector2(0, GUILayoutUtility.GetLastRect().yMax);
        float margin = 5;
        Rect grid_area = position.AtOrigin().Translated(offset).MinusMargin(Vector2.one * margin);

        GUI.BeginScrollView(grid_area, gridScroll, gridviewRect, GUIStyle.none, GUIStyle.none);

        var world = gridviewRect;
        var view = grid_area.At(gridScroll);

        gridScroll += DrawGrid(world, view, grid, changedGrid);
        gridScroll = gridScroll.Clamped(Vector2.zero, gridviewRect.size);

        GUI.EndScrollView();
        changedGrid = false;
        
    }


    public static Vector2 DrawGrid(Rect world, Rect currentWorldView, Grid grid, bool resetPos = false)
    {
        Vector2 dragResult = Vector2.zero;
        if (grid == null)
            return dragResult;

        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        var eventType = Event.current.GetTypeForControl(controlID);
        var state = (GridDrawState)GUIUtility.GetStateObject(typeof(GridDrawState), controlID);

        Vector2 gridSpaceMousePos = Event.current.mousePosition;

        Grid.Box target = grid.GetByClick(gridSpaceMousePos);

        //Temporario
        bool isMouseInside = true;

        switch (eventType)
        {
            case EventType.MouseDown:
                if(isMouseInside)
                {
                    GUIUtility.hotControl = controlID;
                    if (HandleClick(grid, target, gridSpaceMousePos, state))
                        Event.current.Use();
                }
                break;
            case EventType.ContextClick:
                if(isMouseInside)
                {
                    GUIUtility.hotControl = controlID;
                    if (HandleContextClick(grid, target, state))
                        Event.current.Use();
                }
                break;
            case EventType.MouseDrag:
                if (GUIUtility.hotControl == controlID)
                {
                    if (state.selection == null)
                        dragResult = - Event.current.delta;
                    else
                        state.selection.position += Event.current.delta;
                    Event.current.Use();
                }
                break;
            case EventType.MouseUp:
                if (GUIUtility.hotControl == controlID)
                    GUIUtility.hotControl = 0;
                break;
            case EventType.Repaint:
                GridRepaint(world, grid, state);
                Vector2 offset = grid.object_size * .5f;

                var x = GUI.matrix;
                foreach (var link in grid.links)
                {
                    var m = GUI.matrix;
                    var b1 = grid.boxes[link.originIndex].position + offset;
                    var b2 = grid.boxes[link.targetIndex].position + offset;
                    EditorDraw.DrawLine(b1, b2, Color.red);
                }
                EditorGUI.DrawRect(new Rect(0, 0, 100, 3), Color.green);
                EditorGUI.DrawRect(new Rect(0, 0, 3, 100), Color.green);
                var dot = new Rect(0, 0, 10, 10);
                EditorGUI.DrawRect(dot.At(world.min), Color.blue);
                EditorGUI.DrawRect(dot.At(world.max).Translated(-dot.size * 2), Color.blue);
                break;
        }

        return dragResult;
    }

    static void Remove(Grid grid, Grid.Box target, GridDrawState state)
    {
        if (target != null)
        {
            grid.boxes.Remove(target);
            if (state.selection == target)
                state.selection = null;
        }
    }
    static bool HandleClick(Grid grid, Grid.Box target, Vector2 gridSpaceMousePos, GridDrawState state)
    {
        var button = Event.current.button;
        var doubleClick = Event.current.clickCount == 2;
        var singleClick = Event.current.clickCount == 1;

        //If Double Click With No Target, create Box
        if (button == 0 && doubleClick && target == null)
        {
            Grid.Box b = new Grid.Box();
            b.position = gridSpaceMousePos;
            b.position.x = Mathf.Round(b.position.x);
            b.position.y = Mathf.Round(b.position.y);
            grid.boxes.Add(b);
            return true;
        }

        if (button == 0 && singleClick)
        {
            //If Single Click With Target, select it
            state.selection = target;
            return true;
        }

        if (target != null && Event.current.button == 2)
        {
            Remove(grid, target, state);
            return true;
        }
        return false;
    }
    static bool HandleContextClick(Grid grid, Grid.Box target, GridDrawState state)
    {
        if (target != null && state.selection != null && target != state.selection)
        {
            grid.links.Add(grid.GenerateLink(state.selection, target));
            return true;
        }
        return false;
    }
    static void GridRepaint(Rect world, Grid grid, GridDrawState state)
    {
        EditorGUI.DrawRect(world, Color.gray);
        Rect base_box_rect = new Rect(Vector2.zero, grid.object_size);
        foreach (var obj in grid.boxes)
        {
            Rect r = base_box_rect.Translated(obj.position);
            EditorGUI.DrawRect(r, obj == state.selection ? Color.red : Color.white);
        }
    }
}
public class GridDrawState
{
    //public Vector2 drag;
    public Grid.Box selection;
}

