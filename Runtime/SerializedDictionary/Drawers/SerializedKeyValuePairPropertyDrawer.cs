#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace BBExtensions.Dictionary
{
    [CustomPropertyDrawer(typeof(SerializedKeyValuePair<,>))]
    public class SerializedKeyValuePairPropertyDrawer : PropertyDrawer
    {
        private const float MinKeyWidth = 50f;
        private const float MinValueWidth = 100f;
        private const float Padding = 4f;
        private const float ComplexPadding = 16f;
        private const float SeparatorWidth = 2f;
        private const string prefsKey = "SerializedKeyValuePairPropertyDrawerKeyColumnPercent";
        private static readonly Color separatorColor = new(0.4f, 0.4f, 0.4f, 1f);

        private static bool isResizing;
        private static float? cachedKeyColumnPercent;

        private static float keyColumnPercent
        {
            get
            {
                cachedKeyColumnPercent ??= Mathf.Clamp01(EditorPrefs.GetFloat(prefsKey, 0.25f));
                return cachedKeyColumnPercent.Value;
            }
            set
            {
                cachedKeyColumnPercent = Mathf.Clamp01(value);
                EditorPrefs.SetFloat(prefsKey, cachedKeyColumnPercent.Value);
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using var _ = new EditorGUI.PropertyScope(position, label, property);

            var keyProperty = property.FindPropertyRelative(Names.Key);
            var valueProperty = property.FindPropertyRelative(Names.Value);
            bool isDuplicate = property.FindPropertyRelative(Names.Duplicate).boolValue;

            GUI.backgroundColor = isDuplicate ? Color.red : Color.white;

            if (keyProperty == null || valueProperty == null)
            {
                GUI.backgroundColor = Color.white;
                Debug.LogWarningFormat("Unable to find 'Key' or 'Value' property in {0}.", property.name);
                return;
            }

            float adjustedPadding = CalculatePadding(keyProperty, valueProperty);
            float availableWidth = position.width - adjustedPadding;

            float keyPixelWidth = availableWidth * keyColumnPercent;
            float valuePixelWidth = availableWidth - keyPixelWidth - SeparatorWidth;

            var keyRect = new Rect(position.x, position.y, keyPixelWidth, position.height);
            var separatorRect = new Rect(keyRect.xMax + Padding, position.y, SeparatorWidth, position.height);
            var valueRect = new Rect(separatorRect.xMax + adjustedPadding, position.y, valuePixelWidth, position.height);

            EditorGUI.PropertyField(keyRect, keyProperty, GUIContent.none, true);

            EditorGUIUtility.AddCursorRect(separatorRect, MouseCursor.ResizeHorizontal);
            EditorGUI.DrawRect(separatorRect, separatorColor);

            HandleColumnResize(separatorRect, position, adjustedPadding);

            GUI.backgroundColor = Color.white;
            using (new LabelWidth(Mathf.Max(1f, valueRect.width * 0.25f)))
            {
                EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var keyProperty = property.FindPropertyRelative(Names.Key);
            var valueProperty = property.FindPropertyRelative(Names.Value);

            return Mathf.Max(
                EditorGUI.GetPropertyHeight(keyProperty, true),
                EditorGUI.GetPropertyHeight(valueProperty, true)
            ) + 2f;
        }

        private static float CalculatePadding(SerializedProperty keyProperty, SerializedProperty valueProperty)
        {
            float padding = Padding;
            if (IsComplexProperty(keyProperty) || IsComplexProperty(valueProperty))
            {
                padding += ComplexPadding;
            }
            return padding;
        }

        private static bool IsComplexProperty(SerializedProperty property)
        {
            return (property.isArray || property.hasVisibleChildren)
                   && property.propertyType != SerializedPropertyType.String;
        }

        private static void HandleColumnResize(Rect separatorRect, Rect fullRect, float adjustedPadding)
        {
            Event e = Event.current;

            switch (e.type)
            {
                case EventType.MouseDown when separatorRect.Contains(e.mousePosition):
                    isResizing = true;
                    e.Use();
                    break;

                case EventType.MouseUp:
                    isResizing = false;
                    break;

                case EventType.MouseDrag when isResizing:
                {
                    float availableWidth = fullRect.width - adjustedPadding;
                    float newKeyWidth = e.mousePosition.x - fullRect.x;
                    newKeyWidth = Mathf.Clamp(newKeyWidth, MinKeyWidth, availableWidth - MinValueWidth);
                    keyColumnPercent = newKeyWidth / availableWidth;

                    e.Use();
                    break;
                }
            }
        }
    }
}
#endif
