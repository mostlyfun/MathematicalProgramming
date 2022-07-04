namespace MathematicalProgramming;

public readonly struct JagVar3 : IVar3
{
    // data
    public string Key { get; }
    public VarType VarType { get; }
    public readonly int Len1;
    public readonly Func<int, int> GetLen2;
    public readonly Func<int, int, int> GetLen3;
    public readonly Opt<double> Lb;
    public readonly Opt<double> Ub;
    public readonly Opt<Func<int, int, int, double>> GetLbs;
    public readonly Opt<Func<int, int, int, double>> GetUbs;
    // ctor
    public JagVar3(string key, VarType varType, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, double lb, double ub)
    {
        Key = key;
        VarType = varType;
        Len1 = len1;
        GetLen2 = getLen2;
        GetLen3 = getLen3;
        Lb = lb;
        Ub = ub;
        GetLbs = default;
        GetUbs = default;
    }
    public JagVar3(string key, VarType varType, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3,
                    Func<int, int, int, double> lbs, Func<int, int, int, double> ubs)
    {
        Key = key;
        VarType = varType;
        Len1 = len1;
        GetLen2 = getLen2;
        GetLen3 = getLen3;
        Lb = default;
        Ub = default;
        GetLbs = Some(lbs);
        GetUbs = Some(ubs);
    }
    // common
    public override string ToString()
        => Key;


    // index
    public Term this[Sca i, Sca j, Sca k]
        => new(new(string.Format("{0}({1},{2},{3})", Key, i, j, k),
            i.Meta.Sets.Union(j.Meta.Sets.Union(k.Meta.Sets))),
            S.GetOne, this, new(new List<Sca>() { i, j, k}));


    // method
    public int Dim => 3;
    public bool IsContinuous => VarType == VarType.Continuous;
    public bool IsDiscrete => VarType != VarType.Continuous;
}
