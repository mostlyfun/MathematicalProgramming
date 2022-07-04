namespace MathematicalProgramming;

public class Model
{
    // data
    public readonly string Name;
    protected internal Dictionary<string, Set> Sets;
    protected internal Dictionary<string, Constr> Constraints;
    protected internal Dictionary<string, IVar> Variables;
    protected internal (string Key, ObjType Type, Expr Expr) Objective;
    // ctor
    public Model(string name = "anonymous-model")
    {
        Name = name;
        Sets = new();
        Constraints = new();
        Variables = new();
        Objective = ("min0", ObjType.Minimize, 0);
    }
    public override string ToString()
    {
        StringBuilder sb = new();
        sb.Append(nameof(Model)).Append(": ").AppendLine(Name);
        sb.AppendLine("Sets");
        foreach (var item in Sets)
            sb.Append("  ").Append(item.Key).Append(": ").AppendLine(item.Value.ToString());
        sb.AppendLine("Variables");
        foreach (var item in Variables)
            sb.Append("  ").Append(item.Key).Append(": ").AppendLine(item.Value.ToString());
        sb.AppendLine("Constraints");
        foreach (var item in Constraints)
            sb.Append("  ").AppendLine(item.Value.ToString());
        return sb.ToString();
    }


    // method - var0
    string VarKey() => string.Format("_VAR_{0}", Variables.Count);
    Var0 Var0(string key, VarType varType, double lb, double ub)
    {
        Var0 var = new(key, varType, lb, ub);
        Variables.Add(key, var);
        // todo: check & validate
        return var;
    }
    public Var0 Bin0()
        => Var0(VarKey(), VarType.Binary, 0, 1);
    public Var0 Int0(int lb = 0, int ub = int.MaxValue)
        => Var0(VarKey(), VarType.Integer, lb, ub);
    public Var0 Cont0(double lb = 0, double ub = double.MaxValue)
        => Var0(VarKey(), VarType.Continuous, lb, ub);
    public Var0 Frac0()
        => Cont0(0, 1);
    public Var0 Bin0(string key)
        => Var0(key, VarType.Binary, 0, 1);
    public Var0 Int0(string key, int lb = 0, int ub = int.MaxValue)
        => Var0(key, VarType.Integer, lb, ub);
    public Var0 Cont0(string key, double lb = 0, double ub = double.MaxValue)
        => Var0(key, VarType.Continuous, lb, ub);
    public Var0 Frac0(string key)
        => Cont0(key, 0, 1);

    // method - var1
    Var1 Var1(string key, VarType varType, int len1, double lb, double ub)
    {
        Var1 var = new(key, varType, len1, lb, ub);
        Variables.Add(key, var);
        // todo: check validate
        return var;
    }
    Var1 Var1(string key, VarType varType, int len1, Func<int, double> getLb, Func<int, double> getUb)
    {
        Var1 var = new(key, varType, len1, getLb, getUb);
        Variables.Add(key, var);
        // todo: check validate
        return var;
    }
    public Var1 Bin1(int len1)
        => Var1(VarKey(), VarType.Binary, len1, 0, 1);
    public Var1 Int1(int len1, int lb = 0, int ub = int.MaxValue)
        => Var1(VarKey(), VarType.Integer, len1, lb, ub);
    public Var1 Cont1(int len1, double lb = 0, double ub = double.MaxValue)
        => Var1(VarKey(), VarType.Continuous, len1, lb, ub);
    public Var1 Cont1(int len1, Func<int, double> getLb, Func<int, double> getUb)
        => Var1(VarKey(), VarType.Continuous, len1, getLb, getUb);
    public Var1 Frac1(int len1)
        => Cont1(len1, 0, 1);
    public Var1 Bin1(string key, int len1)
        => Var1(key, VarType.Binary, len1, 0, 1);
    public Var1 Int1(string key, int len1, int lb = 0, int ub = int.MaxValue)
        => Var1(key, VarType.Integer, len1, lb, ub);
    public Var1 Cont1(string key, int len1, double lb = 0, double ub = double.MaxValue)
        => Var1(key, VarType.Continuous, len1, lb, ub);
    public Var1 Cont1(string key, int len1, Func<int, double> getLb, Func<int, double> getUb)
        => Var1(key, VarType.Continuous, len1, getLb, getUb);
    public Var1 Frac1(string key, int len1)
        => Cont1(key, len1, 0, 1);

