using System;

namespace BBExtensions.Array2D
{
    [Serializable]
    public sealed class Cell<T>
    {
        public T Value;

        public Cell()
        {
            Value = default(T);
        }
    }
}