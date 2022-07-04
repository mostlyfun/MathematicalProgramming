namespace MathematicalProgramming;

public readonly ref struct ForAll
{
    // data
    internal readonly string Key;
    internal readonly OptList<Set> Sets;
    // ctor
    public ForAll(string key, OptList<Set> sets)
    {
        Key = key;
        Sets = sets;
    }
    // implicit
    public static implicit operator ForAll(Set i)
        => new(ConKey(), i);
    public static implicit operator ForAll((Set i, Set j) sets)
        => new(ConKey(), new(sets.i, sets.j));
    public static implicit operator ForAll((Set i, Set j, Set k) sets)
        => new(ConKey(), new(new List<Set>() { sets.i, sets.j, sets.k}));
    public static implicit operator ForAll((Set i, Set j, Set k, Set l) sets)
        => new(ConKey(), new(new List<Set>() { sets.i, sets.j, sets.k, sets.l }));
}