    // method - var2
    Var2 Var2(string key, VarType varType, int len1, int len2, double lb, double ub)
    {
        Var2 var = new(key, varType, len1, len2, lb, ub);
        Variables.Add(key, var);
        // todo: check validate
        return var;
    }
    Var2 Var2(string key, VarType varType, int len1, int len2, Func<int, int, double> getLb, Func<int, int, double> getUb)
    {
        Var2 var = new(key, varType, len1, len2, getLb, getUb);
        Variables.Add(key, var);
        // todo: check validate
        return var;
    }
    public Var2 Bin2(int len1, int len2)
        => Var2(VarKey(), VarType.Binary, len1, len2, 0, 1);
    public Var2 Int2(int len1, int len2, int lb, int ub)
        => Var2(VarKey(), VarType.Integer, len1, len2, lb, ub);
    public Var2 Cont2(int len1, int len2, double lb, double ub)
        => Var2(VarKey(), VarType.Continuous, len1, len2, lb, ub);
    public Var2 Cont2(int len1, int len2, Func<int, int, double> getLb, Func<int, int, double> getUb)
        => Var2(VarKey(), VarType.Continuous, len1, len2, getLb, getUb);
    public Var2 Frac2(int len1, int len2)
        => Cont2(len1, len2, 0, 1);
    public Var2 Bin2(string key, int len1, int len2)
        => Var2(key, VarType.Binary, len1, len2, 0, 1);
    public Var2 Int2(string key, int len1, int len2, int lb, int ub)
        => Var2(key, VarType.Integer, len1, len2, lb, ub);
    public Var2 Cont2(string key, int len1, int len2, double lb, double ub)
        => Var2(key, VarType.Continuous, len1, len2, lb, ub);
    public Var2 Cont2(string key, int len1, int len2, Func<int, int, double> getLb, Func<int, int, double> getUb)
        => Var2(key, VarType.Continuous, len1, len2, getLb, getUb);
    public Var2 Frac2(string key, int len1, int len2)
        => Cont2(key, len1, len2, 0, 1);

    // method - JagVar2
    JagVar2 VarJag2(string key, VarType varType, int len1, Func<int, int> getLen2, double lb, double ub)
    {
        JagVar2 var = new(key, varType, len1, getLen2, lb, ub);
        Variables.Add(key, var);
        // todo: check validate
        return var;
    }
    JagVar2 VarJag2(string key, VarType varType, int len1, Func<int, int> getLen2, Func<int, int, double> getLb, Func<int, int, double> getUb)
    {
        JagVar2 var = new(key, varType, len1, getLen2, getLb, getUb);
        Variables.Add(key, var);
        // todo: check validate
        return var;
    }
    public JagVar2 Bin2(int len1, Func<int, int> getLen2)
        => VarJag2(VarKey(), VarType.Binary, len1, getLen2, 0, 1);
    public JagVar2 Int2(int len1, Func<int, int> getLen2, int lb, int ub)
        => VarJag2(VarKey(), VarType.Integer, len1, getLen2, lb, ub);
    public JagVar2 Cont2(int len1, Func<int, int> getLen2, double lb, double ub)
        => VarJag2(VarKey(), VarType.Continuous, len1, getLen2, lb, ub);
    public JagVar2 Cont2(int len1, Func<int, int> getLen2, Func<int, int, double> getLb, Func<int, int, double> getUb)
        => VarJag2(VarKey(), VarType.Continuous, len1, getLen2, getLb, getUb);
    public JagVar2 Frac2(int len1, Func<int, int> getLen2)
        => Cont2(len1, getLen2, 0, 1);
    public JagVar2 Bin2(string key, int len1, Func<int, int> getLen2)
        => VarJag2(key, VarType.Binary, len1, getLen2, 0, 1);
    public JagVar2 Int2(string key, int len1, Func<int, int> getLen2, int lb, int ub)
        => VarJag2(key, VarType.Integer, len1, getLen2, lb, ub);
    public JagVar2 Cont2(string key, int len1, Func<int, int> getLen2, double lb, double ub)
        => VarJag2(key, VarType.Continuous, len1, getLen2, lb, ub);
    public JagVar2 Cont2(string key, int len1, Func<int, int> getLen2, Func<int, int, double> getLb, Func<int, int, double> getUb)
        => VarJag2(key, VarType.Continuous, len1, getLen2, getLb, getUb);
    public JagVar2 Frac2(string key, int len1, Func<int, int> getLen2)
        => Cont2(key, len1, getLen2, 0, 1);

