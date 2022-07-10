namespace MathematicalProgramming;

public class LpBuilder : ModelBuilder<StringBuilder, StringBuilder, string>
{
    protected internal override StringBuilder NewModel(Model abstractModel)
        => new(string.Format("\\Problem name: {0}{1}{1}", abstractModel.Name, Environment.NewLine));
    protected internal override StringBuilder NewConstraint(StringBuilder model)
        => new();
    protected internal override string NewVar0(StringBuilder model, Var0 var)
        => var.Key;
    protected internal override string[] NewVar1(StringBuilder model, Var1 var)
        => Enumerable.Range(0, var.Len1).Select(i => string.Format("{0}({1})", var.Key, i)).ToArray();
    protected internal override string[][] NewVar2(StringBuilder model, Var2 var)
    {
        return Enumerable.Range(0, var.Len1)
            .Select(i => Enumerable.Range(0, var.Len2)
            .Select(j => string.Format("{0}({1},{2})", var.Key, i, j))
            .ToArray())
            .ToArray();
    }
    protected internal override string[][] NewJagVar2(StringBuilder model, JagVar2 var)
    {
        return Enumerable.Range(0, var.Len1)
            .Select(i => Enumerable.Range(0, var.GetLen2(i))
            .Select(j => string.Format("{0}({1},{2})", var.Key, i, j))
            .ToArray())
            .ToArray();
    }
    protected internal override string[][][] NewVar3(StringBuilder model, Var3 var)
    {
        return Enumerable.Range(0, var.Len1)
            .Select(i => Enumerable.Range(0, var.Len2)
            .Select(j => Enumerable.Range(0, var.Len3)
            .Select(k => string.Format("{0}({1},{2},{3})", var.Key, i, j, k))
            .ToArray())
            .ToArray())
            .ToArray();
    }
    protected internal override string[][][] NewJagVar3(StringBuilder model, JagVar3 var)
    {
        return Enumerable.Range(0, var.Len1)
            .Select(i => Enumerable.Range(0, var.GetLen2(i))
            .Select(j => Enumerable.Range(0, var.GetLen3(i, j))
            .Select(k => string.Format("{0}({1},{2},{3})", var.Key, i, j, k))
            .ToArray())
            .ToArray())
            .ToArray();
    }
    protected internal override string[][][][] NewVar4(StringBuilder model, Var4 var)
    {
        return Enumerable.Range(0, var.Len1)
            .Select(i => Enumerable.Range(0, var.Len2)
            .Select(j => Enumerable.Range(0, var.Len3)
            .Select(k => Enumerable.Range(0, var.Len4)
            .Select(l => string.Format("{0}({1},{2},{3},{4})", var.Key, i, j, k, l))
            .ToArray())
            .ToArray())
            .ToArray())
            .ToArray();
    }
    protected internal override string[][][][] NewJagVar4(StringBuilder model, JagVar4 var)
    {
        return Enumerable.Range(0, var.Len1)
            .Select(i => Enumerable.Range(0, var.GetLen2(i))
            .Select(j => Enumerable.Range(0, var.GetLen3(i, j))
            .Select(k => Enumerable.Range(0, var.GetLen4(i, j, k))
            .Select(l => string.Format("{0}({1},{2},{3},{4})", var.Key, i, j, k, l))
            .ToArray())
            .ToArray())
            .ToArray())
            .ToArray();
    }
    protected internal override void AddTermToConstr(StringBuilder constraint, double coefficient, string var)
    {
        if (constraint.Length < 2)
        {
            if (coefficient == 1)
                constraint.Append(var);
            else if (coefficient == -1)
                constraint.Append('-').Append(var);
            else
                constraint.Append(coefficient).Append('*').Append(var);
        }
        else
        {
            if (coefficient == 1)
                constraint.Append(" + ").Append(var);
            else if (coefficient == -1)
                constraint.Append(" - ").Append(var);
            else if (coefficient < 0)
                constraint.Append(" - ").Append(-coefficient).Append('*').Append(var);
            else
                constraint.Append(" + ").Append(coefficient).Append('*').Append(var);
        }
    }
    protected internal override void AddConstrToModel(Constr constr, int[] indices, StringBuilder model, StringBuilder constraint, double rhs)
    {
        model.Append(' ');
        Model.AppendConstraintKey(model, constr, indices);
        model.Append(": ").Append(constraint);
        AppendRel(model, constr.Expr.Relation);
        model.AppendLine(rhs.ToString());
    }
    protected internal override void AddObjectiveToModel((string Key, ObjType Type, Expr Expr) objective, StringBuilder model, StringBuilder obj)
    {
        model.AppendLine(objective.Type.ToString());
        model.Append(' ').Append(objective.Key).Append(": ").AppendLine(obj.ToString());
        model.AppendLine("Subject To");
    }
    protected internal override void AddVarBounds(StringBuilder model, Dictionary<string, IVar> vars)
    {
        model.AppendLine("Bounds");
        foreach (var item in vars)
        {
            (int dim, int ind) = GetConcrVars[item.Key];
            switch (dim)
            {
                case 0:
                    var var0 = (Var0)item.Value;
                    model.Append(' ').Append(var0.Lb)
                        .Append(" <= ").Append(var0.Key)
                        .Append(" <= ").AppendLine(var0.Ub.ToString());
                    break;
                case 1:
                    var var1 = (Var1)item.Value;
                    if (var1.Lb.IsSome)
                    {
                        double lb = var1.Lb.Unwrap(), ub = var1.Ub.Unwrap();
                        for (int i = 0; i < var1.Len1; i++)
                            model.Append(' ').Append(lb)
                                    .Append(" <= ").Append(var1.Key).Append('(').Append(i).Append(')')
                                    .Append(" <= ").AppendLine(ub.ToString());
                    }
                    else
                    {
                        Func<int, double> lb = var1.GetLb.Unwrap(), ub = var1.GetUbs.Unwrap();
                        for (int i = 0; i < var1.Len1; i++)
                            model.Append(' ').Append(lb(i))
                                    .Append(" <= ").Append(var1.Key).Append('(').Append(i).Append(')')
                                    .Append(" <= ").AppendLine(ub(i).ToString());
                    }
                    break;
                case 2:
                    if (item.Value is Var2 var2)
                    {
                        if (var2.Lb.IsSome)
                        {
                            double lb = var2.Lb.Unwrap(), ub = var2.Ub.Unwrap();
                            for (int i = 0; i < var2.Len1; i++)
                                for (int j = 0; j < var2.Len2; j++)
                                    model.Append(' ').Append(lb)
                                            .Append(" <= ").Append(var2.Key).Append('(').Append(i).Append(',').Append(j).Append(')')
                                            .Append(" <= ").AppendLine(ub.ToString());
                        }
                        else
                        {
                            Func<int, int, double> lb = var2.GetLbs.Unwrap(), ub = var2.GetUbs.Unwrap();
                            for (int i = 0; i < var2.Len1; i++)
                                for (int j = 0; j < var2.Len2; j++)
                                    model.Append(' ').Append(lb(i, j))
                                            .Append(" <= ").Append(var2.Key).Append('(').Append(i).Append(',').Append(j).Append(')')
                                            .Append(" <= ").AppendLine(ub(i, j).ToString());
                        }
                    }
                    else if (item.Value is JagVar2 jagvar2)
                    {
                        if (jagvar2.Lb.IsSome)
                        {
                            double lb = jagvar2.Lb.Unwrap(), ub = jagvar2.Ub.Unwrap();
                            for (int i = 0; i < jagvar2.Len1; i++)
                                for (int j = 0; j < jagvar2.GetLen2(i); j++)
                                    model.Append(' ').Append(lb)
                                            .Append(" <= ").Append(jagvar2.Key).Append('(').Append(i).Append(',').Append(j).Append(')')
                                            .Append(" <= ").AppendLine(ub.ToString());
                        }
                        else
                        {
                            Func<int, int, double> lb = jagvar2.GetLbs.Unwrap(), ub = jagvar2.GetUbs.Unwrap();
                            for (int i = 0; i < jagvar2.Len1; i++)
                                for (int j = 0; j < jagvar2.GetLen2(i); j++)
                                    model.Append(' ').Append(lb(i, j))
                                            .Append(" <= ").Append(jagvar2.Key).Append('(').Append(i).Append(',').Append(j).Append(')')
                                            .Append(" <= ").AppendLine(ub(i, j).ToString());
                        }
                    }
                    else
                        throw new NotImplementedException();
                    break;
                case 3:
                    if (item.Value is Var3 var3)
                    {
                        if (var3.Lb.IsSome)
                        {
                            double lb = var3.Lb.Unwrap(), ub = var3.Ub.Unwrap();
                            for (int i = 0; i < var3.Len1; i++)
                                for (int j = 0; j < var3.Len2; j++)
                                    for (int k = 0; k < var3.Len3; k++)
                                        model.Append(' ').Append(lb)
                                                .Append(" <= ").Append(var3.Key).Append('(').Append(i).Append(',').Append(j).Append(',').Append(k).Append(')')
                                                .Append(" <= ").AppendLine(ub.ToString());
                        }
                        else
                        {
                            Func<int, int, int, double> lb = var3.GetLbs.Unwrap(), ub = var3.GetUbs.Unwrap();
                            for (int i = 0; i < var3.Len1; i++)
                                for (int j = 0; j < var3.Len2; j++)
                                    for (int k = 0; k < var3.Len3; k++)
                                        model.Append(' ').Append(lb(i, j, k))
                                                .Append(" <= ").Append(var3.Key).Append('(').Append(i).Append(',').Append(j).Append(',').Append(k).Append(')')
                                                .Append(" <= ").AppendLine(ub(i, j, k).ToString());
                        }
                    }
                    else if (item.Value is JagVar3 jagvar3)
                    {
                        if (jagvar3.Lb.IsSome)
                        {
                            double lb = jagvar3.Lb.Unwrap(), ub = jagvar3.Ub.Unwrap();
                            for (int i = 0; i < jagvar3.Len1; i++)
                                for (int j = 0; j < jagvar3.GetLen2(i); j++)
                                    for (int k = 0; k < jagvar3.GetLen3(i, j); k++)
                                        model.Append(' ').Append(lb)
                                                .Append(" <= ").Append(jagvar3.Key).Append('(').Append(i).Append(',').Append(j).Append(',').Append(k).Append(')')
                                                .Append(" <= ").AppendLine(ub.ToString());
                        }
                        else
                        {
                            Func<int, int, int, double> lb = jagvar3.GetLbs.Unwrap(), ub = jagvar3.GetUbs.Unwrap();
                            for (int i = 0; i < jagvar3.Len1; i++)
                                for (int j = 0; j < jagvar3.GetLen2(i); j++)
                                    for (int k = 0; k < jagvar3.GetLen3(i, j); k++)
                                        model.Append(' ').Append(lb(i, j, k))
                                                .Append(" <= ").Append(jagvar3.Key).Append('(').Append(i).Append(',').Append(j).Append(',').Append(k).Append(')')
                                                .Append(" <= ").AppendLine(ub(i, j, k).ToString());
                        }
                    }
                    else
                        throw new NotImplementedException();
                    break;
                case 4:
                    if (item.Value is Var4 var4)
                    {
                        if (var4.Lb.IsSome)
                        {
                            double lb = var4.Lb.Unwrap(), ub = var4.Ub.Unwrap();
                            for (int i = 0; i < var4.Len1; i++)
                                for (int j = 0; j < var4.Len2; j++)
                                    for (int k = 0; k < var4.Len3; k++)
                                        for (int l = 0; l < var4.Len4; l++)
                                            model.Append(' ').Append(lb)
                                                .Append(" <= ").Append(var4.Key).Append('(').Append(i).Append(',').Append(j).Append(',').Append(k).Append(')')
                                                .Append(l).Append(',')
                                                .Append(" <= ").AppendLine(ub.ToString());
                        }
                        else
                        {
                            Func<int, int, int, int, double> lb = var4.GetLbs.Unwrap(), ub = var4.GetUbs.Unwrap();
                            for (int i = 0; i < var4.Len1; i++)
                                for (int j = 0; j < var4.Len2; j++)
                                    for (int k = 0; k < var4.Len3; k++)
                                        for (int l = 0; l < var4.Len4; l++)
                                            model.Append(' ').Append(lb(i, j, k, l))
                                                .Append(" <= ").Append(var4.Key).Append('(').Append(i).Append(',').Append(j).Append(',').Append(k).Append(')')
                                                .Append(l).Append(',')
                                                .Append(" <= ").AppendLine(ub(i, j, k, l).ToString());
                        }
                    }
                    else if (item.Value is JagVar4 jagvar4)
                    {
                        if (jagvar4.Lb.IsSome)
                        {
                            double lb = jagvar4.Lb.Unwrap(), ub = jagvar4.Ub.Unwrap();
                            for (int i = 0; i < jagvar4.Len1; i++)
                                for (int j = 0; j < jagvar4.GetLen2(i); j++)
                                    for (int k = 0; k < jagvar4.GetLen3(i, j); k++)
                                        for (int l = 0; l < jagvar4.GetLen4(i, j, k); l++)
                                            model.Append(' ').Append(lb)
                                                .Append(" <= ").Append(jagvar4.Key).Append('(').Append(i).Append(',').Append(j).Append(',').Append(k).Append(')')
                                                .Append(l).Append(',')
                                                .Append(" <= ").AppendLine(ub.ToString());
                        }
                        else
                        {
                            Func<int, int, int, int, double> lb = jagvar4.GetLbs.Unwrap(), ub = jagvar4.GetUbs.Unwrap();
                            for (int i = 0; i < jagvar4.Len1; i++)
                                for (int j = 0; j < jagvar4.GetLen2(i); j++)
                                    for (int k = 0; k < jagvar4.GetLen3(i, j); k++)
                                        for (int l = 0; l < jagvar4.GetLen4(i, j, k); l++)
                                            model.Append(' ').Append(lb(i, j, k, l))
                                                .Append(" <= ").Append(jagvar4.Key).Append('(').Append(i).Append(',').Append(j).Append(',').Append(k).Append(')')
                                                .Append(l).Append(',')
                                                .Append(" <= ").AppendLine(ub(i, j, k, l).ToString());
                        }
                    }
                    else
                        throw new NotImplementedException();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        model.AppendLine("End");
    }


    // helper
    static void AppendRel(StringBuilder sb, ConstraintRelation rel)
    {
        switch (rel)
        {
            case ConstraintRelation.Leq:
                sb.Append(" <= ");
                break;
            case ConstraintRelation.Geq:
                sb.Append(" >= ");
                break;
            case ConstraintRelation.Eq:
                sb.Append(" = ");
                break;
            default: throw new Exception();
        }
    }

    // method
    protected internal override Res<bool> Solve(StringBuilder builtModel)
        => true;


    // method - get - value
    public override double GetVal0(string keyVar0) => 0;
    public override double[] GetVal1(string keyVar1, int len1) => Array.Empty<double>();
    public override double[][] GetVal2(string keyVar2, int len1, int len2) => Array.Empty<double[]>();
    public override double[][] GetJagVal2(string keyJagVar2, int len1, Func<int, int> getLen2) => Array.Empty<double[]>();
    public override double[][][] GetVal3(string keyVar3, int len1, int len2, int len3) => Array.Empty<double[][]>();
    public override double[][][] GetJagVal3(string keyJagVar3, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3) => Array.Empty<double[][]>();
    public override double[][][][] GetVal4(string keyVar4, int len1, int len2, int len3, int len4) => Array.Empty<double[][][]>();
    public override double[][][][] GetJagVal4(string keyJagVar4, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, int> getLen4) => Array.Empty<double[][][]>();
}
