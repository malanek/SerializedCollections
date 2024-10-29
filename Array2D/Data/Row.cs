using System;

namespace BBExtensions.Array2D
{
    [Serializable]
    internal sealed class Row<T>
    {
        public Cell<T>[] Cells = new Cell<T>[3];

        public Row(int width)
        {
            Cells = new Cell<T>[width];
            for (int i = 0; i < width; i++)
                Cells[i] = new Cell<T>();
        }
    }
}