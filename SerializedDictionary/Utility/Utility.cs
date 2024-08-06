using UnityEngine;

namespace BBExtensions.Dictionary
{
    public static class Utility
    {
        public static bool IsValidKey(object obj)
        {
            try
            {
                return !(obj is Object unityObject && unityObject == null);
            }
            catch
            {
                return false;
            }
        }
    }
}