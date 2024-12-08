using System;
using System.Collections.Generic;
using System.Linq;

namespace BBExtensions.Dictionary
{
    public static class IReadOnlyDictionaryExtensions
    {
        private static readonly Random Random = new();

        public static IReadOnlyDictionary<TKey, TValue> GetRandomSubset<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, int count)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");

            if (count > dictionary.Count)
                throw new ArgumentException("Count cannot be greater than the number of elements in the dictionary.");

            var keys = dictionary.Keys.OrderBy(_ => Random.Next()).Take(count);

            var result = new Dictionary<TKey, TValue>();
            foreach (var key in keys)
            {
                result[key] = dictionary[key];
            }

            return result;
        }
        
        public static IEnumerable<TKey> GetRandomKeys<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, int count)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");

            if (count > dictionary.Count)
                throw new ArgumentException("Count cannot be greater than the number of elements in the dictionary.");

            return dictionary.Keys.OrderBy(_ => Random.Next()).Take(count);
        }

        public static IEnumerable<TValue> GetRandomValues<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, int count)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative.");

            if (count > dictionary.Count)
                throw new ArgumentException("Count cannot be greater than the number of elements in the dictionary.");

            return dictionary.Keys.OrderBy(_ => Random.Next()).Take(count).Select(key => dictionary[key]);
        }
        
        public static TKey GetRandomKey<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.Count == 0)
                throw new InvalidOperationException("Cannot get a random key from an empty dictionary.");

            return dictionary.Keys.OrderBy(_ => Random.Next()).First();
        }

        public static TValue GetRandomValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.Count == 0)
                throw new InvalidOperationException("Cannot get a random value from an empty dictionary.");

            var randomKey = dictionary.Keys.OrderBy(_ => Random.Next()).First();
            return dictionary[randomKey];
        }
    }
}