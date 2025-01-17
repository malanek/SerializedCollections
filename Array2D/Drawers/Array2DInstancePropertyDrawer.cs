#if UNITY_EDITOR
using MExtensions.Utility;
using UnityEditor;
using UnityEngine;

namespace BBExtensions.Array2D
{
    internal sealed class Array2DInstancePropertyDrawer
    {
        private const float CellPadding = 2f;
        private readonly float singleLine = EditorGUIUtility.singleLineHeight;
        private readonly Color background = new(0.2f, 0.2f, 0.2f);
        private const float scrollBarHeight = 15f;
        
        private readonly SerializedProperty property;
        private readonly SerializedProperty rowsProperty;
        private readonly SerializedProperty columnWidthProperty;
        private readonly SerializedProperty renderSizeProperty;
        private readonly GUIContent label;

        public Vector2Int ArraySize { get; private set; }
        public SerializedProperty[,] SerializedProperties { get; private set; }

        public int ColumnWidth
        {
            get => columnWidthProperty.intValue;
            set => columnWidthProperty.intValue = value;
        }

        public Vector2Int RenderSize
        {
            get => renderSizeProperty.vector2IntValue;
            set => renderSizeProperty.vector2IntValue = value;
        }
        
        private Vector2 scrollPosition;

        public Array2DInstancePropertyDrawer(SerializedProperty property, GUIContent label)
        {
            this.property = property;
            this.label = label;
            rowsProperty = property.FindPropertyRelative(Names.Rows);
            columnWidthProperty = property.FindPropertyRelative(Names.Width);
            renderSizeProperty = property.FindPropertyRelative(Names.RenderSize);
            BuildArray();
        }

        private void BuildArray()
        {
            ArraySize = GetArraySize();
            SerializedProperties = new SerializedProperty[ArraySize.x, ArraySize.y];
            if (rowsProperty == null || rowsProperty.arraySize <= 0) return;

            for (int y = 0; y < ArraySize.y; y++)
            {
                var elementsProperty = rowsProperty.GetArrayElementAtIndex(y).FindPropertyRelative(Names.Cells);
                if (elementsProperty is not { arraySize: > 0 })
                    continue;

                for (int x = 0; x < ArraySize.x; x++)
                {
                    SerializedProperties[x, y] = elementsProperty.GetArrayElementAtIndex(x).FindPropertyRelative(Names.CellValue);
                }
            }
        }

        private Vector2Int GetArraySize()
        {
            if (rowsProperty is not { arraySize: > 0 })
                return Vector2Int.zero;

            int xSize = rowsProperty.GetArrayElementAtIndex(0).FindPropertyRelative(Names.Cells).arraySize;
            int ySize = rowsProperty.arraySize;
            return new Vector2Int(xSize, ySize);
        }

        public void OnGUI(Rect position)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);

            Rect foldoutRect = new(position.x, position.y, position.width, singleLine);

            using (new AlwaysActive(GUI.enabled))
            {
                property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(foldoutRect, property.isExpanded, label, menuAction: ShowHeaderContextMenu);
                EditorGUI.EndFoldoutHeaderGroup();
            }

            if (property.isExpanded)
            {
                DrawArrayElements(position);
            }

