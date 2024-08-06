#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace BBExtensions.Dictionary
{
    [CustomPropertyDrawer(typeof(SerializedDictionary<,>), true)]
    public class SerializedDictionaryPropertyDrawer : PropertyDrawer
    {
        private ReorderableList reorderableList;

        private void InitializeReorderableList(SerializedProperty property)
        {
            if (reorderableList != null)
                return;

            SerializedProperty internalCollection = property.FindPropertyRelative(Names.InternalCollection);
            var dictionary = SCEditorUtility.GetPropertyValue(internalCollection, internalCollection.serializedObject.targetObject);
            IKeyable lookupTable = GetLookupTable(dictionary);

            FieldInfo listField = fieldInfo.FieldType.GetField(Names.InternalCollection, BindingFlags.Instance | BindingFlags.NonPublic);
            System.Type entryType = listField.FieldType.GetGenericArguments()[0];
            FieldInfo keyFieldInfo = entryType.GetField(Names.Key);

            reorderableList = new ReorderableList(property.serializedObject, internalCollection, true, false, true, true)
            {
                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    SerializedProperty element = internalCollection.GetArrayElementAtIndex(index);
                    object key = keyFieldInfo.GetValue(lookupTable.GetKeyAt(index));
                    element.FindPropertyRelative(Names.Duplicate).boolValue = lookupTable.GetOccurences(key).Count > 1;
                    EditorGUI.PropertyField(rect, element, GUIContent.none);
                },
                elementHeightCallback = (int index) =>
                {
                    var element = internalCollection.GetArrayElementAtIndex(index);
                    return EditorGUI.GetPropertyHeight(element);
                },
                multiSelect = true
            };
        }

        private IKeyable GetLookupTable(object dictionary)
        {
            PropertyInfo propInfo = dictionary.GetType().GetProperty(Names.LookupTable, BindingFlags.Instance | BindingFlags.NonPublic);
            return (IKeyable)propInfo.GetValue(dictionary);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty internalCollection = property.FindPropertyRelative(Names.InternalCollection);
            Rect headerRect = new(position) { height = EditorGUIUtility.singleLineHeight };
            property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(headerRect, property.isExpanded, label,
                menuAction: (x) => ShowHeaderContextMenu(x, internalCollection));
            EditorGUI.DrawRect(position, Color.gray * 0.2f);
            EditorGUI.EndFoldoutHeaderGroup();

            if (property.isExpanded)
            {
                InitializeReorderableList(property);
                reorderableList.DoList(position.WithPosition(position.position + new Vector2(0, EditorGUIUtility.singleLineHeight)));
            }
        }

        private void ShowHeaderContextMenu(Rect position, SerializedProperty property)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Reset"), false, () => { property.arraySize = 0; property.serializedObject.ApplyModifiedProperties(); });
            menu.DropDown(position);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
            {
                InitializeReorderableList(property);
                return reorderableList.GetHeight() + EditorGUIUtility.singleLineHeight;
            }
            else
                return EditorGUIUtility.singleLineHeight + 2;
        }
    }
}
#endif
