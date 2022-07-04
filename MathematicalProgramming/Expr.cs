namespace MathematicalProgramming;

public class Expr
{
    // data
    internal readonly OptList<Set> Sets;
    internal readonly OptSca Sca;
    internal readonly OptList<Term> Terms;
    internal readonly OptList<Sum> Sums;
    // common
    internal Expr(OptList<Set> sets, OptSca sca, OptList<Term> terms, OptList<Sum> sums)
    {
        Sets = sets;
        Sca = sca;
        Terms = terms;
        Sums = sums;
    }
    public override string ToString()
    {
        StringBuilder sb = new();
        bool started = false;
        if (Sca.IsSome)
        {
            started = true;
            sb.Append(Sca.Unwrap());
        }
        foreach (var item in Terms)
        {
            if (started)
                sb.Append(" + ");
            sb.Append(item);
            started = true;
        }
        foreach (var item in Sums)
        {
            if (started)
                sb.Append(" + ");
            sb.Append(item);
            started = true;
        }
        return sb.ToString();
    }


    // implicit
    public static implicit operator Expr(double num)
        => new(default, (Sca)num, default, default);
    public static implicit operator Expr(int num)
        => new(default, (Sca)num, default, default);
    public static implicit operator Expr(Sca sca)
        => new(sca.Meta.Sets, sca, default, default);
    public static implicit operator Expr(Term term)
        => new(term.Meta.Sets, default, term, default);
    public static implicit operator Expr(Sum sum)
        => new(sum.Sets, default, default, sum);


    // op - expr
    public static Expr operator -(Expr exp)
        => new(exp.Sets, -exp.Sca, exp.Terms.Neg(), exp.Sums.Neg());
    public static Expr operator +(Expr e1, Expr e2)
        => new(e1.Sets + e2.Sets, e1.Sca + e2.Sca, e1.Terms + e2.Terms, e1.Sums + e2.Sums);
    public static Expr operator -(Expr e1, Expr e2)
        => new(e1.Sets + e2.Sets, e1.Sca - e2.Sca, e1.Terms + e2.Terms.Neg(), e1.Sums + e2.Sums.Neg());

    // op - num
    public static Expr operator +(Expr expr, double num)
        => new(expr.Sets, expr.Sca + num, expr.Terms, expr.Sums);
    public static Expr operator -(Expr expr, double num)
        => new(expr.Sets, expr.Sca - num, expr.Terms, expr.Sums);
    public static Expr operator *(Expr expr, double num)
        => new(expr.Sets, expr.Sca * num, expr.Terms.Mul(num), expr.Sums.Mul(num));
    public static Expr operator /(Expr expr, double num)
        => new(expr.Sets, expr.Sca / num, expr.Terms.Div(num), expr.Sums.Div(num));
    public static Expr operator +(double num, Expr expr)
        => new(expr.Sets, expr.Sca + num, expr.Terms, expr.Sums);
    public static Expr operator -(double num, Expr expr)
        => new(expr.Sets, num - expr.Sca, expr.Terms, expr.Sums);
    public static Expr operator *(double num, Expr expr)
        => new(expr.Sets, expr.Sca * num, expr.Terms.Mul(num), expr.Sums.Mul(num));

    // op - sca
    public static Expr operator +(Expr expr, Sca sca)
        => new(expr.Sets.Union(sca.Meta.Sets), expr.Sca + sca, expr.Terms, expr.Sums);
    public static Expr operator -(Expr expr, Sca sca)
        => new(expr.Sets.Union(sca.Meta.Sets), expr.Sca - sca, expr.Terms, expr.Sums);
    public static Expr operator *(Expr expr, Sca sca)
        => new(expr.Sets.Union(sca.Meta.Sets), expr.Sca * sca, expr.Terms.Mul(sca), expr.Sums.Mul(sca));
    public static Expr operator /(Expr expr, Sca sca)
        => new(expr.Sets.Union(sca.Meta.Sets), expr.Sca / sca, expr.Terms.Div(sca), expr.Sums.Div(sca));
    public static Expr operator +(Sca sca, Expr expr)
        => new(expr.Sets.Union(sca.Meta.Sets), expr.Sca + sca, expr.Terms, expr.Sums);
    public static Expr operator -(Sca sca, Expr expr)
        => new(expr.Sets.Union(sca.Meta.Sets), sca - expr.Sca, expr.Terms, expr.Sums);
    public static Expr operator *(Sca sca, Expr expr)
        => new(expr.Sets.Union(sca.Meta.Sets), expr.Sca * sca, expr.Terms.Mul(sca), expr.Sums.Mul(sca));

