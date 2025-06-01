#if UNITY_EDITOR
using UnityEditor;

namespace BBExtensions.SerializedTuple
{
    internal static class Extensions
    {
        internal static bool TryFindPropertyRelative(this SerializedProperty serializedProperty,
            string relativePropertyPath, out SerializedProperty result)
        {
            result = serializedProperty.FindPropertyRelative(relativePropertyPath);
            return result != null;
        }
    }
}
#endif