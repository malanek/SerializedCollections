#if UNITY_EDITOR
using System.Reflection;
using MBBExtensions.Utility;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace BBExtensions.Dictionary
{
    internal sealed class SerializedDictionaryInstancePropertyDrawer
    {
        private readonly FieldInfo fieldInfo;
        private readonly SerializedProperty property;
        private readonly GUIContent label;
        private ReorderableList reorderableList;

        internal SerializedDictionaryInstancePropertyDrawer(FieldInfo fieldInfo, SerializedProperty property, GUIContent label)
        {
            this.fieldInfo = fieldInfo;
            this.property = property;
            this.label = label;
            InitializeReorderableList();
        }

        private void InitializeReorderableList()
        {
            if (reorderableList != null)
                return;

            var internalCollection = property.FindPropertyRelative(Names.InternalCollection);
            var dictionary = SCEditorUtility.GetPropertyValue(internalCollection, internalCollection.serializedObject.targetObject);
            var lookupTable = GetLookupTable(dictionary);

            var listField = fieldInfo.FieldType.GetField(Names.InternalCollection, BindingFlags.Instance | BindingFlags.NonPublic);
            var entryType = listField.FieldType.GetGenericArguments()[0];
            var keyFieldInfo = entryType.GetField(Names.Key);

            reorderableList = new ReorderableList(property.serializedObject, internalCollection, true, false, true, true)
            {
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    var element = internalCollection.GetArrayElementAtIndex(index);
                    var key = keyFieldInfo.GetValue(lookupTable.GetKeyAt(index));
                    element.FindPropertyRelative(Names.Duplicate).boolValue = lookupTable.GetOccurences(key).Count > 1;
                    EditorGUI.PropertyField(rect, element, GUIContent.none);
                },
                elementHeightCallback = index =>
                {
                    var element = internalCollection.GetArrayElementAtIndex(index);
                    return EditorGUI.GetPropertyHeight(element);
                },
                multiSelect = true
            };
        }

        private IKeyable GetLookupTable(object dictionary)
        {
            var propInfo = dictionary.GetType().GetProperty(Names.LookupTable, BindingFlags.Instance | BindingFlags.NonPublic);
            return (IKeyable)propInfo.GetValue(dictionary);
        }

        public void OnGUI(Rect position)
        {
            var internalCollection = property.FindPropertyRelative(Names.InternalCollection);
            var elementCount = internalCollection.arraySize;

            var headerRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var labelRect = new Rect(headerRect.x, headerRect.y, headerRect.width - 50, headerRect.height);
            var countRect = new Rect(headerRect.x + headerRect.width - 50, headerRect.y, 50, headerRect.height);

            using (new AlwaysActive(GUI.enabled))
            {
                property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(labelRect, property.isExpanded, label);
                EditorGUI.EndFoldoutHeaderGroup();
            }

            int newElementCount = EditorGUI.DelayedIntField(countRect, elementCount);
            if (newElementCount != elementCount)
            {
                AdjustElementCount(internalCollection, newElementCount);
            }

            if (property.isExpanded)
            {
                var listRect = position;
                listRect.y += EditorGUIUtility.singleLineHeight + 2;
                listRect.height = reorderableList.GetHeight();
                reorderableList.DoList(listRect);
            }
        }

        private void AdjustElementCount(SerializedProperty collectionProperty, int newCount)
        {
            collectionProperty.arraySize = newCount;
            collectionProperty.serializedObject.ApplyModifiedProperties();
        }

        public float GetPropertyHeight()
        {
            return property.isExpanded ? reorderableList.GetHeight() + EditorGUIUtility.singleLineHeight + 2 : EditorGUIUtility.singleLineHeight + 2;
        }
    }
}
#endif
