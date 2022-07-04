namespace MathematicalProgramming.Helpers;

internal readonly struct SymbolMeta
{
    // data
    internal readonly string Key;
    internal readonly OptList<Set> Sets;


    // ctor
    public SymbolMeta()
    {
        Key = string.Empty;
        Sets = default;
    }
    internal SymbolMeta(string key, OptList<Set> sets)
    {
        Key = key;
        Sets = sets;
    }
    internal SymbolMeta(string key)
    {
        Key = key;
        Sets = new();
    }


    // method
    internal SymbolMeta Join(SymbolMeta other)
        => new(string.Empty, Sets + other.Sets);


    // ctor - op
    public static SymbolMeta operator -(SymbolMeta s)
        => new(string.Format("-{0}", s.Key), s.Sets);
    public static SymbolMeta operator +(SymbolMeta s, SymbolMeta t)
        => new(string.Format("{0} + {1}", s.Key, t.Key), s.Sets + t.Sets);
    public static SymbolMeta operator -(SymbolMeta s, SymbolMeta t)
        => new(string.Format("{0} - {1}", s.Key, t.Key), s.Sets + t.Sets);
    public static SymbolMeta operator *(SymbolMeta s, SymbolMeta t)
        => new(string.Format("{0}*{1}", s.Key, t.Key), s.Sets + t.Sets);
    public static SymbolMeta operator /(SymbolMeta s, SymbolMeta t)
        => new(string.Format("{0}/{1}", s.Key, t.Key), s.Sets + t.Sets);
    public static SymbolMeta operator ^(SymbolMeta s, SymbolMeta t)
        => new(string.Format("{0}^{1}", s.Key, t.Key), s.Sets + t.Sets);
    public static SymbolMeta operator %(SymbolMeta s, SymbolMeta t)
        => new(string.Format("{0}%{1}", s.Key, t.Key), s.Sets + t.Sets);
    public static SymbolMeta operator +(SymbolMeta s, double t)
        => new(string.Format("{0} + {1}", s.Key, t), s.Sets);
    public static SymbolMeta operator +(double t, SymbolMeta s)
        => new(string.Format("{0} + {1}", t, s.Key), s.Sets);
    public static SymbolMeta operator -(SymbolMeta s, double t)
        => new(string.Format("{0} - {1}", s.Key, t), s.Sets);
    public static SymbolMeta operator -(double t, SymbolMeta s)
        => new(string.Format("{0} - {1}", t, s.Key), s.Sets);
    public static SymbolMeta operator *(SymbolMeta s, double t)
        => new(string.Format("{0}*{1}", s.Key, t), s.Sets);
    public static SymbolMeta operator *(double t, SymbolMeta s)
        => new(string.Format("{0}*{1}", t, s.Key), s.Sets);
    public static SymbolMeta operator /(SymbolMeta s, double t)
        => new(string.Format("{0}/{1}", s.Key, t), s.Sets);
    public static SymbolMeta operator /(double t, SymbolMeta s)
            => new(string.Format("{0}/{1}", t, s.Key), s.Sets);
    public static SymbolMeta operator ^(SymbolMeta s, double t)
        => new(string.Format("{0}^{1}", s.Key, t), s.Sets);
    public static SymbolMeta operator ^(double t, SymbolMeta s)
        => new(string.Format("{0}^{1}", t, s.Key), s.Sets);
    public static SymbolMeta operator %(SymbolMeta s, double t)
        => new(string.Format("{0}%{1}", s.Key, t), s.Sets);
    public static SymbolMeta operator %(double t, SymbolMeta s)
            => new(string.Format("{0}%{1}", t, s.Key), s.Sets);
}
