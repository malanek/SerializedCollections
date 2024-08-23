using UnityEngine;

namespace BBExtensions.AttributeDrawers
{
    public class RadioAttribute : PropertyAttribute
    {
        public string GroupName;

        public RadioAttribute(string groupName = "Default")
        {
            GroupName = groupName;
        }
    }
}