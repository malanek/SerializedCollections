#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;

public class EditorWindowFloatField : EditorWindow
{
    private const string controlName = "Vector2IntField";
    private static bool controlFocused;

    private static float newFieldValue;
    private static string fieldLabel;
    private static Action<float> onApply;


    public static void ShowWindow(string title, float fieldValue, Action<float> onApplyCallback, string label)
    {
        newFieldValue = fieldValue;
        onApply = onApplyCallback;
        fieldLabel = label;
        controlFocused = false;
        var window = GetWindow<EditorWindowFloatField>();
        window.titleContent = new GUIContent(title);
        window.maxSize = new Vector2(250, 100);

        window.ShowPopup();
    }

    void OnGUI()
    {
        GUI.SetNextControlName(controlName);
        newFieldValue = EditorGUILayout.FloatField(fieldLabel, newFieldValue);

        if (!controlFocused)
        {
            EditorGUI.FocusTextInControl(controlName);
            controlFocused = true;
        }

        var wrongFieldValue = newFieldValue <= 0;

        if (wrongFieldValue)
        {
            EditorGUILayout.HelpBox($"Wrong {fieldLabel}.", MessageType.Error);
        }
        bool wrongFieldMaxValue = false;
        GUI.enabled = !wrongFieldValue;
        GUI.enabled = !wrongFieldMaxValue;

        if (GUILayout.Button("Apply"))
        {
            Apply();
        }

        GUI.enabled = true;

        // We're doing this in OnGUI() since the Update() function doesn't seem to get called when we show the window with ShowModalUtility().
        var ev = Event.current;
        if (ev.type == EventType.KeyDown || ev.type == EventType.KeyUp)
        {
            switch (ev.keyCode)
            {
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                    Apply();
                    break;
                case KeyCode.Escape:
                    Close();
                    break;
            }
        }
    }

    private void Apply()
    {
        onApply(newFieldValue);
        Close();
    }
}
#endif