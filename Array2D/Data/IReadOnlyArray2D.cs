using System.Collections.Generic;
using UnityEngine;

namespace BBExtensions.Array2D
{
    public interface IReadOnlyArray2D<out T> : IEnumerable<T>
    {
        T this[int x, int y] { get; }
        int ColumnWidth { get; }
        Vector2Int GridSize { get; }
        T[,] ToArray2D();
    }
}