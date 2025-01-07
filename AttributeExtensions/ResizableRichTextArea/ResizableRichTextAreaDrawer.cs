#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace BBExtensions.AttributeDrawers
{
    [CustomPropertyDrawer(typeof(ResizableRichTextAreaAttribute))]
    internal sealed class ResizableRichTextAreaDrawer : PropertyDrawer
    {
        private bool showRichText = true;
        private GUIStyle textAreaStyle;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.LabelField(position, label.text, "Use [ResizableRichTextArea] with strings only.");
                return;
            }

            if (textAreaStyle == null)
            {
                textAreaStyle = new GUIStyle(EditorStyles.textArea)
                {
                    wordWrap = true
                };
            }

            position.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(position, label);

            Rect buttonRect = new Rect(position.xMax - 60, position.y, 60, EditorGUIUtility.singleLineHeight);
            bool newShowRichText = GUI.Toggle(buttonRect, showRichText, "Rich", "Button");
            
            if (newShowRichText != showRichText)
            {
                showRichText = newShowRichText;
                textAreaStyle.richText = showRichText;
            }

            position.y += EditorGUIUtility.singleLineHeight + 2;

            float textHeight = textAreaStyle.CalcHeight(new GUIContent(property.stringValue), position.width);
            position.height = Mathf.Max(EditorGUIUtility.singleLineHeight * 3, textHeight);

            property.stringValue = EditorGUI.TextArea(position, property.stringValue, textAreaStyle);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
                return base.GetPropertyHeight(property, label);

            if (textAreaStyle == null)
            {
                textAreaStyle = new GUIStyle(EditorStyles.textArea)
                {
                    wordWrap = true
                };
            }
            textAreaStyle.richText = showRichText;

            float textHeight = textAreaStyle.CalcHeight(new GUIContent(property.stringValue), EditorGUIUtility.currentViewWidth - 50);
            return Mathf.Max(EditorGUIUtility.singleLineHeight * 3, textHeight) + EditorGUIUtility.singleLineHeight + 4;
        }
    }
}
#endif
