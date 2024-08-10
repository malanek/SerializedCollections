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
        }
    }
}