namespace MathematicalProgramming;

public abstract class ModelBuilder<M, C, V>
{
    // data
    public Opt<M> BuiltModel { get; private set; }
    protected Dictionary<string, (int Dim, int Ind)> GetConcrVars;
    Dictionary<string, ExprHandler<C, V>> Handlers;
    int[] ijk;
    protected internal V[] Vars0;
    protected internal V[][] Vars1;
    protected internal V[][][] Vars2;
    protected internal V[][][][] Vars3;
    protected internal V[][][][][] Vars4;
    // ctor
    public ModelBuilder()
    {
        BuiltModel = default;
        GetConcrVars = EmptyGetConcrVars;
        Handlers = new();
        ijk = Array.Empty<int>();
        Vars0 = Array.Empty<V>();
        Vars1 = Array.Empty<V[]>();
        Vars2 = Array.Empty<V[][]>();
        Vars3 = Array.Empty<V[][][]>();
        Vars4 = Array.Empty<V[][][][]>();
    }


    // method - build helpers
    internal Dictionary<string, ExprHandler<C, V>> SetHandlers(Model abstractModel, M model)
    {
        InitVarArrays(model, abstractModel.Variables);

        Handlers = new(abstractModel.Constraints.Count + 1);
        foreach (var item in abstractModel.Constraints)
            AddOrUpdateHandler(item.Value);
        Handlers.Add(abstractModel.Objective.Key, GetExprHandler(abstractModel.Objective.Expr));
        return Handlers;
    }
    void AddOrUpdateHandler(Constr constr)
        => Handlers[constr.Key] = GetExprHandler(constr.Expr.LhsMinRhs);
    void InitVarArrays(M model, Dictionary<string, IVar> vars)
    {
        GetConcrVars = new();

        int dim = 0;
        int i = 0;
        var varsOfDim = vars.Where(x => x.Value.Dim == dim);
        int len = varsOfDim.Count();
        Vars0 = len == 0 ? Array.Empty<V>() : new V[len];
        foreach (var item in varsOfDim)
        {
            var var = (Var0)item.Value;
            GetConcrVars.Add(var.Key, (dim, i));
            Vars0[i++] = NewVar0(model, var);
        }

        dim = 1;
        i = 0;
        varsOfDim = vars.Where(x => x.Value.Dim == dim);
        len = varsOfDim.Count();
        Vars1 = len == 0 ? Array.Empty<V[]>() : new V[len][];
        foreach (var item in varsOfDim)
        {
            var var = (Var1)item.Value;
            GetConcrVars.Add(var.Key, (dim, i));
            Vars1[i++] = NewVar1(model, var);
        }

        dim = 2;
        i = 0;
        varsOfDim = vars.Where(x => x.Value.Dim == dim);
        len = varsOfDim.Count();
        Vars2 = len == 0 ? Array.Empty<V[][]>() : new V[len][][];
        foreach (var item in varsOfDim)
        {
            var var = (IVar2)item.Value;
            GetConcrVars.Add(var.Key, (dim, i));
            Vars2[i++] = var switch
            {
                Var2 sq => NewVar2(model, sq),
                JagVar2 jag => NewJagVar2(model, jag),
                _ => throw new Exception(),
            };
        }

        dim = 3;
        i = 0;
        varsOfDim = vars.Where(x => x.Value.Dim == dim);
        len = varsOfDim.Count();
        Vars3 = len == 0 ? Array.Empty<V[][][]>() : new V[len][][][];
        foreach (var item in varsOfDim)
        {
            var var = (IVar3)item.Value;
            GetConcrVars.Add(var.Key, (dim, i));
            Vars3[i++] = var switch
            {
                Var3 sq => NewVar3(model, sq),
                JagVar3 jag => NewJagVar3(model, jag),
                _ => throw new Exception(),
            };
        }

        dim = 4;
        i = 0;
        varsOfDim = vars.Where(x => x.Value.Dim == dim);
        len = varsOfDim.Count();
        Vars4 = len == 0 ? Array.Empty<V[][][][]>() : new V[len][][][][];
        foreach (var item in varsOfDim)
        {
            var var = (IVar4)item.Value;
            GetConcrVars.Add(var.Key, (dim, i));
            Vars4[i++] = var switch
            {
                Var4 sq => NewVar4(model, sq),
                JagVar4 jag => NewJagVar4(model, jag),
                _ => throw new Exception(),
            };
        }
    }
    ExprHandler<C, V> GetExprHandler(Expr expr)
    {
        Action<C, int[]> handleTerms = (c, ijk) => { };

        foreach (var term in expr.Terms)
        {
            (int dim, int ind) = GetConcrVars[term.Var.Key];
            if (dim == 0)
            {
                var var = (Var0)term.Var;
                var concVar = Vars0[ind];
                handleTerms += (c, ijk) => AddTermToConstr(c, term.Coef(ijk), concVar);
            }
            else if (dim == 1)
            {
                var var = (Var1)term.Var;
                var concVar = Vars1[ind];
                Sca i = term.Indices[0];
                handleTerms += (c, ijk) => AddTermToConstr(c, term.Coef(ijk),
                    concVar[(int)i.Value(ijk)]);
            }
            else if (dim == 2)
            {
                var var = (IVar2)term.Var;
                var concVar = Vars2[ind];
                Sca i = term.Indices[0], j = term.Indices[1];
                handleTerms += (c, ijk) => AddTermToConstr(c, term.Coef(ijk),
                    concVar[(int)i.Value(ijk)][(int)j.Value(ijk)]);
            }
            else if (dim == 3)
            {
                var var = (IVar3)term.Var;
                var concVar = Vars3[ind];
                Sca i = term.Indices[0], j = term.Indices[1], k = term.Indices[2];
                handleTerms += (c, ijk) => AddTermToConstr(c, term.Coef(ijk),
                    concVar[(int)i.Value(ijk)][(int)j.Value(ijk)][(int)k.Value(ijk)]);
            }
            else if (dim == 4)
            {
                var var = (IVar4)term.Var;
                var concVar = Vars4[ind];
                Sca i = term.Indices[0], j = term.Indices[1], k = term.Indices[2], l = term.Indices[3];
                handleTerms += (c, ijk) => AddTermToConstr(c, term.Coef(ijk),
                    concVar[(int)i.Value(ijk)][(int)j.Value(ijk)][(int)k.Value(ijk)][(int)l.Value(ijk)]);
            }
            else
                throw new NotImplementedException();
        }

        var sumFun = expr.Sums.Length == 0 ? Array.Empty<ExprHandler<C, V>>() : new ExprHandler<C, V>[expr.Sums.Length];
        for (int i = 0; i < expr.Sums.Length; i++)
            sumFun[i] = GetExprHandler(expr.Sums[i].Expr);

        Func<C, int[], double> handle;
        if (expr.Sca.IsNone)
            handle = (con, ijk) => { handleTerms(con, ijk); return 0; };
        else
        {
            var sca = expr.Sca.Unwrap();
            handle = (con, ijk) => { handleTerms(con, ijk); return sca.Value(ijk); };
        }

        return new() { Handle = handle, Sums = sumFun };
    }


