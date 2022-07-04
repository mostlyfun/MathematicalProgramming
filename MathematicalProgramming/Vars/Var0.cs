namespace MathematicalProgramming;

public readonly struct Var0 : IVar
{
    // data
    public string Key { get; }
    public VarType VarType { get; }
    public readonly double Lb;
    public readonly double Ub;
    // ctor
    public Var0(string key, VarType varType, double lb, double ub)
    {
        Key = key;
        VarType = varType;
        Lb = lb;
        Ub = ub;
    }
    // common
    public override string ToString()
        => Key;

    
    // implicit
    public static implicit operator Expr(Var0 var)
        => (Term)var;


    // method
    public int Dim => 0;
    public bool IsContinuous => VarType == VarType.Continuous;
    public bool IsDiscrete => VarType != VarType.Continuous;


    // op - var
    public static Term operator -(Var0 var)
    {
        Term term = var;
        return -term;
    }
    public static Expr operator +(Var0 t, Var0 u)
        => (Term)t + u;
    public static Expr operator -(Var0 t, Var0 u)
        => (Term)t - u;


    // op - term
    public static Expr operator +(Var0 t, Term u)
        => (Term)t + u;
    public static Expr operator +(Term t, Var0 u)
        => t + (Term)u;
    public static Expr operator -(Var0 t, Term u)
        => (Term)t - u;
    public static Expr operator -(Term t, Var0 u)
        => t - (Term)u;

    // op - num
    public static Expr operator +(Var0 var, double num)
    {
        Term term = var;
        return term + num;
    }
    public static Expr operator +(double num, Var0 var)
    {
        Term term = var;
        return term + num;
    }
    public static Expr operator -(Var0 var, double num)
        => (Term)var + (-num);
    public static Expr operator -(double num, Var0 var)
    {
        Term term = var;
        return -term + num;
    }
    public static Term operator *(Var0 var, double num)
        => (Term)var * num;
    public static Term operator* (double num, Var0 var)
        => num * (Term)var;
    public static Term operator /(Var0 var, double num)
        => (Term)var / num;

    // op - sca
    public static Expr operator +(Var0 var, Sca sca)
        => (Term)var + sca;
    public static Expr operator +(Sca sca, Var0 var)
        => (Term)var + sca;
    public static Expr operator -(Var0 var, Sca sca)
        => (Term)var + (-sca);
    public static Expr operator -(Sca sca, Var0 var)
    {
        Term term = var;
        return sca - term;
    }
    public static Term operator *(Var0 var, Sca sca)
        => (Term)var * sca;
    public static Term operator *(Sca sca, Var0 var)
        => sca * (Term)var;
    public static Term operator /(Var0 var, Sca sca)
        => (Term)var / sca;


    // op - constraint - term*term
    public static ConstrExpr operator <=(Var0 lhs, Var0 rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Var0 lhs, Var0 rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Var0 lhs, Var0 rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Var0 _lhs, Var0 _rhs)
        => throw new Exception();
    // op - constraint - term*sca
    public static ConstrExpr operator <=(Sca lhs, Var0 rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Sca lhs, Var0 rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Sca lhs, Var0 rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Sca _lhs, Var0 _rhs)
        => throw new Exception();
    public static ConstrExpr operator <=(Var0 lhs, Sca rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Var0 lhs, Sca rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Var0 lhs, Sca rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Var0 _lhs, Sca _rhs)
        => throw new Exception();
    // op - constraint - term*num
    public static ConstrExpr operator <=(double lhs, Var0 rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(double lhs, Var0 rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(double lhs, Var0 rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(double _lhs, Var0 _rhs)
        => throw new Exception();
    public static ConstrExpr operator <=(Var0 lhs, double rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Var0 lhs, double rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Var0 lhs, double rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Var0 _lhs, double _rhs)
        => throw new Exception();


    // op - sum
    public static SumArg operator |(Set i, Var0 term)
        => new(new(new(i), term));
    public static SumArg operator |((Set i, Set j) sets, Var0 term)
        => new(new(new(sets.i, sets.j), term));
    public static SumArg operator |((Set i, Set j, Set k) sets, Var0 term)
            => new(new(new(new List<Set>() { sets.i, sets.j, sets.k }), term));
}
