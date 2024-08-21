#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BBExtensions.AttributeDrawers
{
    [CustomPropertyDrawer(typeof(ExpandRefAttribute))]
    internal class ExpandableDrawer : PropertyDrawer
    {
        private readonly Dictionary<string, ExpandableInstanceDrawer> drawers = new();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!drawers.ContainsKey(property.propertyPath))
                drawers.Add(property.propertyPath, new ExpandableInstanceDrawer(property, label));
            drawers[property.propertyPath].OnGUI(position);
        }
    }
}
#endif