    // method - abstract
    protected internal abstract M NewModel(Model abstractModel);
    protected internal abstract C NewConstraint(M model);
    protected internal abstract V NewVar0(M model, Var0 var);
    protected internal abstract V[] NewVar1(M model, Var1 var);
    protected internal abstract V[][] NewVar2(M model, Var2 var);
    protected internal abstract V[][] NewJagVar2(M model, JagVar2 var);
    protected internal abstract V[][][] NewVar3(M model, Var3 var);
    protected internal abstract V[][][] NewJagVar3(M model, JagVar3 var);
    protected internal abstract V[][][][] NewVar4(M model, Var4 var);
    protected internal abstract V[][][][] NewJagVar4(M model, JagVar4 var);
    protected internal abstract void AddTermToConstr(C constraint, double coefficient, V var);
    protected internal abstract void AddConstrToModel(Constr constrExpr, int[] indices, M model, C constraint, double rhs);
    protected internal abstract void AddObjectiveToModel((string Key, ObjType Type, Expr Expr) objective, M model, C obj);
    protected internal abstract void AddVarBounds(M model, Dictionary<string, IVar> vars);

    
    // method - get
    public V Var0(string key)
    {
        (int dim, int ind) = GetConcrVars[key];
        // todo: dim==0
        return Vars0[ind];
    }
    public V[] Var1(string key)
    {
        (int dim, int ind) = GetConcrVars[key];
        // todo: dim==1
        return Vars1[ind];
    }
    public V[][] Var2(string key)
    {
        (int dim, int ind) = GetConcrVars[key];
        // todo: dim==2
        return Vars2[ind];
    }
    public V[][][] Var3(string key)
    {
        (int dim, int ind) = GetConcrVars[key];
        // todo: dim==2
        return Vars3[ind];
    }
    

