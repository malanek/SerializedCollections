#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
namespace BBExtensions.AttributeDrawers
{
    internal sealed class ExpandableInstanceDrawer
    {
        private bool isExpanded = false;
        private Editor cachedEditor = null;
        private readonly SerializedProperty property;
        private readonly GUIContent label;

        public ExpandableInstanceDrawer(SerializedProperty property, GUIContent label)
        {
            this.property = property;
            this.label = label;
        }

        public void OnGUI(Rect position)
        {
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                EditorGUI.LabelField(position, label.text, "Only reference values can be expandable!");
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), property, label);

            if (property.objectReferenceValue != null)
            {
                isExpanded = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), isExpanded, GUIContent.none, true);

                if (isExpanded)
                {
                    if (cachedEditor == null)
                        Editor.CreateCachedEditor(property.objectReferenceValue, null, ref cachedEditor);
                    if (cachedEditor != null)
                        cachedEditor.OnInspectorGUI();
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
#endif