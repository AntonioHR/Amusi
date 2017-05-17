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
        //Rect r = GUILayoutUtility.GetRect(new GUIContent("Drag"), EditorStyles.objectField);
        //drag = (Vector2)EditorGUI.Vector2Field(r,GUIContent.none, drag);

        float offset = GUILayoutUtility.GetLastRect().yMax;
        float margin = 5;
        Rect grid_area = position.AtOrigin().Translated(new Vector2(0, offset)).MinusMargin(Vector2.one * margin);

        DrawGrid(grid_area, grid, changedGrid);
        changedGrid = false;
        
    }


    public static void DrawGrid(Rect rect, Grid grid, bool resetPos = false)
    {
        //Debug.Log(Event.current.mousePosition);
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        var eventType = Event.current.GetTypeForControl(controlID);
        if (grid == null)
            return;

        var state = (GridDrawState)GUIUtility.GetStateObject(typeof(GridDrawState), controlID);
        if(resetPos)
        {
            state.drag = grid.GetDefaultPosition() + rect.size/2;
        }

        switch (eventType)
        {
            case EventType.MouseDown:
                Vector2 widgetSpaceMousePos = Event.current.mousePosition - rect.position;
                Vector2 gridSpaceMousePos = widgetSpaceMousePos - state.drag;
                if(rect.Contains(Event.current.mousePosition) && Event.current.button == 0)
                {
                    Grid.Box target = grid.GetByClick(gridSpaceMousePos);
                    if (target == null && Event.current.clickCount == 2)
                    {
                        Grid.Box b = new Grid.Box();
                        b.position = gridSpaceMousePos;
                        grid.boxes.Add(b);
                        Event.current.Use();
                    }

                    if(Event.current.clickCount == 1)
                    {
                        if (target != null)
                        {
                            state.selection = target;
                            //grid.MoveForward(target);
                            Event.current.Use();
                        } else
                        {
                            state.selection = null;
                        }
                        GUIUtility.hotControl = controlID;
                    }
                } else if(rect.Contains(Event.current.mousePosition) && Event.current.button == 2)
                {
                    Grid.Box target = grid.GetByClick(gridSpaceMousePos);
                    if (target != null)
                    {
                        grid.boxes.Remove(target);
                        if (state.selection == target)
                            state.selection = null;
                    }
                }
                break;
            case EventType.ContextClick:
                widgetSpaceMousePos = Event.current.mousePosition - rect.position;
                gridSpaceMousePos = widgetSpaceMousePos - state.drag;
                if (rect.Contains(Event.current.mousePosition))
                {
                    Grid.Box target = grid.GetByClick(gridSpaceMousePos);
                    if (target != null && state.selection != null && target != state.selection)
                    {
                        grid.links.Add(grid.GenerateLink(state.selection, target));
                    }
                }
                break;
            case EventType.MouseDrag:
                if (GUIUtility.hotControl == controlID)
                {
                    if (state.selection == null)
                    {
                        state.drag += Event.current.delta;
                        GUI.changed = true;
                        Event.current.Use();
                    } else
                    {
                        state.selection.position += Event.current.delta;
                        GUI.changed = true;
                        Event.current.Use();
                    }
                }
                break;
            case EventType.MouseUp:
                if (GUIUtility.hotControl == controlID)
                {
                    GUIUtility.hotControl = 0;
                }
                break;
            case EventType.Repaint:
                EditorGUI.DrawRect(rect, Color.gray);
                Rect base_box_rect = new Rect(rect.position, grid.object_size);
                foreach (var obj in grid.boxes)
                {
                    Rect r = base_box_rect.Translated(obj.position + state.drag).Intersection(rect);
                    if (r.Drawable())
                        EditorGUI.DrawRect(r, obj == state.selection ? Color.red : Color.white);
                }
                break;
        }
        grid.drag_debug = state.drag;
    }
    
    
}
public class GridDrawState
{
    public Vector2 drag;
    public Grid.Box selection;
}

