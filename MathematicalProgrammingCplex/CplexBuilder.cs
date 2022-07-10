namespace MathematicalProgrammingCplex;

public class CplexBuilder : ModelBuilder<Cplex, ILinearNumExpr, INumVar>
{
    // data
    public readonly bool AddNames;
    // ctor
    public CplexBuilder(bool addNames)
    {
        AddNames = addNames;
    }


    // model builder
    protected override Cplex NewModel(Model abstractModel)
        => new();
    protected override ILinearNumExpr NewConstraint(Cplex model)
        => model.LinearNumExpr();
    protected override INumVar NewVar0(Cplex model, Var0 var)
    {
        if (AddNames)
            return var.IsDiscrete ? model.IntVar((int)var.Lb, (int)var.Ub, var.Key) : model.NumVar(var.Lb, var.Ub, var.Key);
        else
            return var.IsDiscrete ? model.IntVar((int)var.Lb, (int)var.Ub) : model.NumVar(var.Lb, var.Ub);
    }
    protected override INumVar[] NewVar1(Cplex model, Var1 var)
    {
        static (double[] lb, double[] ub) GetBoundsCont(Var1 var)
        {
            var getLb = var.GetLb.Unwrap();
            var getUb = var.GetUbs.Unwrap();
            double[] lb = new double[var.Len1], ub = new double[var.Len1];
            for (int i = 0; i < lb.Length; i++)
            {
                lb[i] = getLb(i);
                ub[i] = getUb(i);
            }
            return (lb, ub);
        }
        static (int[] lb, int[] ub) GetBoundsDisc(Var1 var)
        {
            var getLb = var.GetLb.Unwrap();
            var getUb = var.GetUbs.Unwrap();
            int[] lb = new int[var.Len1], ub = new int[var.Len1];
            for (int i = 0; i < lb.Length; i++)
            {
                lb[i] = (int)getLb(i);
                ub[i] = (int)getUb(i);
            }
            return (lb, ub);
        }
        static string[] GetNames(Var1 var)
        {
            var names = new string[var.Len1];
            for (int i = 0; i < var.Len1; i++)
                names[i] = string.Format("{0}({1})", var.Key, i);
            return names;
        }
        if (var.Lb.IsSome)
            return VarArr(model, var.IsDiscrete, var.Len1, var.Lb.Unwrap(), var.Ub.Unwrap(),
                    AddNames ? GetNames(var) : Array.Empty<string>());
        else
        {
            var names = AddNames ? GetNames(var) : Array.Empty<string>();
            return var.IsDiscrete
                ? VarArrDisc(model, var.Len1, GetBoundsDisc(var), names)
                : VarArrCont(model, var.Len1, GetBoundsCont(var), names);
        }
    }
    protected override INumVar[][] NewVar2(Cplex model, Var2 var)
    {
        var x = new INumVar[var.Len1][];
        var names = AddNames ? new string[var.Len2] : Array.Empty<string>();
        if (var.Lb.IsSome)
        {
            for (int i = 0; i < x.Length; i++)
            {
                if (AddNames)
                    SetNames(var.Key, i, names);
                x[i] = VarArr(model, var.IsDiscrete, var.Len2, var.Lb.Unwrap(), var.Ub.Unwrap(), names);
            }
        }
        else
        {
            (int[] lbdisc, int[] ubdisc, double[] lbcont, double[] ubcont) = AllocBounds(var.IsDiscrete, var.Len2);
            for (int i = 0; i < x.Length; i++)
            {
                if (AddNames)
                    SetNames(var.Key, i, names);
                x[i] = var.IsDiscrete
                    ? VarArrDisc(model, var.Len2, SetBoundsDisc(var.GetLbs.Unwrap(), var.GetUbs.Unwrap(), i, lbdisc, ubdisc), names)
                    : VarArrCont(model, var.Len2, SetBoundsCont(var.GetLbs.Unwrap(), var.GetUbs.Unwrap(), i, lbcont, ubcont), names);
            }
        }
        return x;
    }
    protected override INumVar[][] NewJagVar2(Cplex model, JagVar2 var)
    {
        var x = new INumVar[var.Len1][];
        if (var.Lb.IsSome)
        {
            for (int i = 0; i < x.Length; i++)
            {
                int len2 = var.GetLen2(i);
                var names = AddNames ? new string[len2] : Array.Empty<string>();
                if (AddNames)
                    SetNames(var.Key, i, names);
                x[i] = VarArr(model, var.IsDiscrete, len2, var.Lb.Unwrap(), var.Ub.Unwrap(), names);
            }
        }
        else
        {
            for (int i = 0; i < x.Length; i++)
            {
                int len2 = var.GetLen2(i);
                (int[] lbdisc, int[] ubdisc, double[] lbcont, double[] ubcont) = AllocBounds(var.IsDiscrete, len2);
                var names = AddNames ? new string[len2] : Array.Empty<string>();
                if (AddNames)
                    SetNames(var.Key, i, names);
                x[i] = var.IsDiscrete
                    ? VarArrDisc(model, len2, SetBoundsDisc(var.GetLbs.Unwrap(), var.GetUbs.Unwrap(), i, lbdisc, ubdisc), names)
                    : VarArrCont(model, len2, SetBoundsCont(var.GetLbs.Unwrap(), var.GetUbs.Unwrap(), i, lbcont, ubcont), names);
            }
        }
        return x;
    }
    protected override INumVar[][][] NewVar3(Cplex model, Var3 var)
    {
        var x = new INumVar[var.Len1][][];
        var names = AddNames ? new string[var.Len3] : Array.Empty<string>();
        if (var.Lb.IsSome)
        {
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = new INumVar[var.Len2][];
                for (int j = 0; j < x[i].Length; j++)
                {
                    if (AddNames)
                        SetNames(var.Key, i, j, names);
                    x[i][j] = VarArr(model, var.IsDiscrete, var.Len3, var.Lb.Unwrap(), var.Ub.Unwrap(), names);
                }
            }
        }
        else
        {
            (int[] lbdisc, int[] ubdisc, double[] lbcont, double[] ubcont) = AllocBounds(var.IsDiscrete, var.Len3);
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = new INumVar[var.Len2][];
                for (int j = 0; j < x[i].Length; j++)
                {
                    if (AddNames)
                        SetNames(var.Key, i, j, names);
                    x[i][j] = var.IsDiscrete
                        ? VarArrDisc(model, var.Len3, SetBoundsDisc(var.GetLbs.Unwrap(), var.GetUbs.Unwrap(), i, j, lbdisc, ubdisc), names)
                        : VarArrCont(model, var.Len3, SetBoundsCont(var.GetLbs.Unwrap(), var.GetUbs.Unwrap(), i, j, lbcont, ubcont), names);
                }
            }
        }
        return x;
    }
    protected override INumVar[][][] NewJagVar3(Cplex model, JagVar3 var)
    {
        var x = new INumVar[var.Len1][][];
        if (var.Lb.IsSome)
        {
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = new INumVar[var.GetLen2(i)][];
                for (int j = 0; j < x[i].Length; j++)
                {
                    int len3 = var.GetLen3(i, j);
                    var names = AddNames ? new string[len3] : Array.Empty<string>();
                    if (AddNames)
                        SetNames(var.Key, i, j, names);
                    x[i][j] = VarArr(model, var.IsDiscrete, len3, var.Lb.Unwrap(), var.Ub.Unwrap(), names);
                }
            }
        }
        else
        {
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = new INumVar[var.GetLen2(i)][];
                for (int j = 0; j < x[i].Length; j++)
                {
                    int len3 = var.GetLen3(i, j);
                    var names = AddNames ? new string[len3] : Array.Empty<string>();
                    (int[] lbdisc, int[] ubdisc, double[] lbcont, double[] ubcont) = AllocBounds(var.IsDiscrete, len3);
                    if (AddNames)
                        SetNames(var.Key, i, j, names);
                    x[i][j] = var.IsDiscrete
                        ? VarArrDisc(model, len3, SetBoundsDisc(var.GetLbs.Unwrap(), var.GetUbs.Unwrap(), i, j, lbdisc, ubdisc), names)
                        : VarArrCont(model, len3, SetBoundsCont(var.GetLbs.Unwrap(), var.GetUbs.Unwrap(), i, j, lbcont, ubcont), names);
                }
            }
        }
        return x;
    }
    protected override INumVar[][][][] NewVar4(Cplex model, Var4 var)
    {
        var x = new INumVar[var.Len1][][][];
        var names = AddNames ? new string[var.Len4] : Array.Empty<string>();
        if (var.Lb.IsSome)
        {
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = new INumVar[var.Len2][][];
                for (int j = 0; j < x[i].Length; j++)
                {
                    x[i][j] = new INumVar[var.Len3][];
                    for (int k = 0; k < x[i][j].Length; k++)
                    {
                        if (AddNames)
                            SetNames(var.Key, i, j, k, names);
                        x[i][j][k] = VarArr(model, var.IsDiscrete, var.Len4, var.Lb.Unwrap(), var.Ub.Unwrap(), names);
                    }
                }
            }
        }
        else
        {
            (int[] lbdisc, int[] ubdisc, double[] lbcont, double[] ubcont) = AllocBounds(var.IsDiscrete, var.Len4);
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = new INumVar[var.Len2][][];
                for (int j = 0; j < x[i].Length; j++)
                {
                    x[i][j] = new INumVar[var.Len3][];
                    for (int k = 0; k < x[i][j].Length; k++)
                    {
                        if (AddNames)
                            SetNames(var.Key, i, j, k, names);
                        x[i][j][k] = var.IsDiscrete
                            ? VarArrDisc(model, var.Len4, SetBoundsDisc(var.GetLbs.Unwrap(), var.GetUbs.Unwrap(), i, j, k, lbdisc, ubdisc), names)
                            : VarArrCont(model, var.Len4, SetBoundsCont(var.GetLbs.Unwrap(), var.GetUbs.Unwrap(), i, j, k, lbcont, ubcont), names);
                    }
                }
            }
        }
        return x;
    }
    protected override INumVar[][][][] NewJagVar4(Cplex model, JagVar4 var)
    {
        var x = new INumVar[var.Len1][][][];
        if (var.Lb.IsSome)
        {
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = new INumVar[var.GetLen2(i)][][];
                for (int j = 0; j < x[i].Length; j++)
                {
                    x[i][j] = new INumVar[var.GetLen3(i, j)][];
                    for (int k = 0; k < x[i][j].Length; k++)
                    {
                        int len4 = var.GetLen4(i, j, k);
                        var names = AddNames ? new string[len4] : Array.Empty<string>();
                        if (AddNames)
                            SetNames(var.Key, i, j, k, names);
                        x[i][j][k] = VarArr(model, var.IsDiscrete, len4, var.Lb.Unwrap(), var.Ub.Unwrap(), names);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = new INumVar[var.GetLen2(i)][][];
                for (int j = 0; j < x[i].Length; j++)
                {
                    x[i][j] = new INumVar[var.GetLen3(i, j)][];
                    for (int k = 0; k < x[i][j].Length; k++)
                    {
                        int len4 = var.GetLen4(i, j, k);
                        var names = AddNames ? new string[len4] : Array.Empty<string>();
                        (int[] lbdisc, int[] ubdisc, double[] lbcont, double[] ubcont) = AllocBounds(var.IsDiscrete, len4);
                        if (AddNames)
                            SetNames(var.Key, i, j, k, names);
                        x[i][j][k] = var.IsDiscrete
                            ? VarArrDisc(model, len4, SetBoundsDisc(var.GetLbs.Unwrap(), var.GetUbs.Unwrap(), i, j, k, lbdisc, ubdisc), names)
                            : VarArrCont(model, len4, SetBoundsCont(var.GetLbs.Unwrap(), var.GetUbs.Unwrap(), i, j, k, lbcont, ubcont), names);
                    }
                }
            }
        }
        return x;
    }
    protected override void AddTermToConstr(ILinearNumExpr constraint, double coefficient, INumVar var)
        => constraint.AddTerm(coefficient, var);
    protected override void AddConstrToModel(Constr constrExpr, int[] indices, Cplex model, ILinearNumExpr constraint, double rhs)
    {
        switch (constrExpr.Relation)
        {
            case ConstraintRelation.Leq:
                if (AddNames)
                    model.AddLe(constraint, rhs, Model.GetConstraintKey(constrExpr, indices));
                else
                    model.AddLe(constraint, rhs);
                break;
            case ConstraintRelation.Geq:
                if (AddNames)
                    model.AddGe(constraint, rhs, Model.GetConstraintKey(constrExpr, indices));
                else
                    model.AddGe(constraint, rhs);
                break;
            case ConstraintRelation.Eq:
                if (AddNames)
                    model.AddEq(constraint, rhs, Model.GetConstraintKey(constrExpr, indices));
                else
                    model.AddEq(constraint, rhs);
                break;
            default: throw new System.Exception();
        }
    }
    protected override void AddObjectiveToModel((string Key, ObjType Type, Expr Expr) objective, Cplex model, ILinearNumExpr obj)
    {
        switch (objective.Type)
        {
            case ObjType.Minimize:
                if (AddNames)
                    model.AddMinimize(obj, objective.Key);
                else
                    model.AddMinimize(obj);
                break;
            case ObjType.Maximize:
                if (AddNames)
                    model.AddMaximize(obj, objective.Key);
                else
                    model.AddMaximize(obj);
                break;
            default: throw new System.Exception();
        }
    }
    protected override void AddVarBounds(Cplex model, Dictionary<string, IVar> vars)
    {
    }


    // helpers
    static INumVar[] VarArr(Cplex model, bool isDiscrete, int len, double lb, double ub, string[] names)
    {
        if (names.Length == 0)
            return isDiscrete ? model.IntVarArray(len, (int)lb, (int)ub) : model.NumVarArray(len, lb, ub);
        else
            return isDiscrete ? model.IntVarArray(len, (int)lb, (int)ub, names) : model.NumVarArray(len, lb, ub, names);
    }
    static INumVar[] VarArrCont(Cplex model, int len, (double[] lb, double[] ub) bounds, string[] names)
    {
        if (names.Length == 0)
            return model.NumVarArray(len, bounds.lb, bounds.ub);
        else
            return model.NumVarArray(len, bounds.lb, bounds.ub, names);
    }
    static INumVar[] VarArrDisc(Cplex model, int len, (int[] lb, int[] ub) bounds, string[] names)
    {
        if (names.Length == 0)
            return model.IntVarArray(len, bounds.lb, bounds.ub);
        else
            return model.IntVarArray(len, bounds.lb, bounds.ub, names);
    }
    static (int[] lbdisc, int[] ubdisc, double[] lbcont, double[] ubcont) AllocBounds(bool isDiscrete, int len)
    {
        double[] lbcont, ubcont;
        int[] lbdisc, ubdisc;
        if (isDiscrete)
        {
            lbcont = ubcont = Array.Empty<double>();
            lbdisc = new int[len];
            ubdisc = new int[len];
        }
        else
        {
            lbdisc = ubdisc = Array.Empty<int>();
            lbcont = new double[len];
            ubcont = new double[len];
        }
        return (lbdisc, ubdisc, lbcont, ubcont);
    }
    Cplex Cplex
    {
        get
        {
            if (BuiltModel.IsNone)
                throw new System.Exception();
            return BuiltModel.Unwrap();
        }
    }
    // helpers - var2
    static void SetNames(string key, int i, string[] names)
    {
        for (int j = 0; j < names.Length; j++)
            names[j] = string.Format("{0}({1},{2})", key, i, j);
    }
    static (double[] lb, double[] ub) SetBoundsCont(Func<int, int, double> getLb, Func<int, int, double> getUb, int i, double[] lb, double[] ub)
    {
        for (int j = 0; j < lb.Length; j++)
        {
            lb[j] = getLb(i, j);
            ub[j] = getUb(i, j);
        }
        return (lb, ub);
    }
    static (int[] lb, int[] ub) SetBoundsDisc(Func<int, int, double> getLb, Func<int, int, double> getUb, int i, int[] lb, int[] ub)
    {
        for (int j = 0; j < lb.Length; j++)
        {
            lb[j] = (int)getLb(i, j);
            ub[j] = (int)getUb(i, j);
        }
        return (lb, ub);
    }
    // helpers - var3
    static void SetNames(string key, int i, int j, string[] names)
    {
        for (int k = 0; k < names.Length; k++)
            names[k] = string.Format("{0}({1},{2},{3})", key, i, j, k);
    }
    static (double[] lb, double[] ub) SetBoundsCont(Func<int, int, int, double> getLb, Func<int, int, int, double> getUb, int i, int j, double[] lb, double[] ub)
    {
        for (int k = 0; k < lb.Length; k++)
        {
            lb[k] = getLb(i, j, k);
            ub[k] = getUb(i, j, k);
        }
        return (lb, ub);
    }
    static (int[] lb, int[] ub) SetBoundsDisc(Func<int, int, int, double> getLb, Func<int, int, int, double> getUb, int i, int j, int[] lb, int[] ub)
    {
        for (int k = 0; k < lb.Length; k++)
        {
            lb[k] = (int)getLb(i, j, k);
            ub[k] = (int)getUb(i, j, k);
        }
        return (lb, ub);
    }
    // helpers - var4
    static void SetNames(string key, int i, int j, int k, string[] names)
    {
        for (int l = 0; l < names.Length; l++)
            names[l] = string.Format("{0}({1},{2},{3},{4})", key, i, j, k, l);
    }
    static (double[] lb, double[] ub) SetBoundsCont(Func<int, int, int, int, double> getLb, Func<int, int, int, int, double> getUb,
                                                int i, int j, int k, double[] lb, double[] ub)
    {
        for (int l = 0; l < lb.Length; l++)
        {
            lb[l] = getLb(i, j, k, l);
            ub[l] = getUb(i, j, k, l);
        }
        return (lb, ub);
    }
    static (int[] lb, int[] ub) SetBoundsDisc(Func<int, int, int, int, double> getLb, Func<int, int, int, int, double> getUb,
                                                int i, int j, int k, int[] lb, int[] ub)
    {
        for (int l = 0; l < lb.Length; l++)
        {
            lb[k] = (int)getLb(i, j, k, l);
            ub[k] = (int)getUb(i, j, k, l);
        }
        return (lb, ub);
    }


    // method
    protected override Res<bool> Solve(Cplex builtModel)
        => TryMap(() => { builtModel.Solve(); return true; });


    // method - get - value
    public override double GetVal0(string keyVar0)
        => Cplex.GetValue(GetVar0(keyVar0));
    public override double[] GetVal1(string keyVar1, int len1)
        => Cplex.GetValues(GetVar1(keyVar1));
    public override double[][] GetVal2(string keyVar2, int len1, int len2)
    {
        var var = GetVar2(keyVar2);
        return var.Select(x1 => Cplex.GetValues(x1)).ToArray();
    }
    public override double[][] GetJagVal2(string keyJagVar2, int len1, Func<int, int> getLen2)
    {
        var var = GetJagVar2(keyJagVar2);
        return var.Select(x1 => Cplex.GetValues(x1)).ToArray();
    }
    public override double[][][] GetVal3(string keyVar3, int len1, int len2, int len3)
    {
        var var = GetVar3(keyVar3);
        return var.Select(x2 => x2.Select(x1 => Cplex.GetValues(x1)).ToArray()).ToArray();
    }
    public override double[][][] GetJagVal3(string keyJagVar3, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3)
    {
        var var = GetJagVar3(keyJagVar3);
        return var.Select(x2 => x2.Select(x1 => Cplex.GetValues(x1)).ToArray()).ToArray();
    }
    public override double[][][][] GetVal4(string keyVar4, int len1, int len2, int len3, int len4)
    {
        var var = GetVar4(keyVar4);
        return var.Select(x3 => x3.Select(x2 => x2.Select(x1 => Cplex.GetValues(x1)).ToArray()).ToArray()).ToArray();
    }
    public override double[][][][] GetJagVal4(string keyJagVar4, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, int> getLen4)
    {
        var var = GetJagVar4(keyJagVar4);
        return var.Select(x3 => x3.Select(x2 => x2.Select(x1 => Cplex.GetValues(x1)).ToArray()).ToArray()).ToArray();
    }
}
