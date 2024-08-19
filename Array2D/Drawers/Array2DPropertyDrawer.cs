#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BBExtensions.Array2D
{
    [CustomPropertyDrawer(typeof(Array2D<>), true)]
    internal class Array2DPropertyDrawer : PropertyDrawer
    {
        private readonly Dictionary<string, Array2DInstancePropertyDrawer> arrayData = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!arrayData.ContainsKey(property.propertyPath))
                arrayData.Add(property.propertyPath, new Array2DInstancePropertyDrawer(property, label));
            arrayData[property.propertyPath].OnGUI(position);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!arrayData.ContainsKey(property.propertyPath))
                arrayData.Add(property.propertyPath, new Array2DInstancePropertyDrawer(property, label));
            return arrayData[property.propertyPath].GetPropertyHeight();
        }
    }
}
#endif