#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace BBExtensions.Dictionary
{
    [CustomPropertyDrawer(typeof(SerializedKeyValuePair<,>))]
    public class SerializedKeyValuePairPropertyDrawer : PropertyDrawer
    {
        private const float MinKeyWidth = 70f;
        private const float Padding = 4f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var keyProperty = property.FindPropertyRelative(Names.Key);
            var valueProperty = property.FindPropertyRelative(Names.Value);
            bool isDuplicate = property.FindPropertyRelative(Names.Duplicate).boolValue;

            GUI.backgroundColor = isDuplicate ? Color.red : Color.white;

            float adjustedPadding = CalculatePadding(keyProperty, valueProperty);
            float availableWidth = position.width - adjustedPadding;

            float keyWidth = Mathf.Max(availableWidth * 0.2f, MinKeyWidth);
            float valueWidth = availableWidth - keyWidth - adjustedPadding;

            var keyRect = new Rect(position.x, position.y, keyWidth, position.height);
            var valueRect = new Rect(position.x + keyWidth + adjustedPadding, position.y, valueWidth, position.height);

            if (keyProperty != null && valueProperty != null)
            {
                EditorGUI.PropertyField(keyRect, keyProperty, GUIContent.none, true);
                GUI.backgroundColor = Color.white;
                EditorGUI.PropertyField(valueRect.Cut(0, 4f, 8f, 0), valueProperty, GUIContent.none, true);
            }
            else
            {
                Debug.LogWarning("Unable to find 'Key' or 'Value' property.");
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var keyProperty = property.FindPropertyRelative(Names.Key);
            var valueProperty = property.FindPropertyRelative(Names.Value);
            return Mathf.Max(EditorGUI.GetPropertyHeight(keyProperty, true), EditorGUI.GetPropertyHeight(valueProperty, true)) + 2f;
        }

        private float CalculatePadding(SerializedProperty keyProperty, SerializedProperty valueProperty)
        {
            float padding = Padding;
            if (IsComplexProperty(valueProperty))
                padding += 16f;
            if (IsComplexProperty(keyProperty))
                padding += 16f;
            return padding;
        }

        private bool IsComplexProperty(SerializedProperty property)
        {
            return (property.isArray || property.hasVisibleChildren) && property.propertyType != SerializedPropertyType.String;
        }
    }
}
#endif
