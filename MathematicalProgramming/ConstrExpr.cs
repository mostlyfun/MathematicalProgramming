namespace MathematicalProgramming;

public readonly struct ConstrExpr
{
    // data
    internal readonly ConstraintRelation Relation;
    internal readonly Expr Lhs;
    internal readonly Expr Rhs;
    internal readonly Expr LhsMinRhs;
    // ctor
    internal ConstrExpr(ConstraintRelation relation, Expr lhs, Expr rhs)
    {
        Relation = relation;
        Lhs = lhs;
        Rhs = rhs;
        LhsMinRhs = lhs - rhs;
    }
    // common
    public override string ToString()
        => string.Format("{0}{1}{2}", Lhs, ToStr(Relation), Rhs);
    static string ToStr(ConstraintRelation relation)
    {
        return relation switch
        {
            ConstraintRelation.Leq => " <= ",
            ConstraintRelation.Geq => " >= ",
            ConstraintRelation.Eq => " = ",
            _ => throw new Exception(),
        };
    }


    // op - constr
    public static Constr operator |(Set i, ConstrExpr expr)
        => new(ConKey(), new(i), expr);
    public static Constr operator |((Set i, Set j) sets, ConstrExpr expr)
        => new(ConKey(), new(sets.i, sets.j), expr);
    public static Constr operator |((Set i, Set j, Set k) sets, ConstrExpr expr)
        => new(ConKey(), new(new List<Set>() { sets.i, sets.j, sets.k }), expr);
    public static Constr operator |((Set i, Set j, Set k, Set l) sets, ConstrExpr expr)
            => new(ConKey(), new(new List<Set>() { sets.i, sets.j, sets.k, sets.l }), expr);
    // op - constr - with key
    public static Constr operator |(string key, ConstrExpr expr)
            => new(key, default, expr);
    public static Constr operator |((string key, Set i) sets, ConstrExpr expr)
            => new(sets.key, new(sets.i), expr);
    public static Constr operator |((string key, Set i, Set j) sets, ConstrExpr expr)
        => new(sets.key, new(sets.i, sets.j), expr);
    public static Constr operator |((string key, Set i, Set j, Set k) sets, ConstrExpr expr)
        => new(sets.key, new(new List<Set>() { sets.i, sets.j, sets.k }), expr);
    public static Constr operator |((string key, Set i, Set j, Set k, Set l) sets, ConstrExpr expr)
            => new(sets.key, new(new List<Set>() { sets.i, sets.j, sets.k, sets.l }), expr);
}