            EditorGUI.EndProperty();
        }

        private void DrawArrayElements(Rect position)
        {
            float contentWidth = ArraySize.x * (ColumnWidth + CellPadding) - CellPadding;

            float adjustedHeight = position.height - singleLine;
            using (new AlwaysActive(GUI.enabled))
            {
                scrollPosition = GUI.BeginScrollView(
                new Rect(position.x, position.y + singleLine, position.width, adjustedHeight),
                scrollPosition,
                new Rect(0, 0, contentWidth, GetTotalHeight()));
            }

            Rect contentRect = new(0, 0, contentWidth, singleLine);

            for (int y = 0; y < ArraySize.y; y++)
            {
                float rowHeight = GetRowHeight(y);
                Rect rowRect = new(contentRect.x, contentRect.y, contentRect.width, rowHeight);

                for (int x = 0; x < ArraySize.x; x++)
                {
                    DrawElement(rowRect, x, y, rowHeight);
                }

                contentRect.y += rowHeight + CellPadding;
            }

            GUI.EndScrollView();
        }

        private void DrawElement(Rect rowRect, int x, int y, float rowHeight)
        {
            SerializedProperty elementProperty = SerializedProperties[x, y];
            Rect cellRect = new(rowRect.x + x * (ColumnWidth + CellPadding), rowRect.y, ColumnWidth, rowHeight);
            EditorGUI.DrawRect(cellRect, background);

            if (elementProperty.hasChildren && elementProperty.propertyType != SerializedPropertyType.String)
                elementProperty.isExpanded = EditorGUI.Foldout(cellRect.WithHeight(singleLine).CutLeft(16f), elementProperty.isExpanded, GUIContent.none);

            using (new LabelWidth(cellRect.width * 0.4f))
            {
                GUI.BeginGroup(cellRect);
                EditorGUI.PropertyField(new Rect(Vector2.zero, cellRect.size), elementProperty, GUIContent.none, true);
                GUI.EndGroup();
            }
        }

        private float GetRowHeight(int y)
        {
            float maxHeight = 0f;
            for (int x = 0; x < ArraySize.x; x++)
            {
                maxHeight = Mathf.Max(maxHeight, EditorGUI.GetPropertyHeight(SerializedProperties[x, y], true));
            }
            return maxHeight;
        }

        private float GetTotalHeight()
        {
            float totalHeight = 0f;
            for (int y = 0; y < ArraySize.y; y++)
            {
                totalHeight += GetRowHeight(y) + CellPadding;
            }
            return totalHeight;
        }

        private void ShowHeaderContextMenu(Rect position)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Reset"), false, OnReset);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Change Grid Size or Column Width"), false, OnSettingsChange);
            menu.DropDown(position);
        }

        private void OnSettingsChange()
        {
            var gridSizeProperty = property.FindPropertyRelative(Names.GridSize);
            Array2DSettingsWindow.ShowWindow(new(gridSizeProperty.vector2IntValue, ColumnWidth), settings =>
            {
                if (settings.ColumnWidth != ColumnWidth)
                {
                    ColumnWidth = settings.ColumnWidth;
                    columnWidthProperty.serializedObject.ApplyModifiedProperties();
                }

                if (settings.GridSize != gridSizeProperty.vector2IntValue)
                {
                    rowsProperty.arraySize = settings.GridSize.y;
                    gridSizeProperty.vector2IntValue = settings.GridSize;

                    for (int y = 0; y < settings.GridSize.y; y++)
                    {
                        var elementsProperty = rowsProperty.GetArrayElementAtIndex(y).FindPropertyRelative(Names.Cells);
                        if (elementsProperty == null) continue;

                        elementsProperty.arraySize = settings.GridSize.x;
                    }

                    property.serializedObject.ApplyModifiedProperties();
                    BuildArray();
                }
            }, property.name);
        }

        private void OnReset()
        {
            for (int y = 0; y < ArraySize.y; y++)
            {
                var elementsProperty = rowsProperty.GetArrayElementAtIndex(y).FindPropertyRelative(Names.Cells);
                if (elementsProperty == null) continue;

                int size = elementsProperty.arraySize;
                elementsProperty.arraySize = 0;
                elementsProperty.arraySize = size;
            }

            property.serializedObject.ApplyModifiedProperties();
            BuildArray();
        }

        public float GetPropertyHeight()
        {
            if (!property.isExpanded) return singleLine;
            return singleLine + Mathf.Min(GetTotalHeight(), EditorGUIUtility.currentViewWidth) + scrollBarHeight;
        }
    }
}
#endif