    // method - var3
    Var3 Var3(string key, VarType varType, int len1, int len2, int len3, double lb, double ub)
    {
        Var3 var = new(key, varType, len1, len2, len3, lb, ub);
        Variables.Add(key, var);
        // todo: check validate
        return var;
    }
    Var3 Var3(string key, VarType varType, int len1, int len2, int len3, Func<int, int, int, double> getLb, Func<int, int, int, double> getUb)
    {
        Var3 var = new(key, varType, len1, len2, len3, getLb, getUb);
        Variables.Add(key, var);
        // todo: check validate
        return var;
    }
    public Var3 Bin3(int len1, int len2, int len3)
        => Var3(VarKey(), VarType.Binary, len1, len2, len3, 0, 1);
    public Var3 Int3(int len1, int len2, int len3, int lb, int ub)
        => Var3(VarKey(), VarType.Integer, len1, len2, len3, lb, ub);
    public Var3 Cont3(int len1, int len2, int len3, double lb, double ub)
        => Var3(VarKey(), VarType.Continuous, len1, len2, len3, lb, ub);
    public Var3 Cont3(int len1, int len2, int len3, Func<int, int, int, double> getLb, Func<int, int, int, double> getUb)
        => Var3(VarKey(), VarType.Continuous, len1, len2, len3, getLb, getUb);
    public Var3 Frac3(int len1, int len2, int len3)
        => Cont3(len1, len2, len3, 0, 1);
    public Var3 Bin3(string key, int len1, int len2, int len3)
        => Var3(key, VarType.Binary, len1, len2, len3, 0, 1);
    public Var3 Int3(string key, int len1, int len2, int len3, int lb, int ub)
        => Var3(key, VarType.Integer, len1, len2, len3, lb, ub);
    public Var3 Cont3(string key, int len1, int len2, int len3, double lb, double ub)
        => Var3(key, VarType.Continuous, len1, len2, len3, lb, ub);
    public Var3 Cont3(string key, int len1, int len2, int len3, Func<int, int, int, double> getLb, Func<int, int, int, double> getUb)
        => Var3(key, VarType.Continuous, len1, len2, len3, getLb, getUb);
    public Var3 Frac3(string key, int len1, int len2, int len3)
        => Cont3(key, len1, len2, len3, 0, 1);

    // method - JagVar3
    JagVar3 JagVar3(string key, VarType varType, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, double lb, double ub)
    {
        JagVar3 var = new(key, varType, len1, getLen2, getLen3, lb, ub);
        Variables.Add(key, var);
        // todo: check validate
        return var;
    }
    JagVar3 JagVar3(string key, VarType varType, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, double> getLb, Func<int, int, int, double> getUb)
    {
        JagVar3 var = new(key, varType, len1, getLen2, getLen3, getLb, getUb);
        Variables.Add(key, var);
        // todo: check validate
        return var;
    }
    public JagVar3 Bin3(int len1, Func<int, int> getLen2, Func<int, int, int> getLen3)
        => JagVar3(VarKey(), VarType.Binary, len1, getLen2, getLen3, 0, 1);
    public JagVar3 Int3(int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, int lb, int ub)
        => JagVar3(VarKey(), VarType.Integer, len1, getLen2, getLen3, lb, ub);
    public JagVar3 Cont3(int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, double lb, double ub)
        => JagVar3(VarKey(), VarType.Continuous, len1, getLen2, getLen3, lb, ub);
    public JagVar3 Cont3(int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, double> getLb, Func<int, int, int, double> getUb)
        => JagVar3(VarKey(), VarType.Continuous, len1, getLen2, getLen3, getLb, getUb);
    public JagVar3 Frac3(int len1, Func<int, int> getLen2, Func<int, int, int> getLen3)
       => Cont3(len1, getLen2, getLen3, 0, 1);
    public JagVar3 Bin3(string key, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3)
        => JagVar3(key, VarType.Binary, len1, getLen2, getLen3, 0, 1);
    public JagVar3 Int3(string key, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, int lb, int ub)
        => JagVar3(key, VarType.Integer, len1, getLen2, getLen3, lb, ub);
    public JagVar3 Cont3(string key, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, double lb, double ub)
        => JagVar3(key, VarType.Continuous, len1, getLen2, getLen3, lb, ub);
    public JagVar3 Cont3(string key, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, double> getLb, Func<int, int, int, double> getUb)
        => JagVar3(key, VarType.Continuous, len1, getLen2, getLen3, getLb, getUb);
    public JagVar3 Frac3(string key, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3)
       => Cont3(key, len1, getLen2, getLen3, 0, 1);

