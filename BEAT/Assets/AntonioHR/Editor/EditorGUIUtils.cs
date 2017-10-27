using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace AntonioHR.Editor
{
    public static class EditorGUIUtils
    {
        class DoubleClickStateObj
        {
            public double lastClick;
        }
        public static int highestDepthId;
        public static float maxInterval = .4f;

        public static bool DoubleClickArea(Rect bounds)
        {
            int id = GUIUtility.GetControlID(FocusType.Passive);

                var stateObj = (DoubleClickStateObj)GUIUtility.GetStateObject(typeof(DoubleClickStateObj), id);

            if (GUI.Button(bounds, GUIContent.none, GUIStyle.none))
            {
                double currentTime = EditorApplication.timeSinceStartup;
                double delta = currentTime - stateObj.lastClick;
                if (delta < maxInterval)
                {
                    return true;
                }
                stateObj.lastClick = currentTime;
            }
            return false;
        }
    }

    
}
