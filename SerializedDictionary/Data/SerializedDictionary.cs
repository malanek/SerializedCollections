using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BBExtensions.Dictionary
{
    [Serializable]
    public class SerializedDictionary<TKey, TValue> :
        ICollection<KeyValuePair<TKey, TValue>>,
        IEnumerable<KeyValuePair<TKey, TValue>>,
        IEnumerable, IDictionary<TKey, TValue>,
        IReadOnlyCollection<KeyValuePair<TKey, TValue>>,
        IReadOnlyDictionary<TKey, TValue>,
        ICollection,
        IDictionary,
        ISerializationCallbackReceiver
    {
        [SerializeField] internal List<SerializedKeyValuePair<TKey, TValue>> internalCollection = new();

#if UNITY_EDITOR
        internal IKeyable LookupTable
        {
            get
            {
                if (_lookupTable == null)
                    _lookupTable = new Duplicates<TKey, TValue>(this);
                return _lookupTable;
            }
        }

        private Duplicates<TKey, TValue> _lookupTable;
#endif

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (UnityEditor.BuildPipeline.isBuildingPlayer)
                LookupTable.RemoveDuplicates();
#endif
        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            LookupTable.RecalculateOccurences();
#endif
        }



        public ICollection<TKey> Keys => internalCollection.Select(x => x.Key).ToList();

        public ICollection<TValue> Values => internalCollection.Select(x => x.Value).ToList();

        public int Count => internalCollection.Count;

        public bool IsReadOnly => false;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        public bool IsSynchronized => false;

        public object SyncRoot => this;

        public bool IsFixedSize => false;

        ICollection IDictionary.Keys => (ICollection)Keys;

        ICollection IDictionary.Values => (ICollection)Values;

        public object this[object key]
        {
            get => key is TKey typedKey ? this[typedKey] : throw new ArgumentException("Invalid key type");
            set
            {
                if (key is TKey typedKey && value is TValue typedValue)
                {
                    this[typedKey] = typedValue;
                }
                else
                {
                    throw new ArgumentException("Invalid key or value type");
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                var item = internalCollection.FirstOrDefault(x => x.Key.Equals(key));
                if (item == null)
                {
                    throw new KeyNotFoundException($"Key '{key}' not found in dictionary.");
                }
                return item.Value;
            }
            set
            {
                var item = internalCollection.FirstOrDefault(x => x.Key.Equals(key));
                if (item != null)
                {
                    item.Value = value;
                }
                else
                {
                    internalCollection.Add(new SerializedKeyValuePair<TKey, TValue>(key, value));
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            if (ContainsKey(key))
            {
                throw new ArgumentException($"An element with the same key '{key}' already exists in the dictionary.");
            }
            internalCollection.Add(new SerializedKeyValuePair<TKey, TValue>(key, value));
        }

        public bool ContainsKey(TKey key) => internalCollection.Any(x => x.Key.Equals(key));

        public bool Remove(TKey key)
        {
            var item = internalCollection.FirstOrDefault(x => x.Key.Equals(key));
            if (item != null)
            {
                internalCollection.Remove(item);
                return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            var item = internalCollection.FirstOrDefault(x => x.Key.Equals(key));
            if (item != null)
            {
                value = item.Value;
                return true;
            }
            value = default(TValue);
            return false;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            internalCollection.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return internalCollection.Any(x => x.Key.Equals(item.Key) && EqualityComparer<TValue>.Default.Equals(x.Value, item.Value));
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            foreach (var kvp in internalCollection)
            {
                array[arrayIndex++] = new KeyValuePair<TKey, TValue>(kvp.Key, kvp.Value);
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var existingItem = internalCollection.FirstOrDefault(x => x.Key.Equals(item.Key) && EqualityComparer<TValue>.Default.Equals(x.Value, item.Value));
            if (existingItem != null)
            {
                internalCollection.Remove(existingItem);
                return true;
            }
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return internalCollection.Select(x => new KeyValuePair<TKey, TValue>(x.Key, x.Value)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Rank != 1)
                throw new ArgumentException("Only single dimensional arrays are supported.", nameof(array));

            if (array.GetLowerBound(0) != 0)
                throw new ArgumentException("The array is not zero-based.", nameof(array));

            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is less than zero.");

            if (array.Length - index < Count)
                throw new ArgumentException("The number of elements in the source dictionary is greater than the available space from index to the end of the destination array.");

            if (array is KeyValuePair<TKey, TValue>[] keyValueArray)
            {
                CopyTo(keyValueArray, index);
            }
            else
            {
                if (array is DictionaryEntry[] dictEntryArray)
                {
                    foreach (var item in internalCollection)
                    {
                        dictEntryArray[index++] = new DictionaryEntry(item.Key, item.Value);
                    }
                }
                else
                {
                    if (array is object[] objects)
                    {
                        try
                        {
                            foreach (var item in internalCollection)
                            {
                                objects[index++] = new KeyValuePair<TKey, TValue>(item.Key, item.Value);
                            }
                        }
                        catch (ArrayTypeMismatchException)
                        {
                            throw new ArgumentException("The type of the source dictionary cannot be cast automatically to the type of the destination array.", nameof(array));
                        }
                    }
                    else
                    {
                        throw new ArgumentException("Invalid array type.", nameof(array));
                    }
                }
            }
        }

        public void Add(object key, object value)
        {
            if (key is TKey typedKey && value is TValue typedValue)
            {
                Add(typedKey, typedValue);
            }
            else
            {
                throw new ArgumentException("Invalid key or value type");
            }
        }

        public bool Contains(object key)
        {
            return key is TKey typedKey && ContainsKey(typedKey);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new DictionaryEnumerator<TKey, TValue>(this);
        }

        public void Remove(object key)
        {
            if (key is TKey typedKey)
            {
                Remove(typedKey);
            }
        }


    }

    public class DictionaryEnumerator<TKey, TValue> : IDictionaryEnumerator
    {
        private readonly IEnumerator<KeyValuePair<TKey, TValue>> _enumerator;

        public DictionaryEnumerator(IDictionary<TKey, TValue> dictionary)
        {
            _enumerator = dictionary.GetEnumerator();
        }

        public DictionaryEntry Entry => new DictionaryEntry(_enumerator.Current.Key, _enumerator.Current.Value);

        public object Key => _enumerator.Current.Key;

        public object Value => _enumerator.Current.Value;

        public object Current => Entry;

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator.Reset();
        }
    }
}