    // method - var4
    Var4 Var4(string key, VarType varType, int len1, int len2, int len3, int len4, double lb, double ub)
    {
        Var4 var = new(key, varType, len1, len2, len3, len4, lb, ub);
        Variables.Add(key, var);
        // todo: check validate
        return var;
    }
    Var4 Var4(string key, VarType varType, int len1, int len2, int len3, int len4, Func<int, int, int, int, double> getLb, Func<int, int, int, int, double> getUb)
    {
        Var4 var = new(key, varType, len1, len2, len3, len4, getLb, getUb);
        Variables.Add(key, var);
        // todo: check validate
        return var;
    }
    public Var4 Bin4(int len1, int len2, int len3, int len4)
         => Var4(VarKey(), VarType.Binary, len1, len2, len3, len4, 0, 1);
    public Var4 Int4(int len1, int len2, int len3, int len4, int lb, int ub)
        => Var4(VarKey(), VarType.Integer, len1, len2, len3, len4, lb, ub);
    public Var4 Cont4(int len1, int len2, int len3, int len4, double lb, double ub)
        => Var4(VarKey(), VarType.Continuous, len1, len2, len3, len4, lb, ub);
    public Var4 Cont4(int len1, int len2, int len3, int len4, Func<int, int, int, int, double> getLb, Func<int, int, int, int, double> getUb)
        => Var4(VarKey(), VarType.Continuous, len1, len2, len3, len4, getLb, getUb);
    public Var4 Frac4(int len1, int len2, int len3, int len4)
        => Cont4(len1, len2, len3, len4, 0, 1);
    public Var4 Bin4(string key, int len1, int len2, int len3, int len4)
        => Var4(key, VarType.Binary, len1, len2, len3, len4, 0, 1);
    public Var4 Int4(string key, int len1, int len2, int len3, int len4, int lb, int ub)
        => Var4(key, VarType.Integer, len1, len2, len3, len4, lb, ub);
    public Var4 Cont4(string key, int len1, int len2, int len3, int len4, double lb, double ub)
        => Var4(key, VarType.Continuous, len1, len2, len3, len4, lb, ub);
    public Var4 Cont4(string key, int len1, int len2, int len3, int len4, Func<int, int, int, int, double> getLb, Func<int, int, int, int, double> getUb)
        => Var4(key, VarType.Continuous, len1, len2, len3, len4, getLb, getUb);
    public Var4 Frac4(string key, int len1, int len2, int len3, int len4)
        => Cont4(key, len1, len2, len3, len4, 0, 1);

    // method - JagVar4
    JagVar4 JagVar4(string key, VarType varType, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, int> getLen4, double lb, double ub)
    {
        JagVar4 var = new(key, varType, len1, getLen2, getLen3, getLen4, lb, ub);
        Variables.Add(key, var);
        // todo: check validate
        return var;
    }
    JagVar4 JagVar4(string key, VarType varType, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, int> getLen4, Func<int, int, int, int, double> getLb, Func<int, int, int, int, double> getUb)
    {
        JagVar4 var = new(key, varType, len1, getLen2, getLen3, getLen4, getLb, getUb);
        Variables.Add(key, var);
        // todo: check validate
        return var;
    }
    public JagVar4 Bin4(int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, int> getLen4)
         => JagVar4(VarKey(), VarType.Binary, len1, getLen2, getLen3, getLen4, 0, 1);
    public JagVar4 Int4(int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, int> getLen4, int lb, int ub)
        => JagVar4(VarKey(), VarType.Integer, len1, getLen2, getLen3, getLen4, lb, ub);
    public JagVar4 Cont4(int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, int> getLen4, double lb, double ub)
        => JagVar4(VarKey(), VarType.Continuous, len1, getLen2, getLen3, getLen4, lb, ub);
    public JagVar4 Cont4(int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, int> getLen4, Func<int, int, int, int, double> getLb, Func<int, int, int, int, double> getUb)
        => JagVar4(VarKey(), VarType.Continuous, len1, getLen2, getLen3, getLen4, getLb, getUb);
    public JagVar4 Frac4(int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, int> getLen4)
       => Cont4(len1, getLen2, getLen3, getLen4, 0, 1);
    public JagVar4 Bin4(string key, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, int> getLen4)
        => JagVar4(key, VarType.Binary, len1, getLen2, getLen3, getLen4, 0, 1);
    public JagVar4 Int4(string key, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, int> getLen4, int lb, int ub)
        => JagVar4(key, VarType.Integer, len1, getLen2, getLen3, getLen4, lb, ub);
    public JagVar4 Cont4(string key, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, int> getLen4, double lb, double ub)
        => JagVar4(key, VarType.Continuous, len1, getLen2, getLen3, getLen4, lb, ub);
    public JagVar4 Cont4(string key, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, int> getLen4, Func<int, int, int, int, double> getLb, Func<int, int, int, int, double> getUb)
        => JagVar4(key, VarType.Continuous, len1, getLen2, getLen3, getLen4, getLb, getUb);
    public JagVar4 Frac4(string key, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, int> getLen4)
       => Cont4(key, len1, getLen2, getLen3, getLen4, 0, 1);


