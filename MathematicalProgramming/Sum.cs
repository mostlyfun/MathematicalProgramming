namespace MathematicalProgramming;

public readonly struct Sum
{
    // data
    internal readonly OptList<Set> Sets;
    internal readonly Expr Expr;
    // ctor
    internal Sum(OptList<Set> sets, Expr sumExpr)
    {
        Sets = sets;
        Expr = sumExpr;
    }
    // common
    public override string ToString()
        => string.Format("SUM{{{0} | {1}}}", string.Join(",", Sets), Expr);


    // op
    public static Sum operator -(Sum sum)
        => new(sum.Sets, -sum.Expr);
    public static Expr operator +(Sum s1, Sum s2)
        => new(s1.Sets + s2.Sets, default, default, new(s1, s2));
    public static Expr operator -(Sum s1, Sum s2)
        => new(s1.Sets + s2.Sets, default, default, new(s1, -s2));

    // op - num
    public static Expr operator +(Sum sum, double num)
        => new(sum.Sets, new(num), default, sum);
    public static Expr operator +(double num, Sum sum)
        => new(sum.Sets, new(num), default, sum);
    public static Expr operator -(Sum sum, double num)
        => new(sum.Sets, new(-num), default, sum);
    public static Expr operator -(double num, Sum sum)
        => new(sum.Sets, new(num), default, -sum);
    public static Sum operator *(Sum sum, double num)
        => new(sum.Sets, sum.Expr * num);
    public static Sum operator *(double num, Sum sum)
        => new(sum.Sets, num * sum.Expr);
    public static Sum operator /(Sum sum, double num)
        => new(sum.Sets, sum.Expr / num);

    // op - sca
    public static Expr operator +(Sum sum, Sca sca)
        => new(sum.Sets + sca.Meta.Sets, new(sca), default, sum);
    public static Expr operator +(Sca sca, Sum sum)
        => new(sum.Sets + sca.Meta.Sets, new(sca), default, sum);
    public static Expr operator -(Sum sum, Sca sca)
        => new(sum.Sets + sca.Meta.Sets, new(-sca), default, sum);
    public static Expr operator -(Sca sca, Sum sum)
        => new(sum.Sets + sca.Meta.Sets, new(sca), default, -sum);
    public static Sum operator *(Sum sum, Sca sca)
        => new(sum.Sets, sum.Expr * sca);
    public static Sum operator *(Sca sca, Sum sum)
        => new(sum.Sets, sca * sum.Expr);
    public static Sum operator /(Sum sum, Sca sca)
        => new(sum.Sets, sum.Expr / sca);

    // op - term
    public static Expr operator +(Sum sum, Term term)
        => new(sum.Sets + term.Meta.Sets, default, term, sum);
    public static Expr operator +(Term term, Sum sum)
        => new(sum.Sets + term.Meta.Sets, default, term, sum);
    public static Expr operator -(Sum sum, Term term)
        => new(sum.Sets + term.Meta.Sets, default, new(-term), sum);
    public static Expr operator -(Term term, Sum sum)
            => new(sum.Sets + term.Meta.Sets, default, term, new(-sum));

    // op - constraint - sum*sum
    public static ConstrExpr operator <=(Sum lhs, Sum rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Sum lhs, Sum rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Sum lhs, Sum rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Sum _lhs, Sum _rhs)
        => throw new Exception();
    // op - constraint - sum*term
    public static ConstrExpr operator <=(Sum lhs, Term rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Sum lhs, Term rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Sum lhs, Term rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Sum _lhs, Term _rhs)
        => throw new Exception();
    public static ConstrExpr operator <=(Term lhs, Sum rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Term lhs, Sum rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Term lhs, Sum rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Term _lhs, Sum _rhs)
        => throw new Exception();
    // op - constraint - sum*sca
    public static ConstrExpr operator <=(Sum lhs, Sca rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Sum lhs, Sca rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Sum lhs, Sca rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Sum _lhs, Sca _rhs)
        => throw new Exception();
    public static ConstrExpr operator <=(Sca lhs, Sum rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Sca lhs, Sum rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Sca lhs, Sum rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Sca _lhs, Sum _rhs)
        => throw new Exception();
    // op - constraint - sum*num
    public static ConstrExpr operator <=(Sum lhs, double rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Sum lhs, double rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Sum lhs, double rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Sum _lhs, double _rhs)
        => throw new Exception();
    public static ConstrExpr operator <=(double lhs, Sum rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(double lhs, Sum rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(double lhs, Sum rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(double _lhs, Sum _rhs)
        => throw new Exception();
}
