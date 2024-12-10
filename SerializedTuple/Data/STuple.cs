using System;

namespace BBExtensions.SerializedTuple
{
    [Serializable]
    public class STuple<T1, T2>
    {
        public T1 Item1;
        public T2 Item2;

        public STuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public static implicit operator (T1 item1, T2 item2)(STuple<T1, T2> x) => (x.Item1, x.Item2);

        public static explicit operator STuple<T1, T2>((T1 item1, T2 item2) tuple) => new(tuple.item1, tuple.item2);
    }

    [Serializable]
    public class STuple<T1, T2, T3> : STuple<T1, T2>
    {
        public T3 Item3;

        public STuple(T1 item1, T2 item2, T3 item3) : base(item1, item2)
        {
            Item3 = item3;
        }

        public static implicit operator (T1 item1, T2 item2, T3 item3)(STuple<T1, T2, T3> x) =>
            (x.Item1, x.Item2, x.Item3);

        public static explicit operator STuple<T1, T2, T3>((T1 item1, T2 item2, T3 item3) tuple) =>
            new(tuple.item1, tuple.item2, tuple.item3);
    }

    [Serializable]
    public class STuple<T1, T2, T3, T4> : STuple<T1, T2, T3>
    {
        public T4 Item4;

        public STuple(T1 item1, T2 item2, T3 item3, T4 item4) : base(item1, item2, item3)
        {
            Item4 = item4;
        }

        public static implicit operator (T1 item1, T2 item2, T3 item3, T4 item4)(STuple<T1, T2, T3, T4> x) =>
            (x.Item1, x.Item2, x.Item3, x.Item4);

        public static explicit operator STuple<T1, T2, T3, T4>((T1 item1, T2 item2, T3 item3, T4 item4) tuple) =>
            new(tuple.item1, tuple.item2, tuple.item3, tuple.item4);
    }

    [Serializable]
    public class STuple<T1, T2, T3, T4, T5> : STuple<T1, T2, T3, T4>
    {
        public T5 Item5;

        public STuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) : base(item1, item2, item3, item4)
        {
            Item5 = item5;
        }

        public static implicit operator (T1 item1, T2 item2, T3 item3, T4 item4, T5 item5
            )(STuple<T1, T2, T3, T4, T5> x) => (x.Item1, x.Item2, x.Item3, x.Item4, x.Item5);

        public static explicit operator
            STuple<T1, T2, T3, T4, T5>((T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) tuple) =>
            new(tuple.item1, tuple.item2, tuple.item3, tuple.item4, tuple.item5);
    }

    [Serializable]
    public class STuple<T1, T2, T3, T4, T5, T6> : STuple<T1, T2, T3, T4, T5>
    {
        public T6 Item6;

        public STuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) : base(item1, item2, item3, item4,
            item5)
        {
            Item6 = item6;
        }

        public static implicit operator (T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6
            )(STuple<T1, T2, T3, T4, T5, T6> x) => (x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6);

        public static explicit operator
            STuple<T1, T2, T3, T4, T5, T6>((T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) tuple) =>
            new(tuple.item1, tuple.item2, tuple.item3, tuple.item4, tuple.item5, tuple.item6);
    }

    [Serializable]
    public class STuple<T1, T2, T3, T4, T5, T6, T7> : STuple<T1, T2, T3, T4, T5, T6>
    {
        public T7 Item7;

        public STuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) : base(item1, item2, item3,
            item4, item5, item6)
        {
            Item7 = item7;
        }

        public static implicit operator (T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7
            )(STuple<T1, T2, T3, T4, T5, T6, T7> x) => (x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7);

        public static explicit operator STuple<T1, T2, T3, T4, T5, T6, T7>(
            (T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7) tuple) => new(tuple.item1,
            tuple.item2, tuple.item3, tuple.item4, tuple.item5, tuple.item6, tuple.item7);
    }

    [Serializable]
    public class STuple<T1, T2, T3, T4, T5, T6, T7, T8> : STuple<T1, T2, T3, T4, T5, T6, T7>
    {
        public T8 Item8;

        public STuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) : base(item1,
            item2, item3, item4, item5, item6, item7)
        {
            Item8 = item8;
        }

        public static implicit operator (T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8
            )(STuple<T1, T2, T3, T4, T5, T6, T7, T8> x) =>
            (x.Item1, x.Item2, x.Item3, x.Item4, x.Item5, x.Item6, x.Item7, x.Item8);

        public static explicit operator STuple<T1, T2, T3, T4, T5, T6, T7, T8>(
            (T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7, T8 item8) tuple) => new(tuple.item1,
            tuple.item2, tuple.item3, tuple.item4, tuple.item5, tuple.item6, tuple.item7, tuple.item8);
    }
}