    // method - set
    string SetKey() => string.Format("_SET_{0}", Sets.Count);
    public Set Set(IEnumerable<int> generator)
        => Set(SetKey(), generator);
    public Set Set(int card)
        => Set(SetKey(), card);
    public Set Set(Set depSet, Func<int, IEnumerable<int>> generator)
        => Set(SetKey(), depSet, generator);
    public Set Set(Set depSet1, Set depSet2, Func<int, int, IEnumerable<int>> generator)
        => Set(SetKey(), depSet1, depSet2, generator);
    public Set Set(Set depSet1, Set depSet2, Set depSet3, Func<int, int, int, IEnumerable<int>> generator)
        => Set(SetKey(), depSet1, depSet2, depSet3, generator);
    public Set Set(string key, IEnumerable<int> generator)
    {
        Set set = new(this, Sets.Count, key, _ => generator);
        // todo: validate
        Sets.Add(key, set);
        return set;
    }
    public Set Set(string key, int card)
        => Set(key, Enumerable.Range(0, card));
    public Set Set(string key, Set depSet, Func<int, IEnumerable<int>> generator)
    {
        int i = depSet.IndexInModel;
        Set set = new(this, Sets.Count, key, ijk => generator(ijk[i]));
        // todo: validate
        Sets.Add(key, set);
        return set;
    }
    public Set Set(string key, Set depSet1, Set depSet2, Func<int, int, IEnumerable<int>> generator)
    {
        int i = depSet1.IndexInModel, j = depSet2.IndexInModel;
        Set set = new(this, Sets.Count, key, ijk => generator(ijk[i], ijk[j]));
        // todo: validate
        Sets.Add(key, set);
        return set;
    }
    public Set Set(string key, Set depSet1, Set depSet2, Set depSet3, Func<int, int, int, IEnumerable<int>> generator)
    {
        int i = depSet1.IndexInModel, j = depSet2.IndexInModel, k = depSet3.IndexInModel;
        Set set = new(this, Sets.Count, key, ijk => generator(ijk[i], ijk[j], ijk[k]));
        // todo: validate
        Sets.Add(key, set);
        return set;
    }


    // method - constraint
    public Constr this[string constraintKey]
    {
        get => Constraints[constraintKey];
        set
        {
            // todo: check & validate
            value.Key = constraintKey;
            Constraints[constraintKey] = value;
        }
    }
    public static Model operator +(Model model, Constr constr)
    {
        // todo: check & validate
        model.Constraints[ConKey()] = constr;
        return model;
    }


    // method - objective
    public void Obj(string objectiveName, ObjType objectiveType, Expr objectiveExpression)
        => Objective = (objectiveName, objectiveType, objectiveExpression);
    public void Obj(ObjType objectiveType, Expr objectiveExpression)
        => Objective = ("obj", objectiveType, objectiveExpression);


    // method - helpers
    public static string GetConstraintKey(Constr constr, int[] indices)
    {
        return string.Format("{0}({1})", constr.Key,
            string.Join(',', constr.Sets.Select(s => indices[s.IndexInModel])));
    }
    public static void AppendConstraintKey(StringBuilder stringBuilder, Constr constr, int[] indices)
    {
        bool started = false;
        stringBuilder.Append(constr.Key);
        foreach (var set in constr.Sets)
        {
            int ind = indices[set.IndexInModel];
            if (!started)
            {
                started = true;
                stringBuilder.Append('(').Append(ind);
            }
            else
                stringBuilder.Append(',').Append(ind);
        }
        if (started)
            stringBuilder.Append(')');
    }


    // method - solve
    public M Build<M, C, V>(ModelBuilder<M, C, V> builder)
        => builder.Build(this);
    public (M BuiltModel, bool IsSolved) BuildAndSolve<M,C,V>(ModelBuilder<M,C,V> builder)
    {
        var m = builder.Build(this);
        bool solved = builder.Solve(m);
        return (m, solved);
    }
    public bool Solve<M, C, V>(ModelBuilder<M, C, V> builder)
    {
        var m = builder.Build(this);
        return builder.Solve(m);
    }
}
