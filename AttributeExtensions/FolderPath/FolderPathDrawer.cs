#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace BBExtensions.AttributeDrawers.Internal
{
    [CustomPropertyDrawer(typeof(FolderPathAttribute))]
    internal sealed class FolderPathDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, label.text, "[FolderPath] only works with strings!");
                return;
            }
            EditorGUI.BeginProperty(position, label, property);
            DrawProperty(position, property, label);
            EditorGUI.EndProperty();
        }

        private void DrawProperty(Rect position, SerializedProperty property, GUIContent label)
        {
            float labelWidth = EditorGUIUtility.labelWidth;
            Rect labelRect = new(position.x, position.y, labelWidth, position.height);
            Rect textFieldRect = new(position.x + labelWidth, position.y, position.width - labelWidth - 27, position.height);
            Rect buttonRect = new(position.x + position.width - 25, position.y, 25, position.height);

            EditorGUI.LabelField(labelRect, label);
            property.stringValue = EditorGUI.TextField(textFieldRect, property.stringValue);

            if (GUI.Button(buttonRect, "../"))
                SelectFolder(property);
        }

        private void SelectFolder(SerializedProperty property)
        {
            string path = EditorUtility.OpenFolderPanel("Select folder", "", "");

            if (!string.IsNullOrEmpty(path))
            {
                if (path.StartsWith(Application.dataPath))
                    path = "Assets" + path.Substring(Application.dataPath.Length);

                Undo.RecordObject(property.serializedObject.targetObject, "Set Folder Path");
                property.stringValue = path;
                property.serializedObject.ApplyModifiedProperties();
            }
            GUIUtility.ExitGUI();
        }
    }
}
#endif
