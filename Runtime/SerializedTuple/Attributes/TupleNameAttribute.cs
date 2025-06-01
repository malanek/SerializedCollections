using System;
using UnityEngine;

namespace BBExtensions.SerializedTuple
{
    public sealed class TupleNameAttribute : PropertyAttribute
    {
        public readonly string[] FieldNames =
        {
            "Item 1",
            "Item 2",
            "Item 3",
            "Item 4",
            "Item 5",
            "Item 6",
            "Item 7",
            "Item 8",
        };

        public TupleNameAttribute(string item1, string item2)
        {
            FieldNames[0] = item1;
            FieldNames[1] = item2;
        }

        public TupleNameAttribute(string item1, string item2, string item3) : this(item1, item2)
        {
            FieldNames[2] = item3;
        }

        public TupleNameAttribute(string item1, string item2, string item3, string item4) : this(item1, item2, item3)
        {
            FieldNames[3] = item4;
        }

        public TupleNameAttribute(string item1, string item2, string item3, string item4, string item5) : this(item1,
            item2, item3, item4)
        {
            FieldNames[4] = item5;
        }

        public TupleNameAttribute(string item1, string item2, string item3, string item4, string item5, string item6) :
            this(item1, item2, item3, item4, item5)
        {
            FieldNames[5] = item6;
        }

        public TupleNameAttribute(string item1, string item2, string item3, string item4, string item5, string item6,
            string item7) : this(item1, item2, item3, item4, item5, item6)
        {
            FieldNames[6] = item7;
        }

        public TupleNameAttribute(string item1, string item2, string item3, string item4, string item5, string item6,
            string item7, string item8) : this(item1, item2, item3, item4, item5, item6, item7)
        {
            FieldNames[7] = item8;
        }

        public static void ValidateUsage(Type fieldType)
        {
            if (!IsSTupleType(fieldType))
            {
                throw new InvalidOperationException(
                    $"TupleNameAttribute can only be applied to fields of type STuple or its derived types. Field type: {fieldType}");
            }
        }

        private static bool IsSTupleType(Type type)
        {
            while (type != null)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(STuple<,>))
                {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }
    }
}