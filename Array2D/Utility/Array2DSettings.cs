using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace BBExtensions.Array2D
{
    [Serializable]
    internal sealed class Array2DSettings
    {
        public Vector2Int GridSize;
        [FormerlySerializedAs("CellWidth")] public int ColumnWidth;

        public Array2DSettings(Vector2Int gridSize, int columnWidth)
        {
            GridSize = gridSize;
            ColumnWidth = columnWidth;
        }
    }
}