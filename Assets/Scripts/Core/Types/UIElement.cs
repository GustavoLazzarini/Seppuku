//Made by Galactspace Studios

using UnityEngine;
using UnityEngine.Localization;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Core.Types
{
    public enum InterfaceElementType 
    {
        Button,
        Label,
        Slider,
        SliderInt,
        Toggle,
        VisualElement,
        StaticSlider
    }

    [System.Serializable]
    public class UIElement
    {
        public string elementName;
        public LocalizedString elementText;
        public InterfaceElementType elementType;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(UIElement))]
    public class UIElementTypeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect nameRect = new Rect(position.x, position.y, 100, position.height - 2);
            Rect bigNameRect = new Rect(position.x, position.y, 240, position.height - 2);
            Rect textRect = new Rect(position.x - 10, position.y, 250, position.height);
            Rect typeRect = new Rect(position.x + 245, position.y, 100, position.height);

            SerializedProperty elName = property.FindPropertyRelative("elementName");
            SerializedProperty elText = property.FindPropertyRelative("elementText");
            SerializedProperty elType = property.FindPropertyRelative("elementType");


            if ((InterfaceElementType)elType.enumValueIndex != InterfaceElementType.VisualElement)
            {
                EditorGUI.PropertyField(nameRect, elName, GUIContent.none);
                EditorGUI.PropertyField(textRect, elText, GUIContent.none);
            }
            else
            {
                EditorGUI.PropertyField(bigNameRect, elName, GUIContent.none);
            }

            EditorGUI.PropertyField(typeRect, elType, GUIContent.none);

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
#endif
}
