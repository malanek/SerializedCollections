using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BBExtensions.Array2D
{
    [Serializable]
    public sealed class Array2D<T> : IEnumerable<T>, IReadOnlyArray2D<T>, IEquatable<Array2D<T>>
    {
        [SerializeField] internal Vector2Int gridSize;
        [SerializeField] internal int columnWidth;
        [SerializeField] internal Vector2Int renderSize;
        [SerializeField] internal Row<T>[] rows;

        public int ColumnWidth
        {
            get => columnWidth;
            set
            {
                if (columnWidth < 1)
                    return;
                columnWidth = value;
            }
        }

        public Vector2Int GridSize
        {
            get
            {
                if (rows == null || rows.Length == 0)
                    return new Vector2Int(0, 0);
                int y = rows.Length;
                int x = rows[0].Cells.Length;
                gridSize = new Vector2Int(x, y);
                return gridSize;
            }
            set
            {
                if (value.x <= 0 || value.y <= 0)
                    throw new ArgumentException("GridSize must have positive dimensions.");
                gridSize = value;
                Row<T>[] newRows = new Row<T>[value.y];
                for (int i = 0; i < value.y; i++)
                    newRows[i] = new Row<T>(value.x);
                int minRows = Mathf.Min(value.y, rows.Length);
                for (int y = 0; y < minRows; y++)
                {
                    int minCols = Mathf.Min(value.x, rows[y].Cells.Length);
                    Array.Copy(rows[y].Cells, 0, newRows[y].Cells, 0, minCols);
                }

                rows = newRows;
            }
        }

        public Array2D()
        {
            columnWidth = TryGuessColumnWidth(typeof(T));
            gridSize = new Vector2Int(3, 3);
            rows = new Row<T>[3];
            for (int i = 0; i < 3; i++)
                rows[i] = new Row<T>(3);
        }

        public Array2D(int width, int height)
        {
            columnWidth = TryGuessColumnWidth(typeof(T));
            gridSize = new Vector2Int(width, height);
            rows = new Row<T>[height];
            for (int i = 0; i < height; i++)
                rows[i] = new Row<T>(width);
        }

        public Array2D(T[,] array)
        {
            int width = array.GetLength(0);
            int height = array.GetLength(1);
            columnWidth = TryGuessColumnWidth(typeof(T));
            gridSize = new Vector2Int(width, height);
            rows = new Row<T>[height];
            for (int i = 0; i < height; i++)
                rows[i] = new Row<T>(width);
        }

        public T this[int x, int y]
        {
            get => rows[rows.Length - y - 1].Cells[x].Value;
            set => rows[rows.Length - y - 1].Cells[x].Value = value;
        }

        private int TryGuessColumnWidth(Type type)
        {
            return Consts.TypeWidths.GetValueOrDefault(type, 128);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int y = 0; y < rows.Length; y++)
            {
                for (int x = 0; x < rows[y].Cells.Length; x++)
                {
                    yield return rows[y].Cells[x].Value;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T[,] ToArray2D()
        {
            T[,] result = new T[GridSize.x, GridSize.y];
            for (int y = 0; y < GridSize.y; y++)
            for (int x = 0; x < GridSize.x; x++)
                result[x, y] = this[x, y];
            return result;
        }

        public bool Equals(Array2D<T> other)
        {
            if (other == null)
                return false;
            if (other.GridSize != GridSize)
                return false;
            for (int x = 0; x < GridSize.x; x++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    if (!other[x, y].Equals(this[x, y]))
                        return false;
                }
            }

            return true;
        }
    }
}