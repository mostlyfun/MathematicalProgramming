namespace MathematicalProgramming;

public struct Term
{
    // data
    internal readonly SymbolMeta Meta;
    internal readonly Func<int[], double> Coef;
    internal readonly IVar Var;
    internal readonly OptList<Sca> Indices;
    // common
    internal Term(SymbolMeta meta, Func<int[], double> coef, IVar var, OptList<Sca> indices)
    {
        Meta = meta;
        Coef = coef;
        Var = var;
        Indices = indices;
    }
    public override string ToString()
        => Meta.Key;

    // implicit
    public static implicit operator Term(Var0 var)
        => new(new("1*" + var.Key), S.GetOne, var, default);

    // op - tern
    public static Term operator -(Term term)
        => new(-term.Meta, ijk => -term.Coef(ijk), term.Var, term.Indices);
    public static Expr operator +(Term t, Term u)
        => new(t.Meta.Sets + u.Meta.Sets, default, new(t, u), default);
    public static Expr operator -(Term t, Term u)
        => new(t.Meta.Sets + u.Meta.Sets, default, new(t, -u), default);

    // op - num
    public static Expr operator +(Term term, double num)
        => new(term.Meta.Sets, (Sca)num, term, default);
    public static Expr operator +(double num, Term term)
        => new(term.Meta.Sets, (Sca)num, term, default);
    public static Expr operator -(Term term, double num)
        => term + (-num);
    public static Expr operator -(double num, Term term)
        => -term + num;
    public static Term operator *(Term term, double num)
        => new(num * term.Meta, ijk => num * term.Coef(ijk), term.Var, term.Indices);
    public static Term operator *(double num, Term term)
        => new(num * term.Meta, ijk => num * term.Coef(ijk), term.Var, term.Indices);
    public static Term operator /(Term term, double num)
        => new(term.Meta / num, ijk => term.Coef(ijk) / num, term.Var, term.Indices);

    // op - sca
    public static Expr operator +(Term term, Sca sca)
        => new(term.Meta.Sets + sca.Meta.Sets, sca, term, default);
    public static Expr operator +(Sca sca, Term term)
        => term + sca;
    public static Expr operator -(Term term, Sca sca)
        => term + (-sca);
    public static Expr operator -(Sca sca, Term term)
        => -term + sca;
    public static Term operator *(Term term, Sca sca)
        => new(sca.Meta * term.Meta, ijk => sca.Value(ijk) * term.Coef(ijk), term.Var, term.Indices);
    public static Term operator *(Sca sca, Term term)
        => new(sca.Meta * term.Meta, ijk => sca.Value(ijk) * term.Coef(ijk), term.Var, term.Indices);
    public static Term operator /(Term term, Sca sca)
        => new(term.Meta / sca.Meta, ijk => term.Coef(ijk) / sca.Value(ijk), term.Var, term.Indices);

    // op - constraint - term*term
    public static ConstrExpr operator <=(Term lhs, Term rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Term lhs, Term rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Term lhs, Term rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Term _lhs, Term _rhs)
        => throw new Exception();
    // op - constraint - term*sca
    public static ConstrExpr operator <=(Sca lhs, Term rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Sca lhs, Term rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Sca lhs, Term rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Sca _lhs, Term _rhs)
        => throw new Exception();
    public static ConstrExpr operator <=(Term lhs, Sca rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Term lhs, Sca rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Term lhs, Sca rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Term _lhs, Sca _rhs)
        => throw new Exception();
    // op - constraint - term*num
    public static ConstrExpr operator <=(double lhs, Term rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(double lhs, Term rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(double lhs, Term rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(double _lhs, Term _rhs)
        => throw new Exception();
    public static ConstrExpr operator <=(Term lhs, double rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Term lhs, double rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Term lhs, double rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Term _lhs, double _rhs)
        => throw new Exception();


    // op - sum
    public static SumArg operator |(Set i, Term term)
        => new(new(new(i), term));
    public static SumArg operator |((Set i, Set j) sets, Term term)
        => new(new(new(sets.i, sets.j), term));
    public static SumArg operator |((Set i, Set j, Set k) sets, Term term)
            => new(new(new(new List<Set>() { sets.i, sets.j, sets.k }), term));
}
