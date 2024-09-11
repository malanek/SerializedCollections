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
        private readonly bool canDraw;
        private Rect bgRect;

        public ExpandableInstanceDrawer(SerializedProperty property, GUIContent label)
        {
            this.property = property;
            this.label = label;
            canDraw = property.propertyType != SerializedPropertyType.ObjectReference || IsPartOfArray(property);
        }

        public void OnGUI(Rect position)
        {
            if (canDraw)
            {
                EditorGUI.LabelField(position, label.text, "Only reference values can be expandable!");
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PropertyField(position, property, label);

            if (property.objectReferenceValue == null)
            {
                EditorGUI.LabelField(position, label.text, "Object reference is null.");
                EditorGUI.EndProperty();
                return;
            }

            if (property.objectReferenceValue != null && property.objectReferenceValue.Equals(null))
            {
                EditorGUI.LabelField(position, label.text, "Object reference has been destroyed.");
                EditorGUI.EndProperty();
                return;
            }

            isExpanded = EditorGUI.Foldout(position, isExpanded, GUIContent.none, true);

            if (isExpanded)
            {
                if (cachedEditor == null)
                {
                    Editor.CreateCachedEditor(property.objectReferenceValue, null, ref cachedEditor);
                }

                if (cachedEditor != null)
                {
                    cachedEditor.OnInspectorGUI();

                    Rect scale = GUILayoutUtility.GetLastRect();
                    float yOffset = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    bgRect = new Rect(position.x, position.y + yOffset, position.width, scale.yMax - position.y - yOffset);

                    EditorGUI.DrawRect(bgRect, new Color(0.1f, 0.1f, 0.1f, 0.1f));
                }
                else
                {
                    EditorGUI.LabelField(position, label.text, "Failed to create editor for object.");
                }
            }

            EditorGUI.EndProperty();
        }

        private bool IsPartOfArray(SerializedProperty property) => property.propertyPath.Contains("Array.data");
    }
}
#endif