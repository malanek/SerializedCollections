#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BBExtensions.AttributeDrawers.Internal
{
    [CustomPropertyDrawer(typeof(ShowIfEnumFlagAttribute))]
    public sealed class ShowIfEnumFlagPropertyDrawer : PropertyDrawer
    {
        private struct CachedData
        {
            public SerializedProperty EnumProperty;
            public int FlagValue;
            public bool IsValid;
        }

        private readonly Dictionary<string, CachedData> propertyCache = new();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!ShouldShow(property))
                return 0f;
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!ShouldShow(property))
                return;
            EditorGUI.PropertyField(position, property, label, true);
        }

        private bool ShouldShow(SerializedProperty property)
        {
            if (!propertyCache.TryGetValue(property.propertyPath, out var cached))
            {
                cached = CacheProperty(property);
                propertyCache[property.propertyPath] = cached;
            }

            if (!cached.IsValid || cached.EnumProperty == null)
                return false;

            return (cached.EnumProperty.intValue & cached.FlagValue) != 0;
        }

        private CachedData CacheProperty(SerializedProperty property)
        {
            var attr = (ShowIfEnumFlagAttribute)attribute;
            var path = property.propertyPath;
            int lastDot = path.LastIndexOf('.');
            if (lastDot < 0)
                return default;

            string parentPath = path[..(lastDot + 1)] + attr.EnumFieldName;
            SerializedProperty enumProp = property.serializedObject.FindProperty(parentPath);

            if (enumProp is not { propertyType: SerializedPropertyType.Enum })
                return new CachedData { EnumProperty = null, FlagValue = attr.FlagValue, IsValid = false };

            return new CachedData
            {
                EnumProperty = enumProp,
                FlagValue = attr.FlagValue,
                IsValid = true
            };
        }
    }
}
#endif
