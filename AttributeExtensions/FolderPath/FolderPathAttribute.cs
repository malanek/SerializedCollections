using System;
using UnityEngine;

namespace BBExtensions.AttributeDrawers
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class FolderPathAttribute : PropertyAttribute
    {
        public FolderPathAttribute() { }
    }
}