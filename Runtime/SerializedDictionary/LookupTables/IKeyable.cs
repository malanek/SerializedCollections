using System.Collections.Generic;

internal interface IKeyable
{
    void RecalculateOccurences();
    IReadOnlyList<int> GetOccurences(object key);
    object GetKeyAt(int index);
    void RemoveDuplicates();
}