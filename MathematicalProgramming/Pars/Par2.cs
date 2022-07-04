namespace MathematicalProgramming;

public readonly struct Par2
{
    // data
    internal readonly string Key;
    internal readonly Opt<Func<int, int, double>> GetElement;
    internal readonly Opt<double[][]> Elements;
    // ctor
    public Par2(string key, Func<int, int, double> getElement)
    {
        Key = key;
        GetElement = getElement;
        Elements = default;
    }
    public Par2(string key, double[][] elements)
    {
        Key = key;
        GetElement = default;
        Elements = elements;
    }
    public Par2(Func<int, int, double> getElement)
        : this(ParKey(), getElement) { }
    public Par2(double[][] elements)
        : this(ParKey(), elements) { }
    // implicit
    public static implicit operator Par2(double[][] elements)
        => new(elements);
    // common
    public override string ToString()
        => Key;


    // index
    public Sca this[Sca i, Sca j]
    {
        get
        {
            if (Elements.IsSome)
            {
                var elements = Elements.Unwrap();
                return new(new(string.Format("{0}({1},{2})", Key, i, j), i.Meta.Sets.Union(j.Meta.Sets)),
                    ijk => elements[(int)i.Value(ijk)][(int)j.Value(ijk)]);
            }
            else
            {
                var getElement = GetElement.Unwrap();
                return new(new(string.Format("{0}({1},{2})", Key, i, j), i.Meta.Sets.Union(j.Meta.Sets)),
                    ijk => getElement((int)i.Value(ijk), (int)j.Value(ijk)));
            }
        }
    }
}
