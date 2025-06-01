using System.Collections.Generic;
using System.Linq;

namespace BBExtensions.Dictionary
{
    internal sealed class Duplicates<TKey, TValue> : IKeyable
    {
        private readonly List<int> emptyList;
        private readonly Dictionary<TKey, List<int>> occurences;
        private readonly SerializedDictionary<TKey, TValue> dictionary;

        public Duplicates(SerializedDictionary<TKey, TValue> dictionary)
        {
            occurences = new Dictionary<TKey, List<int>>();
            emptyList = new List<int>();
            this.dictionary = dictionary;
        }

        public object GetKeyAt(int index) => dictionary.internalCollection[index];

        public IReadOnlyList<int> GetOccurences(object key)
        {
            if (key is TKey castKey && occurences.TryGetValue(castKey, out var list))
                return list;
            return emptyList;
        }

        public void RecalculateOccurences()
        {
            occurences.Clear();
            int count = dictionary.internalCollection.Count;
            for (int i = 0; i < count; i++)
            {
                var kvp = dictionary.internalCollection[i];
                if (!Utility.IsValidKey(kvp.Key))
                    continue;
                if (!occurences.ContainsKey(kvp.Key))
                    occurences.Add(kvp.Key, new List<int>() { i });
                else
                    occurences[kvp.Key].Add(i);
            }
        }

        public void RemoveDuplicates()
        {
            dictionary.internalCollection = dictionary.internalCollection.GroupBy(x => x.Key)
                .Where(x => Utility.IsValidKey(x.Key))
                .Select(x => x.First()).ToList();
        }
    }
}