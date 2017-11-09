using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AntonioHR.Amusi.Editor;
using System.Linq;
using System;
using AntonioHR.Amusi.Data.Nodes;

namespace AntonioHR.Amusi.Editor
{
    [CustomEditor(typeof(ConditionMusicTreeNode))]
    public class ConditionMusicTreeNodeInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var cond = target as ConditionMusicTreeNode;
            ShowTreeCondition(cond);
        }

        private void ShowTreeCondition(ConditionMusicTreeNode cond)
        {
            var treeAsset = MusicTreeEditorManager.Instance.TreeAsset;
            if(treeAsset == null)
            {
                return;
            }

            var treeVars = treeAsset.vars;
            var varNames = treeVars.Select(x => x.name).ToArray();

            int treeVarIndex = treeVars.FindIndex(x => x.name == cond.condition.variableName);


            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Variable");
            treeVarIndex = EditorGUILayout.Popup(treeVarIndex, varNames);
            EditorGUILayout.EndHorizontal();



            if (treeVarIndex == -1)
            {
                cond.condition.variableName = "";
                return;
            }

            cond.condition.variableName = varNames[treeVarIndex];
            var currentVar = treeVars[treeVarIndex];
            ShowVarCondition(cond, currentVar);
        }

        private static void ShowVarCondition(ConditionMusicTreeNode cond, ConditionVariables.ConditionVariable currentVar)
        {
            switch (currentVar.value.type)
            {
                case ConditionVariables.ConditionVariableValue.Type.Boolean:
                    ShowBoolVarCondition(cond);
                    break;
                case ConditionVariables.ConditionVariableValue.Type.Float:
                    ShowFloatVarCondition(cond, currentVar);
                    break;
                case ConditionVariables.ConditionVariableValue.Type.Integer:
                    ShowIntVarCondition(cond);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void ShowBoolVarCondition(ConditionMusicTreeNode cond)
        {
            cond.condition.boolVal = GUILayout.Toggle(cond.condition.boolVal, new GUIContent("Value"));
        }

        private static void ShowIntVarCondition(ConditionMusicTreeNode cond)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Condition Type");
            cond.condition.intCondition = (ConditionVariables.Condition.IntCondition)EditorGUILayout.EnumPopup((Enum)cond.condition.intCondition);
            EditorGUILayout.EndHorizontal();

            cond.condition.intVal = EditorGUILayout.IntField("Value", cond.condition.intVal);
            
        }

        private static void ShowFloatVarCondition(ConditionMusicTreeNode cond, ConditionVariables.ConditionVariable currentVar)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Condition Type");
            cond.condition.floatCondition = (ConditionVariables.Condition.FloatCondition)EditorGUILayout.EnumPopup((Enum)cond.condition.floatCondition);
            EditorGUILayout.EndHorizontal();

            cond.condition.floatVal = EditorGUILayout.FloatField("Value", cond.condition.floatVal);
        }
    }
}