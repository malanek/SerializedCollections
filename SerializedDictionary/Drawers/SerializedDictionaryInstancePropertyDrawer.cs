#if UNITY_EDITOR
using System.Reflection;
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
            var headerRect = new Rect(position) { height = EditorGUIUtility.singleLineHeight };

            property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(headerRect, property.isExpanded, label,
                menuAction: menuPosition => ShowHeaderContextMenu(menuPosition, internalCollection));

            EditorGUI.DrawRect(position, Color.gray * 0.2f);
            EditorGUI.EndFoldoutHeaderGroup();

            if (property.isExpanded)
            {
                reorderableList.DoList(position.WithPosition(position.position + new Vector2(0, EditorGUIUtility.singleLineHeight)));
            }
        }

        private void ShowHeaderContextMenu(Rect position, SerializedProperty property)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Reset"), false, () => ResetProperty(property));
            menu.DropDown(position);
        }

        private void ResetProperty(SerializedProperty property)
        {
            property.arraySize = 0;
            property.serializedObject.ApplyModifiedProperties();
        }

        public float GetPropertyHeight()
        {
            return property.isExpanded ? reorderableList.GetHeight() + EditorGUIUtility.singleLineHeight : EditorGUIUtility.singleLineHeight + 2;
        }
    }
}
#endif
