#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;

namespace BBExtensions.SerializedTuple
{
    [CustomPropertyDrawer(typeof(TupleNameAttribute))]
    internal sealed class TupleNameDrawer : PropertyDrawer
    {
        private static readonly string[] fieldNames =
        {
            "Item1",
            "Item2",
            "Item3",
            "Item4",
            "Item5",
            "Item6",
            "Item7",
            "Item8",
        };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            TupleNameAttribute tupleNameAttribute = (TupleNameAttribute)attribute;
            Type fieldType = fieldInfo.FieldType;
            TupleNameAttribute.ValidateUsage(fieldType);

            EditorGUI.BeginProperty(position, label, property);

            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                property.isExpanded, label, true);

            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            float lineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            Rect fieldRect = new Rect(position.x, position.y + lineHeight, position.width,
                EditorGUIUtility.singleLineHeight);
            int offset = 0;

            for (int i = 0; i < fieldNames.Length; i++)
            {
                if (!property.TryFindPropertyRelative(fieldNames[i], out SerializedProperty p))
                    break;
                DrawField(fieldRect, lineHeight, ref offset, p, tupleNameAttribute.FieldNames[i]);
            }
            

            EditorGUI.EndProperty();
        }

        private static void DrawField(Rect fieldRect, float lineHeight, ref int offset, SerializedProperty fieldToDraw,
            string label)
        {
            fieldRect.y += offset * lineHeight;
            EditorGUI.PropertyField(fieldRect, fieldToDraw, new GUIContent($"{offset + 1}. {label}"), true);
            offset++;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            int fieldCount = 2;
            for (int i = 2; i < fieldNames.Length; i++)
            {
                if (property.FindPropertyRelative(fieldNames[i]) != null)
                    fieldCount++;
                else
                    break;
            }

            return EditorGUIUtility.singleLineHeight +
                   fieldCount * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
        }
    }
}
#endif