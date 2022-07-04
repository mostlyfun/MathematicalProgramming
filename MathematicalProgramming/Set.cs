namespace MathematicalProgramming;

public class Set
{
    // data
    internal Model Model;
    internal readonly string Key;
    internal readonly Func<int[], IEnumerable<int>> Generator;
    internal readonly int IndexInModel;
    // ctor
    internal Set(Model model, int indexInModel, string key, Func<int[], IEnumerable<int>> generator)
    {
        Model = model;
        IndexInModel = indexInModel;
        Key = key;
        Generator = generator;
    }
    // common
    public override string ToString()
        => Key;


    // op - sum
    public static SumArg operator |(Set i, double num)
        => new(new(new(i), num));
}
