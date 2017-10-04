﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AntonioHR.ConditionVariables.Editor
{
    [CustomPropertyDrawer(typeof(ConditionVariableValue))]
    public class ConditionVariableInspector : PropertyDrawer
    {
        public ConditionVariableValue obj;


        public override float GetPropertyHeight(SerializedProperty property, UnityEngine.GUIContent label)
        {

            return 3 * EditorGUIUtility.singleLineHeight;
        }


        public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label)
        {
            UpdateObject(property);
            if (obj == null)
                return;

            var jump = new Vector2(0, EditorGUIUtility.singleLineHeight);


            Rect r_full = new Rect(position.position, new Vector2(position.width, EditorGUIUtility.singleLineHeight));
            Rect r_label = new Rect(position.position, new Vector2(EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight));
            Rect r_value = new Rect(position.position + Vector2.right * EditorGUIUtility.labelWidth, 
                new Vector2(EditorGUIUtility.fieldWidth, EditorGUIUtility.singleLineHeight));
            
            EditorGUI.LabelField(r_label, property.name);


            obj.type = (ConditionVariableValue.Type)EditorGUI.EnumPopup(r_full.Translated(jump).ShrinkToRightAbsolute(10), "Type", obj.type);


            DrawPropertyValue(r_full.Translated(2 * jump).ShrinkToRightAbsolute(10));
        }

        private void DrawPropertyValue(Rect position)
        {
            switch (obj.type)
            {
                case ConditionVariableValue.Type.Integer:
                    obj.intValue = EditorGUI.IntField(position, "Starting Value", obj.intValue);
                    break;
                case ConditionVariableValue.Type.Boolean:
                    obj.boolValue = EditorGUI.Toggle(position, "Starting Value", obj.boolValue);
                    break;
                case ConditionVariableValue.Type.Float:
                    obj.floatValue = EditorGUI.FloatField(position, "Starting Value", obj.floatValue);
                    break;
                default:
                    break;
            }
        }

        private void UpdateObject(SerializedProperty property)
        {
            obj = (ConditionVariableValue)EditorHelper.GetTargetObjectOfProperty(property);
        }
    }
}