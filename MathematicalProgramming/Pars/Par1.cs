namespace MathematicalProgramming;

public readonly struct Par1
{
    // data
    internal readonly string Key;
    internal readonly Opt<Func<int, double>> GetElement;
    internal readonly Opt<double[]> Elements;
    // ctor
    public Par1(string key, Func<int, double> getElement)
    {
        Key = key;
        GetElement = getElement;
        Elements = default;
    }
    public Par1(string key, double[] elements)
    {
        Key = key;
        GetElement = default;
        Elements = elements;
    }
    public Par1(Func<int, double> getElement)
        : this(ParKey(), getElement) { }
    public Par1(double[] elements)
        : this(ParKey(), elements) { }
    // implicit
    public static implicit operator Par1(double[] elements)
        => new(elements);
    // common
    public override string ToString()
        => Key;

    // index
    public Sca this[Sca i]
    {
        get
        {
            if (Elements.IsSome)
            {
                var elements = Elements.Unwrap();
                return new(new(string.Format("{0}({1})", Key, i), i.Meta.Sets), ijk => elements[(int)i.Value(ijk)]);
            }
            else
            {
                var getElement = GetElement.Unwrap();
                return new(new(string.Format("{0}({1})", Key, i), i.Meta.Sets), ijk => getElement((int)i.Value(ijk)));
            }
        }
    }
}
