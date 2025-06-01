#if UNITY_EDITOR
namespace BBExtensions.Dictionary
{
    internal static class Names
    {
        public const string Key = nameof(SerializedKeyValuePair<byte, byte>.Key);
        public const string Value = nameof(SerializedKeyValuePair<byte, byte>.Value);
        public const string InternalCollection = nameof(SerializedDictionary<byte, byte>.internalCollection);
        public const string LookupTable = nameof(SerializedDictionary<byte, byte>.LookupTable);
        public const string Duplicate = nameof(SerializedKeyValuePair<byte, byte>.HaveDuplicate);
    }
}
#endif