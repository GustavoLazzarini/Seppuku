//Made by Galactspace Studios

using UnityEngine;
using UnityEditor;

namespace Tools.ScriptCreator
{
    [System.Serializable]
    public class ScVariable
    {
        public string propertyName;
        public bool createProperty;
        public ScType variableType;
        public string customTypeName;
    }

    [CustomPropertyDrawer(typeof(ScVariable))]
    public class ScVariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float propOffset = 100;

            var createPropRect = new Rect(propOffset + position.x, position.y, 5, position.height);
            var varNameRect = new Rect(propOffset + position.x + 20, position.y, 120, position.height);
            var varTypeRect = new Rect(propOffset + position.x + 20 + 120 + 5, position.y, 85, position.height);
            var varCustomTypeRect = new Rect(propOffset + position.x + 20 + 120 + 85 + 10, position.y, 120, position.height);

            SerializedProperty varTypeProp = property.FindPropertyRelative("variableType");

            EditorGUI.PropertyField(varTypeRect, varTypeProp, GUIContent.none);

            if ((ScType)varTypeProp.intValue != ScType.Space)
            {
                EditorGUI.PropertyField(createPropRect, property.FindPropertyRelative("createProperty"), GUIContent.none);
                EditorGUI.PropertyField(varNameRect, property.FindPropertyRelative("propertyName"), GUIContent.none);
            }

            if ((ScType)varTypeProp.intValue == ScType.Custom)
            {
                EditorGUI.PropertyField(varCustomTypeRect, property.FindPropertyRelative("customTypeName"), GUIContent.none);
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
