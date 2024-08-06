using System;

namespace BBExtensions.Dictionary
{
    [Serializable]
    internal class SerializedKeyValuePair<TKey, TValue>
    {
        public bool HaveDuplicate;
        public TKey Key;
        public TValue Value;

        public SerializedKeyValuePair(TKey key, TValue value)
        {
            HaveDuplicate = false;
            Key = key;
            Value = value;
        }

        public SerializedKeyValuePair()
        {
            HaveDuplicate = false;
            Key = default(TKey);
            Value = default(TValue);
        }
    }
}