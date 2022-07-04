namespace MathematicalProgramming.Helpers;

// todo: allow for a couple of elements without allocating the hashset
internal readonly struct OptSet<T>
{
    // data
    internal readonly Opt<HashSet<T>> MaybeSet;
    // ctor
    public OptSet()
        => MaybeSet = default;
    internal OptSet(HashSet<T> set)
        => MaybeSet = set;
    internal OptSet(T v1)
        : this(new HashSet<T>() { v1 }) { }
    internal OptSet(T v1, T v2)
        : this(new HashSet<T>() { v1, v2 }) { }
    // common
    public override string ToString()
        => MaybeSet.IsNone ? string.Empty : string.Join(", ", MaybeSet.Unwrap());

    // op
    public static OptSet<T> operator +(OptSet<T> s, OptSet<T> t)
    {
        if (s.MaybeSet.IsSome)
        {
            if (t.MaybeSet.IsSome)
            {
                HashSet<T> su = s.MaybeSet.Unwrap(), tu = t.MaybeSet.Unwrap();
                foreach (var item in tu)
                    if (!su.Contains(item))
                        su.Add(item);
                return new(su);
            }
            else
                return s;
        }
        else
            return t;
    }


    // method
    public int Length
    {
        get
        {
            if (MaybeSet.IsSome)
                return MaybeSet.Unwrap().Count;
            else
                return 0;
        }
    }
    public bool Contains(T value)
        => MaybeSet.IsSome && MaybeSet.Unwrap().Contains(value);
    public IEnumerable<U> Select<U>(Func<T, U> convert)
        => MaybeSet.IsNone ? Array.Empty<U>() : MaybeSet.Unwrap().Select(x => convert(x));
}
