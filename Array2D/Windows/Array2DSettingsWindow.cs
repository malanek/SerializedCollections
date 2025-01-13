#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace BBExtensions.Array2D
{
    internal sealed class Array2DSettingsWindow : EditorWindow
    {
        private static string editedfFieldName;
        private static Array2DSettings array2DSettings;
        private static Action<Array2DSettings> onApplyCallback;
        private static readonly Color HeaderColor = new(0.2f, 0.6f, 1f, 1f);

        public static void ShowWindow(Array2DSettings initialValues, Action<Array2DSettings> callback, string fieldName)
        {
            editedfFieldName = ObjectNames.NicifyVariableName(fieldName);
            array2DSettings = initialValues;
            onApplyCallback = callback;

            var window = CreateInstance<Array2DSettingsWindow>();
            window.titleContent = new GUIContent("Grid Settings");
            window.maxSize = new Vector2(300, 125);
            window.minSize = new Vector2(300, 125);
            window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 300, 125);
            window.ShowModalUtility();
        }

        private void OnGUI()
        {
            DrawHeader();
            EditorGUILayout.Space();
            DrawField();
            EditorGUILayout.Space();
            DrawButtons();
            HandleKeyboardInput();
        }

        private void DrawHeader()
        {
            var headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                normal = { textColor = Color.white }
            };

            EditorGUI.DrawRect(new Rect(0, 0, position.width, 30), HeaderColor);
            GUILayout.Space(5);
            EditorGUILayout.LabelField(editedfFieldName, headerStyle);
        }

        private static void DrawField()
        {
            using (new LabelWidth(88))
            {
                GUILayout.BeginVertical();

                GUILayout.Space(5);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Grid Size", GUILayout.Width(EditorGUIUtility.labelWidth - 4));
                array2DSettings.GridSize = EditorGUILayout.Vector2IntField("", array2DSettings.GridSize, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();

                GUILayout.Space(5);
                
                GUILayout.BeginHorizontal();
                GUILayout.Label("Column Width", GUILayout.Width(EditorGUIUtility.labelWidth - 4));
                array2DSettings.ColumnWidth = EditorGUILayout.IntField("", array2DSettings.ColumnWidth, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }
            
            if (!IsValid())
            {
                EditorGUILayout.HelpBox("Invalid value! Values must be greater than zero.", MessageType.Error);
            }
        }

        private static bool IsValid()
        {
            return array2DSettings.GridSize is { x: > 0, y: > 0 } && array2DSettings.ColumnWidth > 0;
        }

        private void DrawButtons()
        {
            GUI.enabled = array2DSettings.GridSize is { x: > 0, y: > 0 } && array2DSettings.ColumnWidth >= 0;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Cancel", GUILayout.Height(30)))
            {
                Close();
            }

            if (GUILayout.Button("Apply", GUILayout.Height(30)))
            {
                ApplyChanges();
            }

            GUILayout.EndHorizontal();

            GUI.enabled = true;
        }

        private void HandleKeyboardInput()
        {
            if (Event.current.type != EventType.KeyDown || !IsValid())
                return;
            switch (Event.current.keyCode)
            {
                case KeyCode.Return or KeyCode.KeypadEnter:
                    ApplyChanges();
                    break;
                case KeyCode.Escape:
                    Close();
                    break;
            }
        }

        private void ApplyChanges()
        {
            onApplyCallback?.Invoke(array2DSettings);
            Close();
        }
    }
}
#endif