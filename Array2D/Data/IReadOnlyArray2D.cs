using System.Collections.Generic;
using UnityEngine;

namespace BBExtensions.Array2D
{
    public interface IReadOnlyArray2D<out T> : IEnumerable<T>
    {
        T this[int x, int y] { get; }
        float ColumnWidth { get; }
        Vector2Int GridSize { get; }
    }
}