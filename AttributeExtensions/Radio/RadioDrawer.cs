#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
namespace BBExtensions.AttributeDrawers
{
    [CustomPropertyDrawer(typeof(RadioAttribute))]
    internal sealed class RadioDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Boolean)
            {
                EditorGUI.LabelField(position, label.text, "Radio attribute can only be used with bool fields.");
                return;
            }
            RadioAttribute radioAttr = (RadioAttribute)attribute;
            EditorGUI.BeginChangeCheck();

            bool currentValue = property.boolValue;
            bool newValue = EditorGUI.Toggle(position, label, currentValue);

            if (EditorGUI.EndChangeCheck() && newValue)
            {
                property.boolValue = true;
                ResetOtherGroupValues(property, radioAttr.GroupName);
            }

        }

        private void ResetOtherGroupValues(SerializedProperty property, string groupName)
        {
            SerializedObject targetObject = property.serializedObject;

            SerializedProperty iterator = targetObject.GetIterator();
            iterator.Next(true);

            while (iterator.NextVisible(false))
            {
                SerializedProperty otherProperty = targetObject.FindProperty(iterator.propertyPath);

                if (otherProperty.propertyType == SerializedPropertyType.Boolean &&
                    otherProperty.propertyPath != property.propertyPath &&
                    fieldInfo.DeclaringType.GetField(otherProperty.name).GetCustomAttributes(typeof(RadioAttribute), false) is RadioAttribute[] attrs &&
                    attrs.Length > 0 &&
                    attrs[0].GroupName == groupName)
                {
                    otherProperty.boolValue = false;
                }
            }

            targetObject.ApplyModifiedProperties();
        }
    }
}
#endif