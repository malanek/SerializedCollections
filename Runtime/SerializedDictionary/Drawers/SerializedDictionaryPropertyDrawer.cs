#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BBExtensions.Dictionary
{
    [CustomPropertyDrawer(typeof(SerializedDictionary<,>), true)]
    public class SerializedDictionaryPropertyDrawer : PropertyDrawer
    {
        private readonly Dictionary<string, SerializedDictionaryInstancePropertyDrawer> arrayData = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!arrayData.ContainsKey(property.propertyPath))
                arrayData.Add(property.propertyPath, new SerializedDictionaryInstancePropertyDrawer(fieldInfo, property, label));
            arrayData[property.propertyPath].OnGUI(position);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!arrayData.ContainsKey(property.propertyPath))
                arrayData.Add(property.propertyPath, new SerializedDictionaryInstancePropertyDrawer(fieldInfo, property, label));
            return arrayData[property.propertyPath].GetPropertyHeight();
        }
    }
}
#endif