    // op - term
    public static Expr operator +(Expr expr, Term term)
        => new(expr.Sets + term.Meta.Sets, expr.Sca, expr.Terms + term, expr.Sums);
    public static Expr operator +(Term term, Expr expr)
        => new(expr.Sets + term.Meta.Sets, expr.Sca, term + expr.Terms, expr.Sums);
    public static Expr operator -(Expr expr, Term term)
        => new(expr.Sets + term.Meta.Sets, expr.Sca, expr.Terms + (-term), expr.Sums);
    public static Expr operator -(Term term, Expr expr)
        => new(expr.Sets + term.Meta.Sets, expr.Sca, term + expr.Terms.Neg(), expr.Sums);

    // op - sum
    public static Expr operator +(Expr expr, Sum sum)
        => new(expr.Sets + sum.Sets, expr.Sca, expr.Terms, expr.Sums + sum);
    public static Expr operator +(Sum sum, Expr expr)
        => new(expr.Sets + sum.Sets, expr.Sca, expr.Terms, sum + expr.Sums);
    public static Expr operator -(Expr expr, Sum sum)
        => new(expr.Sets + sum.Sets, expr.Sca, expr.Terms, expr.Sums + (-sum));
    public static Expr operator -(Sum sum, Expr expr)
        => new(expr.Sets + sum.Sets, expr.Sca, expr.Terms, sum + expr.Sums.Neg());


    // op - constraint - expr*expr
    public static ConstrExpr operator <=(Expr lhs, Expr rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Expr lhs, Expr rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Expr lhs, Expr rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Expr _lhs, Expr _rhs)
        => throw new Exception();
    // op - constraint - num*expr
    public static ConstrExpr operator <=(Expr lhs, double rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Expr lhs, double rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Expr lhs, double rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Expr _lhs, double _rhs)
        => throw new Exception();
    public static ConstrExpr operator <=(double lhs, Expr rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(double lhs, Expr rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(double lhs, Expr rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(double _lhs, Expr _rhs)
        => throw new Exception();
    // op - constraint - sca*expr
    public static ConstrExpr operator <=(Expr lhs, Sca rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Expr lhs, Sca rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Expr lhs, Sca rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Expr _lhs, Sca _rhs)
        => throw new Exception();
    public static ConstrExpr operator <=(Sca lhs, Expr rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Sca lhs, Expr rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Sca lhs, Expr rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Sca _lhs, Expr _rhs)
        => throw new Exception();
    // op - constraint - term*expr
    public static ConstrExpr operator <=(Expr lhs, Term rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Expr lhs, Term rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Expr lhs, Term rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Expr _lhs, Term _rhs)
        => throw new Exception();
    public static ConstrExpr operator <=(Term lhs, Expr rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Term lhs, Expr rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Term lhs, Expr rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Term _lhs, Expr _rhs)
        => throw new Exception();
    // op - constraint - sum*expr
    public static ConstrExpr operator <=(Expr lhs, Sum rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Expr lhs, Sum rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Expr lhs, Sum rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Expr _lhs, Sum _rhs)
        => throw new Exception();
    public static ConstrExpr operator <=(Sum lhs, Expr rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Sum lhs, Expr rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Sum lhs, Expr rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Sum _lhs, Expr _rhs)
        => throw new Exception();


    // op - sum
    public static SumArg operator |(Set i, Expr expr)
        => new(new(new(i), expr));
    public static SumArg operator |((Set i, Set j) sets, Expr expr)
        => new(new(new(sets.i, sets.j), expr));
    public static SumArg operator |((Set i, Set j, Set k) sets, Expr expr)
            => new(new(new(new List<Set>() { sets.i, sets.j, sets.k }), expr));
}
