namespace MathematicalProgramming;

public readonly struct Par3
{
    // data
    internal readonly string Key;
    internal readonly Opt<Func<int, int, int, double>> GetElement;
    internal readonly Opt<double[][][]> Elements;
    // ctor
    public Par3(string key, Func<int, int, int, double> getElement)
    {
        Key = key;
        GetElement = getElement;
        Elements = default;
    }
    public Par3(string key, double[][][] elements)
    {
        Key = key;
        GetElement = default;
        Elements = elements;
    }
    public Par3(Func<int, int, int, double> getElement)
        : this(ParKey(), getElement) { }
    public Par3(double[][][] elements)
        : this(ParKey(), elements) { }
    // implicit
    public static implicit operator Par3(double[][][] elements)
        => new(elements);
    // common
    public override string ToString()
        => Key;


    // index
    public Sca this[Sca i, Sca j, Sca k]
    {
        get
        {
            if (Elements.IsSome)
            {
                var elements = Elements.Unwrap();
                return new(new(string.Format("{0}({1},{2},{3})", Key, i, j, k), i.Meta.Sets.Union(j.Meta.Sets.Union(k.Meta.Sets))),
                    ijk => elements[(int)i.Value(ijk)][(int)j.Value(ijk)][(int)k.Value(ijk)]);
            }
            else
            {
                var getElement = GetElement.Unwrap();
                return new(new(string.Format("{0}({1},{2},{3})", Key, i, j, k), i.Meta.Sets.Union(j.Meta.Sets.Union(k.Meta.Sets))),
                    ijk => getElement((int)i.Value(ijk), (int)j.Value(ijk), (int)k.Value(ijk)));
            }
        }
    }
}
