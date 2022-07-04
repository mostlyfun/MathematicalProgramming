namespace MathematicalProgramming;

public readonly struct Par4
{
    // data
    internal readonly string Key;
    internal readonly Opt<Func<int, int, int, int, double>> GetElement;
    internal readonly Opt<double[][][][]> Elements;
    // ctor
    public Par4(string key, Func<int, int, int, int, double> getElement)
    {
        Key = key;
        GetElement = getElement;
        Elements = default;
    }
    public Par4(string key, double[][][][] elements)
    {
        Key = key;
        GetElement = default;
        Elements = elements;
    }
    public Par4(Func<int, int, int, int, double> getElement)
        : this(ParKey(), getElement) { }
    public Par4(double[][][][] elements)
        : this(ParKey(), elements) { }
    // implicit
    public static implicit operator Par4(double[][][][] elements)
        => new(elements);
    // common
    public override string ToString()
        => Key;


    // index
    public Sca this[Sca i, Sca j, Sca k, Sca l]
    {
        get
        {
            if (Elements.IsSome)
            {
                var elements = Elements.Unwrap();
                return new(new(string.Format("{0}({1},{2},{3},{4})", Key, i, j, k, l), i.Meta.Sets.Union(j.Meta.Sets.Union(k.Meta.Sets.Union(l.Meta.Sets)))),
                    ijk => elements[(int)i.Value(ijk)][(int)j.Value(ijk)][(int)k.Value(ijk)][(int)l.Value(ijk)]);
            }
            else
            {
                var getElement = GetElement.Unwrap();
                return new(new(string.Format("{0}({1},{2},{3},{4})", Key, i, j, k, l), i.Meta.Sets.Union(j.Meta.Sets.Union(k.Meta.Sets.Union(l.Meta.Sets)))),
                    ijk => getElement((int)i.Value(ijk), (int)j.Value(ijk), (int)k.Value(ijk), (int)l.Value(ijk)));
            }
        }
    }
}
