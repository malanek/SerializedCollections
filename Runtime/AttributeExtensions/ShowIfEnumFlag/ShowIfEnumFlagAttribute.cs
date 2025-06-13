using System;
using UnityEngine;

namespace BBExtensions.AttributeDrawers
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowIfEnumFlagAttribute : PropertyAttribute
    {
        public string EnumFieldName { get; }
        public int FlagValue { get; }

        public ShowIfEnumFlagAttribute(string enumFieldName, int flagValue)
        {
            EnumFieldName = enumFieldName;
            FlagValue = flagValue;
        }
    }
}