#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace BBExtensions.Array2D
{
    internal class Array2DInstancePropertyDrawer
    {
        private const float CellPadding = 2f;
        private readonly float singleLine = EditorGUIUtility.singleLineHeight;
        private readonly Color background = new(0.2f, 0.2f, 0.2f);
        private readonly SerializedProperty property;
        private readonly SerializedProperty rowsProperty;
        private readonly SerializedProperty columnWidthProperty;
        private readonly GUIContent label;

        public Vector2Int ArraySize { get; private set; }

        public SerializedProperty[,] SerializedProperties { get; private set; }

        public float ColumnWidth
        {
            get => columnWidthProperty.floatValue;
            set => columnWidthProperty.floatValue = value;
        }

        public bool CheckForReferenceChanges(SerializedProperty property)
        {
            if (this.property == null)
                return true;
            if (property == null)
                return true;
            if (this.property != property)
                return true;
            return false;
        }

        public Array2DInstancePropertyDrawer(SerializedProperty property, GUIContent label)
        {
            this.property = property;
            this.label = label;
            rowsProperty = property.FindPropertyRelative(Names.Rows);
            columnWidthProperty = property.FindPropertyRelative(Names.Width);
            BuildArray();
        }

        public void BuildArray()
        {
            ArraySize = GetArraySize();
            SerializedProperties = new SerializedProperty[ArraySize.x, ArraySize.y];
            if (rowsProperty != null && rowsProperty.arraySize > 0)
            {
                for (int y = 0; y < rowsProperty.arraySize; y++)
                {
                    var rowProperty = rowsProperty.GetArrayElementAtIndex(y);
                    var elementsProperty = rowProperty.FindPropertyRelative(Names.Elements);

                    if (elementsProperty != null && elementsProperty.arraySize > 0)
                    {
                        for (int x = 0; x < elementsProperty.arraySize; x++)
                        {
                            SerializedProperties[x, y] = elementsProperty.GetArrayElementAtIndex(x);
                        }
                    }
                }
            }
        }

        private Vector2Int GetArraySize()
        {
            int xSize = 0;
            int ySize = rowsProperty.arraySize;
            if (rowsProperty != null && rowsProperty.arraySize > 0)
            {
                var rowProperty = rowsProperty.GetArrayElementAtIndex(0);
                var elementsProperty = rowProperty.FindPropertyRelative(Names.Elements);
                xSize = elementsProperty.arraySize;
            }
            return new Vector2Int(xSize, ySize);
        }

        public void OnGUI(Rect position)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);

            Rect foldoutRect = new(position.x, position.y, position.width, singleLine);
            property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(foldoutRect, property.isExpanded, label,
                    menuAction: (x) => ShowHeaderContextMenu(x, property));
            EditorGUI.EndFoldoutHeaderGroup();
            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }
            if (ArraySize.y > 0)
            {
                float totalHeight = 0f;
                for (int y = 0; y < ArraySize.y; y++)
                    if (ArraySize.x > 0)
                        totalHeight += GetRowHeight(y) + CellPadding;

                Rect contentRect = new(position.x, position.y + singleLine, position.width, totalHeight);

                for (int y = 0; y < ArraySize.y; y++)
                {
                    if (ArraySize.x > 0)
                    {
                        float cellHeight = GetRowHeight(y);

                        Rect rowRect = new(contentRect.x, contentRect.y, contentRect.width, cellHeight);

                        for (int x = 0; x < ArraySize.x; x++)
                        {
                            SerializedProperty elementProperty = SerializedProperties[x, y];
                            Rect cellRect = new(rowRect.x + x * (ColumnWidth + CellPadding), rowRect.y, ColumnWidth, cellHeight);
                            EditorGUI.DrawRect(cellRect, background);
                            if (elementProperty.hasChildren && elementProperty.propertyType != SerializedPropertyType.String)
                                elementProperty.isExpanded = EditorGUI.Foldout(cellRect.WithHeight(singleLine).CutLeft(16f), elementProperty.isExpanded, GUIContent.none);
                            using (new LabelWidth(cellRect.width * 0.4f))
                            {
                                GUI.BeginGroup(cellRect);
                                Rect propertyRect = new Rect(Vector2.zero, new Vector2(cellRect.width, cellRect.height));
                                EditorGUI.PropertyField(propertyRect, elementProperty, GUIContent.none, true);
                                GUI.EndGroup();
                            }
                        }
                        contentRect.y += cellHeight + CellPadding;
                    }
                }
            }
            EditorGUI.EndProperty();
        }

        private float GetRowHeight(int y)
        {
            float maxHeight = 0;
            for (int x = 0; x < ArraySize.x; x++)
            {
                float height = EditorGUI.GetPropertyHeight(SerializedProperties[x, y], true);
                maxHeight = Mathf.Max(maxHeight, height);
            }
            return maxHeight;
        }

        private void ShowHeaderContextMenu(Rect position, SerializedProperty property)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Reset"), false, () => OnReset(property));
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Change Column Width"), false, () => OnWidthChange(property.FindPropertyRelative(Names.Width)));
            menu.AddItem(new GUIContent("Change Grid Size"), false, () => OnChangeGridSize(property));
            menu.DropDown(position);
        }

        private void OnWidthChange(SerializedProperty widthProperty)
        {
            EditorWindowFloatField.ShowWindow("Change column width", widthProperty.floatValue, x =>
            {
                ColumnWidth = x;
                widthProperty.floatValue = x;
                widthProperty.serializedObject.ApplyModifiedProperties();
            }, "Width");
        }

        private void OnReset(SerializedProperty property)
        {
            for (int y = 0; y < rowsProperty.arraySize; y++)
            {
                SerializedProperty rowProperty = rowsProperty.GetArrayElementAtIndex(y);
                SerializedProperty elementsProperty = rowProperty.FindPropertyRelative(Names.Elements);

                if (elementsProperty != null)
                {
                    int size = elementsProperty.arraySize;
                    elementsProperty.arraySize = 0;
                    elementsProperty.arraySize = size;
                }
            }
            property.serializedObject.ApplyModifiedProperties();
            BuildArray();
        }

        private void OnChangeGridSize(SerializedProperty property)
        {
            EditorWindowVector2IntField.ShowWindow("Change Grid Size", property.FindPropertyRelative(Names.GridSize).vector2IntValue, (size) =>
            {
                rowsProperty.arraySize = size.y;
                property.FindPropertyRelative(Names.GridSize).vector2IntValue = size;
                for (int y = 0; y < size.y; y++)
                {
                    SerializedProperty rowProperty = rowsProperty.GetArrayElementAtIndex(y);
                    SerializedProperty elementsProperty = rowProperty.FindPropertyRelative(Names.Elements);

                    if (elementsProperty != null)
                    {
                        elementsProperty.arraySize = size.x;
                    }
                }

                property.serializedObject.ApplyModifiedProperties();
                BuildArray();
            }, "Grid Size");
        }

        public float GetPropertyHeight()
        {
            if (!property.isExpanded)
                return singleLine;
            if (ArraySize.y == 0)
                return singleLine;

            float totalHeight = 0f;

            for (int y = 0; y < ArraySize.y; y++)
                if (ArraySize.x > 0)
                    totalHeight += GetRowHeight(y) + CellPadding;
            return totalHeight + singleLine;
        }
    }
}
#endif