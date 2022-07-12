//Made by Galactspace Studios

using UnityEngine;
using UnityEditor;

namespace Tools.ScriptCreator
{
    [System.Serializable]
    public class ScMethod
    {
        public string methodName;
        public ScType returnType;
        public string customReturnType;
        public int parametersLength;
    }
    
    [CustomPropertyDrawer(typeof(ScMethod))]
    public class ScMethodDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float propOffset = 100;

            var createPropRect = new Rect(propOffset + position.x, position.y, 20, position.height);
            var varNameRect = new Rect(propOffset + position.x + 25, position.y, 120, position.height);
            var varTypeRect = new Rect(propOffset + position.x + 25 + 120 + 5, position.y, 85, position.height);
            var varCustomTypeRect = new Rect(propOffset + position.x + 25 + 120 + 85 + 10, position.y, 120, position.height);

            SerializedProperty varTypeProp = property.FindPropertyRelative("returnType");

            EditorGUI.PropertyField(varTypeRect, varTypeProp, GUIContent.none);

            if ((ScType)varTypeProp.intValue != ScType.Space)
            {                
                EditorGUI.PropertyField(createPropRect, property.FindPropertyRelative("parametersLength"), GUIContent.none);
                EditorGUI.PropertyField(varNameRect, property.FindPropertyRelative("methodName"), GUIContent.none);
            }

            if ((ScType)varTypeProp.intValue == ScType.Custom)
            {
                EditorGUI.PropertyField(varCustomTypeRect, property.FindPropertyRelative("customReturnType"), GUIContent.none);
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}
