#if UNITY_EDITOR
using System;
using UnityEditor;

namespace BBExtensions.Array2D
{
    public readonly struct LabelWidth : IDisposable
    {
        public float PreviousWidth { get; }

        public LabelWidth(float width)
        {
            PreviousWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = width;
        }

        public void Dispose() => EditorGUIUtility.labelWidth = PreviousWidth;
    }
}
#endif