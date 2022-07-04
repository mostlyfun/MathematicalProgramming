namespace MathematicalProgramming;

public class Constr
{
    // data
    internal string Key;
    internal readonly OptList<Set> Sets;
    internal readonly ConstrExpr Expr;
    // ctor
    internal Constr(string key, OptList<Set> sets, ConstrExpr expr)
    {
        // todo: make sure the sets are distinct
        Key = key;
        Sets = sets;
        Expr = expr;
    }
    // common
    public override string ToString()
        => string.Format("{0}({1}) | {2}", Key, string.Join(", ", Sets), Expr);
    // implicit
    public static implicit operator Constr(ConstrExpr expr)
        => new(ConKey(), default, expr);

    // method
    public ConstraintRelation Relation
        => Expr.Relation;
}
