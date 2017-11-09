using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AntonioHR.ConditionVariables;
using UnityEngine;
using UnityEditor;

namespace AntonioHR.Amusi.Editor.Windows.MusicTree.Internal
{
    public static class VarsGUI
    {
        private static string tempVarName = "";

        public static void DrawVarsEditor()
        {
            foreach (var treeVar in MusicTreeEditorManager.Instance.TreeAsset.vars)
            {
                bool deletedAny = false;
                DrawVarEditor(treeVar, out deletedAny);
                if (deletedAny)
                    break;
            }


            using (var hor = new GUILayout.HorizontalScope(MusicTreeEditorWindow.configs.Skin.box))
            {
                tempVarName = GUILayout.TextField(tempVarName);
                EditorGUI.BeginDisabledGroup(tempVarName.Length == 0);
                if (GUILayout.Button("+", GUILayout.Width(20)))
                {
                    OpenNewVarMenu();
                }
                EditorGUI.EndDisabledGroup();
            }
        }
        private static void DrawVarEditor(ConditionVariables.ConditionVariable treeVar, out bool deletedAny)
        {
            EditorGUILayout.BeginHorizontal(MusicTreeEditorWindow.configs.Skin.box);
            GUILayout.Label(treeVar.name);
            GUILayout.FlexibleSpace();
            deletedAny = false;

            var valueDescripton = treeVar.value;
            switch (valueDescripton.type)
            {
                case ConditionVariables.ConditionVariableValue.Type.Integer:
                    valueDescripton.intValue = EditorGUILayout.IntField(valueDescripton.intValue, GUILayout.Width(50));
                    break;
                case ConditionVariables.ConditionVariableValue.Type.Boolean:
                    valueDescripton.boolValue = EditorGUILayout.Toggle(valueDescripton.boolValue, GUILayout.Width(50));
                    break;
                case ConditionVariables.ConditionVariableValue.Type.Float:
                    valueDescripton.floatValue = EditorGUILayout.FloatField(valueDescripton.floatValue, GUILayout.Width(50));
                    break;
                default:
                    break;
            }
            if (GUILayout.Button("x", GUILayout.Width(20)))
            {
                DeleteVar(treeVar);
                deletedAny = true;
            }
            EditorGUILayout.EndHorizontal();
            var varArea = GUILayoutUtility.GetLastRect();

            if (Event.current.button == 1 && varArea.Contains(Event.current.mousePosition))
            {
                OpenVarMenu(treeVar);
            }
        }
        private static void OpenVarMenu(ConditionVariables.ConditionVariable treeVar)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent(string.Format("Delete {0}", treeVar.name)), false, () => DeleteVar(treeVar));
            menu.ShowAsContext();
        }
        private static void OpenNewVarMenu()
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Boolean"), false, () => CreateVar(ConditionVariableValue.Type.Boolean));
            menu.AddItem(new GUIContent("Integer"), false, () => CreateVar(ConditionVariableValue.Type.Integer));
            menu.AddItem(new GUIContent("Float"), false, () => CreateVar(ConditionVariableValue.Type.Float));
            menu.ShowAsContext();
        }

        private static void CreateVar(ConditionVariableValue.Type type)
        {
            var newVar = new ConditionVariable
            {
                name = tempVarName,
                value = new ConditionVariableValue()
                {
                    type = type
                }
            };
            tempVarName = "";
            MusicTreeEditorManager.Instance.TreeAsset.vars.Add(newVar);
        }
        private static void DeleteVar(ConditionVariable treeVar)
        {
            MusicTreeEditorManager.Instance.TreeAsset.vars.Remove(treeVar);
        }
    }
}
