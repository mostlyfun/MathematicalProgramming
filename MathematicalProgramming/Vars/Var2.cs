namespace MathematicalProgramming;

public readonly struct Var2 : IVar2
{
    // data
    public string Key { get; }
    public VarType VarType { get; }
    public readonly int Len1, Len2;
    public readonly Opt<double> Lb;
    public readonly Opt<double> Ub;
    public readonly Opt<Func<int, int, double>> GetLbs;
    public readonly Opt<Func<int, int, double>> GetUbs;
    // ctor
    public Var2(string key, VarType varType, int len1, int len2, double lb, double ub)
    {
        Key = key;
        VarType = varType;
        Len1 = len1;
        Len2 = len2;
        Lb = lb;
        Ub = ub;
        GetLbs = default;
        GetUbs = default;
    }
    public Var2(string key, VarType varType, int len1, int len2, Func<int, int, double> lbs, Func<int, int, double> ubs)
    {
        Key = key;
        VarType = varType;
        Len1 = len1;
        Len2 = len2;
        Lb = default;
        Ub = default;
        GetLbs = Some(lbs);
        GetUbs = Some(ubs);
    }
    // common
    public override string ToString()
        => Key;


    // index
    public Term this[Sca i, Sca j]
        => new(new(string.Format("{0}({1},{2})", Key, i, j),
            i.Meta.Sets.Union(j.Meta.Sets)),
            S.GetOne, this, new(i, j));


    // method
    public int Dim => 2;
    public bool IsContinuous => VarType == VarType.Continuous;
    public bool IsDiscrete => VarType != VarType.Continuous;
}