    // method - build
    public M Build(Model model)
    {
        M m = NewModel(model);
        BuiltModel = m;
        SetHandlers(model, m);
        ijk = new int[model.Sets.Count];
        HandleObj(model.Objective, ijk, this, m, model.Objective.Expr.Sums, Handlers[model.Objective.Key]);
        foreach (var constraint in model.Constraints.Values)
        {
            Array.Clear(ijk);
            var h = Handlers[constraint.Key];
            var sets = constraint.Sets;
            var sums = constraint.Expr.LhsMinRhs.Sums;
            switch (sets.Length)
            {
                case 0: Handle0(constraint, ijk, this, m, sums, h, default); break;
                case 1: Handle1(constraint, ijk, this, m, sums, h, default, sets[0]); break;
                case 2: Handle2(constraint, ijk, this, m, sums, h, default, sets[0], sets[1]); break;
                case 3: Handle3(constraint, ijk, this, m, sums, h, default, sets[0], sets[1], sets[2]); break;
                case 4: Handle4(constraint, ijk, this, m, sums, h, default, sets[0], sets[1], sets[2], sets[3]); break;
                default: throw new Exception();
            }
        }
        AddVarBounds(m, model.Variables);
        return m;
    }
    public void AddOrUpdateConstraint(Constr constraint)
    {
        if (BuiltModel.IsNone)
            throw new Exception();
        
        AddOrUpdateHandler(constraint);
        var m = BuiltModel.Unwrap();
        Array.Clear(ijk);
        var h = Handlers[constraint.Key];
        var sets = constraint.Sets;
        var sums = constraint.Expr.LhsMinRhs.Sums;
        switch (sets.Length)
        {
            case 0: Handle0(constraint, ijk, this, m, sums, h, default); break;
            case 1: Handle1(constraint, ijk, this, m, sums, h, default, sets[0]); break;
            case 2: Handle2(constraint, ijk, this, m, sums, h, default, sets[0], sets[1]); break;
            case 3: Handle3(constraint, ijk, this, m, sums, h, default, sets[0], sets[1], sets[2]); break;
            case 4: Handle4(constraint, ijk, this, m, sums, h, default, sets[0], sets[1], sets[2], sets[3]); break;
            default: throw new Exception();
        }
    }
    // method - build - helper
    static void HandleObj((string Key, ObjType Type, Expr Expr) objective,
                            int[] ijk, ModelBuilder<M, C, V> builder, M model,
                            OptList<Sum> sums, ExprHandler<C, V> handler)
    {
        var obj = builder.NewConstraint(model);
        double _excess = handler.Handle(obj, ijk);
        int s = 0;
        foreach (var sum in sums)
        {
            var h = handler.Sums[s++];
            var sets = sum.Sets;
            _excess += sets.Length switch
            {
                0 => Handle0(default, ijk, builder, model, sum.Expr.Sums, h, obj),
                1 => Handle1(default, ijk, builder, model, sum.Expr.Sums, h, obj, sets[0]),
                2 => Handle2(default, ijk, builder, model, sum.Expr.Sums, h, obj, sets[0], sets[1]),
                3 => Handle3(default, ijk, builder, model, sum.Expr.Sums, h, obj, sets[0], sets[1], sets[2]),
                4 => Handle4(default, ijk, builder, model, sum.Expr.Sums, h, obj, sets[0], sets[1], sets[2], sets[3]),
                _ => throw new Exception(),
            };
        }
        builder.AddObjectiveToModel(objective, model, obj);
        // todo: log excess?
    }
    static double Handle0(Opt<Constr> constr, int[] ijk, ModelBuilder<M, C, V> builder, M model,
                            OptList<Sum> sums, ExprHandler<C, V> handler, Opt<C> maybeCon)
    {
        bool newCon = maybeCon.IsNone;
        C con = newCon ? builder.NewConstraint(model) : maybeCon.Unwrap();
        double rhs = handler.Handle(con, ijk);
        int s = 0;
        foreach (var sum in sums)
        {
            var h = handler.Sums[s++];
            var sets = sum.Sets;
            rhs += sets.Length switch
            {
                0 => Handle0(default, ijk, builder, model, sum.Expr.Sums, h, con),
                1 => Handle1(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0]),
                2 => Handle2(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0], sets[1]),
                3 => Handle3(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0], sets[1], sets[2]),
                4 => Handle4(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0], sets[1], sets[2], sets[3]),
                _ => throw new Exception(),
            };
        }
        if (newCon)
            builder.AddConstrToModel(constr.Unwrap(), ijk, model, con, -rhs);
        return rhs;
    }
    static double Handle1(Opt<Constr> constr, int[] ijk, ModelBuilder<M, C, V> builder, M model,
                            OptList<Sum> sums, ExprHandler<C, V> handler, Opt<C> maybeCon,
                            Set set1)
    {
        double outerRhs = 0;
        bool newCon = maybeCon.IsNone;
        var ind1 = set1.IndexInModel;
        var gen1 = set1.Generator(ijk);
        foreach (var i1 in gen1)
        {
            ijk[ind1] = i1;
            C con = newCon ? builder.NewConstraint(model) : maybeCon.Unwrap();
            double rhs = handler.Handle(con, ijk);
            int s = 0;
            foreach (var sum in sums)
            {
                var h = handler.Sums[s++];
                var sets = sum.Sets;
                rhs += sets.Length switch
                {
                    0 => Handle0(default, ijk, builder, model, sum.Expr.Sums, h, con),
                    1 => Handle1(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0]),
                    2 => Handle2(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0], sets[1]),
                    3 => Handle3(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0], sets[1], sets[2]),
                    4 => Handle4(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0], sets[1], sets[2], sets[3]),
                    _ => throw new Exception(),
                };
            }
            if (newCon)
                builder.AddConstrToModel(constr.Unwrap(), ijk, model, con, -rhs);
            else
                outerRhs += rhs;
        }
        return outerRhs;
    }
    static double Handle2(Opt<Constr> constr, int[] ijk, ModelBuilder<M, C, V> builder, M model,
                            OptList<Sum> sums, ExprHandler<C, V> handler, Opt<C> maybeCon,
                            Set set1, Set set2)
    {
        double outerRhs = 0;
        bool newCon = maybeCon.IsNone;
        var ind1 = set1.IndexInModel;
        var ind2 = set2.IndexInModel;
        var gen1 = set1.Generator(ijk);
        foreach (var i1 in gen1)
        {
            ijk[ind1] = i1;
            var gen2 = set2.Generator(ijk);
            foreach (var i2 in gen2)
            {
                ijk[ind2] = i2;
                C con = newCon ? builder.NewConstraint(model) : maybeCon.Unwrap();
                double rhs = handler.Handle(con, ijk);
                int s = 0;
                foreach (var sum in sums)
                {
                    var h = handler.Sums[s++];
                    var sets = sum.Sets;
                    rhs += sets.Length switch
                    {
                        0 => Handle0(default, ijk, builder, model, sum.Expr.Sums, h, con),
                        1 => Handle1(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0]),
                        2 => Handle2(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0], sets[1]),
                        3 => Handle3(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0], sets[1], sets[2]),
                        4 => Handle4(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0], sets[1], sets[2], sets[3]),
                        _ => throw new Exception(),
                    };
                }
                if (newCon)
                    builder.AddConstrToModel(constr.Unwrap(), ijk, model, con, -rhs);
                else
                    outerRhs += rhs;
            }
        }
        return outerRhs;
    }
    static double Handle3(Opt<Constr> constr, int[] ijk, ModelBuilder<M, C, V> builder, M model,
                            OptList<Sum> sums, ExprHandler<C, V> handler, Opt<C> maybeCon,
                            Set set1, Set set2, Set set3)
    {
        double outerRhs = 0;
        bool newCon = maybeCon.IsNone;
        var ind1 = set1.IndexInModel;
        var ind2 = set2.IndexInModel;
        var ind3 = set3.IndexInModel;
        var gen1 = set1.Generator(ijk);
        foreach (var i1 in gen1)
        {
            ijk[ind1] = i1;
            var gen2 = set2.Generator(ijk);
            foreach (var i2 in gen2)
            {
                ijk[ind2] = i2;
                var gen3 = set3.Generator(ijk);
                foreach (var i3 in gen3)
                {
                    ijk[ind3] = i3;
                    C con = newCon ? builder.NewConstraint(model) : maybeCon.Unwrap();
                    double rhs = handler.Handle(con, ijk);
                    int s = 0;
                    foreach (var sum in sums)
                    {
                        var h = handler.Sums[s++];
                        var sets = sum.Sets;
                        rhs += sets.Length switch
                        {
                            0 => Handle0(default, ijk, builder, model, sum.Expr.Sums, h, con),
                            1 => Handle1(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0]),
                            2 => Handle2(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0], sets[1]),
                            3 => Handle3(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0], sets[1], sets[2]),
                            4 => Handle4(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0], sets[1], sets[2], sets[3]),
                            _ => throw new Exception(),
                        };
                    }
                    if (newCon)
                        builder.AddConstrToModel(constr.Unwrap(), ijk, model, con, -rhs);
                    else
                        outerRhs += rhs;
                }
            }
        }
        return outerRhs;
    }
    static double Handle4(Opt<Constr> constr, int[] ijk, ModelBuilder<M, C, V> builder, M model,
                           OptList<Sum> sums, ExprHandler<C, V> handler, Opt<C> maybeCon,
                           Set set1, Set set2, Set set3, Set set4)
    {
        double outerRhs = 0;
        bool newCon = maybeCon.IsNone;
        var ind1 = set1.IndexInModel;
        var ind2 = set2.IndexInModel;
        var ind3 = set3.IndexInModel;
        var ind4 = set4.IndexInModel;
        var gen1 = set1.Generator(ijk);
        foreach (var i1 in gen1)
        {
            ijk[ind1] = i1;
            var gen2 = set2.Generator(ijk);
            foreach (var i2 in gen2)
            {
                ijk[ind2] = i2;
                var gen3 = set3.Generator(ijk);
                foreach (var i3 in gen3)
                {
                    ijk[ind3] = i3;
                    var gen4 = set4.Generator(ijk);
                    foreach (var i4 in gen4)
                    {
                        ijk[ind4] = i4;
                        C con = newCon ? builder.NewConstraint(model) : maybeCon.Unwrap();
                        double rhs = handler.Handle(con, ijk);
                        int s = 0;
                        foreach (var sum in sums)
                        {
                            var h = handler.Sums[s++];
                            var sets = sum.Sets;
                            rhs += sets.Length switch
                            {
                                0 => Handle0(default, ijk, builder, model, sum.Expr.Sums, h, con),
                                1 => Handle1(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0]),
                                2 => Handle2(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0], sets[1]),
                                3 => Handle3(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0], sets[1], sets[2]),
                                4 => Handle4(default, ijk, builder, model, sum.Expr.Sums, h, con, sets[0], sets[1], sets[2], sets[3]),
                                _ => throw new Exception(),
                            };
                        }
                        if (newCon)
                            builder.AddConstrToModel(constr.Unwrap(), ijk, model, con, -rhs);
                        else
                            outerRhs += rhs;
                    }
                }
            }
        }
        return outerRhs;
    }


    // method
    protected internal abstract bool Solve(M builtModel);


    // method - get - var
    public V GetVar0(string key)
        => Vars0[GetConcrVars[key].Ind];
    public V[] GetVar1(string key)
        => Vars1[GetConcrVars[key].Ind];
    public V[][] GetVar2(string key)
        => Vars2[GetConcrVars[key].Ind];
    public V[][][] GetVar3(string key)
        => Vars3[GetConcrVars[key].Ind];
    public V[][][][] GetVar4(string key)
            => Vars4[GetConcrVars[key].Ind];
    public V[][] GetJagVar2(string key)
        => Vars2[GetConcrVars[key].Ind];
    public V[][][] GetJagVar3(string key)
        => Vars3[GetConcrVars[key].Ind];
    public V[][][][] GetJagVar4(string key)
            => Vars4[GetConcrVars[key].Ind];
    public V GetVar0(Var0 var)
        => Vars0[GetConcrVars[var.Key].Ind];
    public V[] GetVar1(Var1 var)
        => Vars1[GetConcrVars[var.Key].Ind];
    public V[][] GetVar2(Var2 var)
        => Vars2[GetConcrVars[var.Key].Ind];
    public V[][][] GetVar3(Var3 var)
        => Vars3[GetConcrVars[var.Key].Ind];
    public V[][][][] GetVar4(Var4 var)
            => Vars4[GetConcrVars[var.Key].Ind];
    public V[][] GetJagVar2(JagVar2 var)
        => Vars2[GetConcrVars[var.Key].Ind];
    public V[][][] GetJagVar3(JagVar3 var)
        => Vars3[GetConcrVars[var.Key].Ind];
    public V[][][][] GetJagVar4(JagVar4 var)
            => Vars4[GetConcrVars[var.Key].Ind];


    // method - get - value
    public abstract double GetVal0(string keyVar0);
    public abstract double[] GetVal1(string keyVar1);
    public abstract double[][] GetVal2(string keyVar2);
    public abstract double[][] GetJagVal2(string keyJagVar2);
    public abstract double[][][] GetVal3(string keyVar3);
    public abstract double[][][] GetJagVal3(string keyJagVar3);
    public abstract double[][][][] GetVal4(string keyVar4);
    public abstract double[][][][] GetJagVal4(string keyJagVar4);
    public double GetVal0(Var0 var) => GetVal0(var.Key);
    public double[] GetVal1(Var1 var) => GetVal1(var.Key);
    public double[][] GetVal2(Var2 var) => GetVal2(var.Key);
    public double[][] GetJagVal2(Var2 var) => GetJagVal2(var.Key);
    public double[][][] GetVal3(Var3 var) => GetVal3(var.Key);
    public double[][][] GetJagVal3(Var3 var) => GetJagVal3(var.Key);
    public double[][][][] GetVal4(Var4 var) => GetVal4(var.Key);
    public double[][][][] GetJagVal4(Var4 var) => GetJagVal4(var.Key);
}
