using TMPro;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Plawius.Editor
{
    [CustomPropertyDrawer(typeof(ControllerIcons.PlatformDefault))]
    public class PlatformDefaultDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            var w = position.width / 2;
            var amountRect = new Rect(position.x, position.y, w, position.height);
            var unitRect = new Rect(position.x + w + 5, position.y, w - 5, position.height);

            EditorGUI.PropertyField(amountRect, property.FindPropertyRelative("platform"), GUIContent.none);
            EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("fontAsset"), GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }

    [CustomEditor(typeof(ControllerIcons))]
    public class ControllerIconsEditor : UnityEditor.Editor
    {
        private SerializedProperty m_platformDefaults;
        private SerializedProperty m_activeFont;
        private ReorderableList m_reorderableList;

        private void OnEnable()
        {
            m_activeFont = serializedObject.FindProperty("m_activeFont");
            m_platformDefaults = serializedObject.FindProperty("m_platformDefaults");

            m_reorderableList = new ReorderableList(serializedObject, m_platformDefaults, false, true, true, true)
            {
                drawHeaderCallback = DrawHeaderCallback,
                drawElementCallback = DrawElementCallback,
                elementHeightCallback = ElementHeightCallback,
                onAddCallback = OnAddCallback
            };

        }

        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, "Platform defaults");
        }

        private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = m_reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), element, GUIContent.none, true);
        }

        private float ElementHeightCallback(int index)
        {
            var propertyHeight = EditorGUI.GetPropertyHeight(m_reorderableList.serializedProperty.GetArrayElementAtIndex(index), true);
            var spacing = EditorGUIUtility.singleLineHeight / 2;
            return propertyHeight + spacing;
        }

        private void OnAddCallback(ReorderableList list)
        {
            var index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var oldFont = m_activeFont.objectReferenceValue;
            EditorGUILayout.PropertyField(m_activeFont, new GUIContent("Active Font"));
            var newFont = m_activeFont.objectReferenceValue;
            if (newFont != oldFont)
            {
                (serializedObject.targetObject as ControllerIcons)?.SetActiveFont(newFont as TMP_FontAsset);
            }

            if (GUILayout.Button("Initialize"))
            {
                (serializedObject.targetObject as ControllerIcons)?.ForceInit();
            }

            EditorGUILayout.Space();

            m_reorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}