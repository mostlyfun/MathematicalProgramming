namespace MathematicalProgramming.Helpers;

public readonly struct OptList<T> : IEnumerable<T>
{
    // data
    internal readonly Opt<List<T>> MaybeList;
    internal readonly Opt<T> Val1;
    internal readonly Opt<T> Val2;


    // ctor
    public OptList()
    {
        MaybeList = default;
        Val1 = default;
        Val2 = default;
    }
    internal OptList(List<T> list)
    {
        MaybeList = list;
        Val1 = default;
        Val2 = default;
    }
    internal OptList(T val1)
    {
        MaybeList = default;
        Val1 = val1;
        Val2 = default;
    }
    internal OptList(T val1, T val2)
    {
        MaybeList = default;
        Val1 = val1;
        Val2 = val2;
    }


    // method
    public T this[int index]
    {
        get
        {
            if (MaybeList.IsSome)
                return MaybeList.Unwrap()[index];
            if (index == 0)
                return Val1.IsSome ? Val1.Unwrap() : throw new IndexOutOfRangeException();
            if (index == 1)
                return Val2.IsSome ? Val2.Unwrap() : throw new IndexOutOfRangeException();
            throw new IndexOutOfRangeException();
        }
    }
    public int Length
    {
        get
        {
            if (MaybeList.IsSome)
                return MaybeList.Unwrap().Count;
            if (Val1.IsNone)
                return 0;
            return Val2.IsNone ? 1 : 2;
        }
    }
    

    // implicit
    public static implicit operator OptList<T>(T val)
        => new(val);


    // op
    public static OptList<T> operator +(OptList<T> s, OptList<T> t)
    {
        if (s.MaybeList.IsSome)
        {
            if (t.MaybeList.IsSome)
            {
                var sl = s.MaybeList.Unwrap();
                sl.AddRange(t.MaybeList.Unwrap());
                return new(sl);
            }
            else if (t.Val1.IsSome)
            {
                var sl = s.MaybeList.Unwrap();
                sl.Add(t.Val1.Unwrap());
                if (t.Val2.IsSome)
                    sl.Add(t.Val2.Unwrap());
                return new(sl);
            }
            else
                return s;
        }
        else if (s.Val1.IsSome)
        {
            if (s.Val2.IsNone) // s == 1
            {
                if (t.MaybeList.IsSome)
                {
                    var tl = t.MaybeList.Unwrap();
                    tl.Add(s.Val1.Unwrap());
                    return new(tl);
                }
                else if (t.Val1.IsSome)
                {
                    if (t.Val2.IsNone) // t == 1
                        return new(s.Val1.Unwrap(), t.Val1.Unwrap());
                    else
                        return new(new List<T>() { s.Val1.Unwrap(), t.Val1.Unwrap(), t.Val2.Unwrap() });
                }
                else
                    return s;
            }
            else // s == 2
            {
                if (t.MaybeList.IsSome)
                {
                    var tl = t.MaybeList.Unwrap();
                    tl.Add(s.Val1.Unwrap());
                    tl.Add(s.Val2.Unwrap());
                    return new(tl);
                }
                else if (t.Val1.IsSome)
                {
                    if (t.Val2.IsNone) // t == 1
                        return new(new List<T>() { s.Val1.Unwrap(), s.Val2.Unwrap(), t.Val1.Unwrap() });
                    else
                        return new(new List<T>() { s.Val1.Unwrap(), s.Val2.Unwrap(), t.Val1.Unwrap(), t.Val2.Unwrap() });
                }
                else
                    return s;
            }
        }
        else // s == 0
            return t;
    }
    public IEnumerator<T> GetEnumerator()
        => new OptListEnumerator<T>(this);
    IEnumerator IEnumerable.GetEnumerator()
        => new OptListEnumerator<T>(this);
}
