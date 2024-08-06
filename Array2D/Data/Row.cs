using System;

namespace BBExtensions.Array2D
{
    [Serializable]
    public class Row<T>
    {
        public T[] elements = new T[3];

        public Row(int width)
        {
            elements = new T[width];
        }
    }
}