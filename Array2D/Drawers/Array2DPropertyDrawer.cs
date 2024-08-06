#if UNITY_EDITOR
using BBExtensions.Array2D;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Array2D<>), true)]
public class Array2DPropertyDrawer : PropertyDrawer
{
    private const float CellPadding = 2f;
    private readonly float singleLine = EditorGUIUtility.singleLineHeight;
    private readonly Color background = new(0.2f, 0.2f, 0.2f);

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, GUIContent.none, property);

        var foldoutRect = new Rect(position.x, position.y, position.width, singleLine);
        property.isExpanded = EditorGUI.BeginFoldoutHeaderGroup(foldoutRect, property.isExpanded, label,
                menuAction: (x) => ShowHeaderContextMenu(x, property));
        EditorGUI.EndFoldoutHeaderGroup();
        float columnWidth = property.FindPropertyRelative(Names.Width).floatValue;
        if (property.isExpanded)
        {
            var rowsProperty = property.FindPropertyRelative(Names.Rows);

            if (rowsProperty != null && rowsProperty.arraySize > 0)
            {
                float totalHeight = 0f;

                for (int x = 0; x < rowsProperty.arraySize; x++)
                {
                    var rowProperty = rowsProperty.GetArrayElementAtIndex(x);
                    var elementsProperty = rowProperty.FindPropertyRelative(Names.Elements);

                    if (elementsProperty != null && elementsProperty.arraySize > 0)
                    {
                        float cellHeight = GetRowHeight(elementsProperty);
                        totalHeight += cellHeight + CellPadding;
                    }
                }

                var contentRect = new Rect(position.x, position.y + singleLine, position.width, totalHeight);

                for (int x = 0; x < rowsProperty.arraySize; x++)
                {
                    var rowProperty = rowsProperty.GetArrayElementAtIndex(x);
                    var elementsProperty = rowProperty.FindPropertyRelative(Names.Elements);

                    if (elementsProperty != null && elementsProperty.arraySize > 0)
                    {
                        float cellHeight = GetRowHeight(elementsProperty);

                        var rowRect = new Rect(contentRect.x, contentRect.y, contentRect.width, cellHeight);

                        for (int y = 0; y < elementsProperty.arraySize; y++)
                        {
                            SerializedProperty elementProperty = elementsProperty.GetArrayElementAtIndex(y);
                            Rect cellRect = new(rowRect.x + y * (columnWidth + CellPadding), rowRect.y, columnWidth, cellHeight);
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
        }

        EditorGUI.EndProperty();
    }

    private float GetRowHeight(SerializedProperty elementsProperty)
    {
        float maxHeight = 0;
        for (int i = 0; i < elementsProperty.arraySize; i++)
        {
            var elementProperty = elementsProperty.GetArrayElementAtIndex(i);
            float height = EditorGUI.GetPropertyHeight(elementProperty, true);
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
            widthProperty.floatValue = x;
            widthProperty.serializedObject.ApplyModifiedProperties();
        }, "Width");
    }

    private void OnReset(SerializedProperty property)
    {
        SerializedProperty rowsProperty = property.FindPropertyRelative(Names.Rows);
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
    }

    private void OnChangeGridSize(SerializedProperty property)
    {
        EditorWindowVector2IntField.ShowWindow("Change Grid Size", property.FindPropertyRelative(Names.GridSize).vector2IntValue, (size) =>
        {
            SerializedProperty rowsProperty = property.FindPropertyRelative(Names.Rows);
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
        }, "Grid Size");
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return singleLine;

        var rowsProperty = property.FindPropertyRelative(Names.Rows);
        if (rowsProperty == null || rowsProperty.arraySize == 0)
            return base.GetPropertyHeight(property, label);

        float totalHeight = 0f;

        for (int i = 0; i < rowsProperty.arraySize; i++)
        {
            var rowProperty = rowsProperty.GetArrayElementAtIndex(i);
            var elementsProperty = rowProperty.FindPropertyRelative(Names.Elements);

            if (elementsProperty != null && elementsProperty.arraySize > 0)
            {
                totalHeight += GetRowHeight(elementsProperty) + CellPadding;
            }
        }
        return totalHeight + singleLine;
    }
}
#endif
