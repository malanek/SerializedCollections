#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace BBExtensions.Dictionary
{
    [CustomPropertyDrawer(typeof(SerializedKeyValuePair<,>))]
    public class SerializedKeyValuePairPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty keyProperty = property.FindPropertyRelative(Names.Key);
            SerializedProperty valueProperty = property.FindPropertyRelative(Names.Value);
            GUI.backgroundColor = property.FindPropertyRelative(Names.Duplicate).boolValue ? Color.red : Color.white;
            // Define width constants
            float padding = 4f; // Padding between key and value
            if ((valueProperty.isArray || valueProperty.hasVisibleChildren) && valueProperty.propertyType != SerializedPropertyType.String)
                padding += 16f;
            if ((keyProperty.isArray || keyProperty.hasVisibleChildren) && keyProperty.propertyType != SerializedPropertyType.String)
                padding += 16f;

            // Calculate the available width for Key and Value fields
            float availableWidth = position.width - padding;

            float keyWidth = availableWidth * 0.2f;
            if (keyWidth < 70f)
                keyWidth = 70f;
            float valueWidth = availableWidth - keyWidth - padding;

            // Define rects for key and value fields
            Rect keyRect = new Rect(position.x, position.y, keyWidth, position.height);
            Rect valueRect = new Rect(position.x + keyWidth + padding, position.y, valueWidth, position.height);

            // Draw the property fields on top of the backgrounds
            if (keyProperty != null && valueProperty != null)
            {
                EditorGUI.PropertyField(keyRect, keyProperty, GUIContent.none, true);
                GUI.backgroundColor = Color.white;
                EditorGUI.PropertyField(valueRect.Cut(0,4f,8f,0), valueProperty, GUIContent.none, true);
            }
            else
            {
                Debug.LogWarning("Unable to find 'Key' or 'Value' property.");
            }
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty keyProperty = property.FindPropertyRelative(Names.Key);
            SerializedProperty valueProperty = property.FindPropertyRelative(Names.Value);
            var result = Mathf.Max(EditorGUI.GetPropertyHeight(keyProperty, true), EditorGUI.GetPropertyHeight(valueProperty, true));
            return result + 2;
        }
    }
}
#endif
