// Copyright Ramses Jelsma, 2020

using MoleSystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(MoleBehaviour.FloatRange))]
public class FloatRangeDrawer : PropertyDrawer
{
    const int fieldwidth = 50;


    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        // Create property container element.
        var container = new VisualElement();

        var min = new PropertyField(property.FindPropertyRelative("min"));
        var max = new PropertyField(property.FindPropertyRelative("max"));

        container.Add(min);
        container.Add(max);

        return container;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        var minRect = new Rect(position.x, position.y, 30, position.height);
        var dashRect = new Rect(position.x + 30, position.y, 10, position.height);
        var maxRect = new Rect(position.x + 40, position.y, 30, position.height);


        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(minRect, property.FindPropertyRelative("min"), GUIContent.none);
        EditorGUI.LabelField(dashRect, new GUIContent("-"));
        EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("max"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}