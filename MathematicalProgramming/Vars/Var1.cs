namespace MathematicalProgramming;

public readonly struct Var1 : IVar
{
    // data
    public string Key { get; }
    public VarType VarType { get; }
    public readonly int Len1;
    public readonly Opt<double> Lb;
    public readonly Opt<double> Ub;
    public readonly Opt<Func<int, double>> GetLb;
    public readonly Opt<Func<int, double>> GetUbs;
    // ctor
    public Var1(string key, VarType varType, int len1, double lb, double ub)
    {
        Key = key;
        VarType = varType;
        Len1 = len1;
        Lb = lb;
        Ub = ub;
        GetLb = default;
        GetUbs = default;
    }
    public Var1(string key, VarType varType, int len1, Func<int, double> lbs, Func<int, double> ubs)
    {
        Key = key;
        VarType = varType;
        Len1 = len1;
        Lb = default;
        Ub = default;
        GetLb = Some(lbs);
        GetUbs = Some(ubs);
    }
    // common
    public override string ToString()
        => Key;


    // index
    public Term this[Sca i]
        => new(new(string.Format("{0}({1})", Key, i), i.Meta.Sets), S.GetOne, this, i);


    // method
    public int Dim => 1;
    public bool IsContinuous => VarType == VarType.Continuous;
    public bool IsDiscrete => VarType != VarType.Continuous;
}
