using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RegexAttribute))]
public class RegexPropertyDrawer : PropertyDrawer
{
    const int helpHeight = 30;
    const int textHeight = 16;

    RegexAttribute regexAttribute { get { return ((RegexAttribute)attribute); } }

    public override float GetPropertyHeight(SerializedProperty property, UnityEngine.GUIContent label)
    {
        if (isValid(property))
            return base.GetPropertyHeight(property, label);
        else
            return base.GetPropertyHeight(property, label) + helpHeight;
    }

    public override void OnGUI(UnityEngine.Rect position, SerializedProperty property, UnityEngine.GUIContent label)
    {
        //Adjust the height of the text field;
        Rect textPosition = position;
        textPosition.height = textHeight;
        DrawTextField(textPosition, property, label);


        //Adjust the help box position to appear indented underneath the text field
        Rect helpPosition = EditorGUI.IndentedRect(position);
        helpPosition.y += textHeight;
        helpPosition.height = helpHeight;
        DrawHelpBox(helpPosition, property);
    }

    private void DrawTextField(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();
        string value = EditorGUI.TextField(position, label, property.stringValue);
        if(EditorGUI.EndChangeCheck())
            property.stringValue = value;
    }

    private void DrawHelpBox(Rect helpPosition, SerializedProperty property)
    {
        if(isValid(property))
            return;
        EditorGUI.HelpBox(helpPosition, regexAttribute.helpMessage, MessageType.Error);        
    }

    private bool isValid(SerializedProperty property)
    {
        return Regex.IsMatch(property.stringValue, regexAttribute.pattern);
    }
}
