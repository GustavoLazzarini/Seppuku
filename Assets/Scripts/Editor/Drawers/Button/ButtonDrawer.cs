//Copyright Galactspace 2022

using UnityEngine;
using UnityEditor;
using Core.Attributes;

namespace Drawers
{
    [CustomPropertyDrawer(typeof(ButtonAttribute))]
    public class ButtonDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.Boolean)
            {
                var btn = GUI.Button(position, label);
                if (btn)
                {
                    property.boolValue = true;
                }
            }
            else EditorGUI.LabelField(position, label.text, "Use [Button] with bools.");
        }
    }
}