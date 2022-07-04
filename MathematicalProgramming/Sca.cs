namespace MathematicalProgramming;

public readonly struct Sca
{
    // data
    internal readonly SymbolMeta Meta;
    internal readonly Func<int[], double> Value;
    // common
    internal Sca(SymbolMeta meta, Func<int[], double> value)
    {
        Meta = meta;
        Value = value;
    }
    public override string ToString()
        => Meta.Key;


    // implicit
    public static implicit operator Sca(double value)
        => new(new(value.ToString()), _ => value);
    public static implicit operator Sca(int value)
        => new(new(value.ToString()), _ => value);
    public static implicit operator Sca(Set set)
        => new(new(set.Key.ToLower(), new(set)), ijk => ijk[set.IndexInModel]);


    // op - scalar
    public static Sca operator -(Sca sca)
        => new(-sca.Meta, ijk => -sca.Value(ijk));
    public static Sca operator +(Sca sca1, Sca sca2)
        => new(sca1.Meta + sca2.Meta, ijk => sca1.Value(ijk) + sca2.Value(ijk));
    public static Sca operator -(Sca sca1, Sca sca2)
        => new(sca1.Meta - sca2.Meta, ijk => sca1.Value(ijk) - sca2.Value(ijk));
    public static Sca operator *(Sca sca1, Sca sca2)
        => new(sca1.Meta * sca2.Meta, ijk => sca1.Value(ijk) * sca2.Value(ijk));
    public static Sca operator /(Sca sca1, Sca sca2)
        => new(sca1.Meta / sca2.Meta, ijk => sca1.Value(ijk) / sca2.Value(ijk));
    public static Sca operator ^(Sca sca1, Sca sca2)
        => new(sca1.Meta ^ sca2.Meta, ijk => Math.Pow(sca1.Value(ijk), sca2.Value(ijk)));
    public static Sca operator %(Sca sca1, Sca sca2)
        => new(sca1.Meta % sca2.Meta, ijk => sca1.Value(ijk) % sca2.Value(ijk));
    // op - number
    public static Sca operator +(Sca sca, double num)
        => new(sca.Meta + num, ijk => sca.Value(ijk) + num);
    public static Sca operator +(double num, Sca sca)
        => new(num + sca.Meta, ijk => sca.Value(ijk) + num);
    public static Sca operator -(Sca sca, double num)
        => new(sca.Meta - num, ijk => sca.Value(ijk) - num);
    public static Sca operator -(double num, Sca sca)
        => new(num - sca.Meta, ijk => num - sca.Value(ijk));
    public static Sca operator *(Sca sca, double num)
        => new(sca.Meta * num, ijk => sca.Value(ijk) * num);
    public static Sca operator *(double num, Sca sca)
        => new(num * sca.Meta, ijk => sca.Value(ijk) * num);
    public static Sca operator /(Sca sca, double num)
        => new(sca.Meta / num, ijk => sca.Value(ijk) / num);
    public static Sca operator /(double num, Sca sca)
        => new(num  / sca.Meta, ijk => num / sca.Value(ijk));
    public static Sca operator ^(Sca sca, double num)
        => new(sca.Meta ^ num, ijk => Math.Pow(sca.Value(ijk), num));
    public static Sca operator ^(double num, Sca sca)
        => new(num ^ sca.Meta, ijk => Math.Pow(num, sca.Value(ijk)));
    public static Sca operator %(Sca sca, double num)
        => new(sca.Meta % num, ijk => sca.Value(ijk) % num);
    public static Sca operator %(double num, Sca sca)
        => new(num % sca.Meta, ijk => num % sca.Value(ijk));


    // op - constraint - sca*sca
    public static ConstrExpr operator <=(Sca lhs, Sca rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Sca lhs, Sca rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Sca lhs, Sca rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Sca _lhs, Sca _rhs)
        => throw new Exception();
    // op - constraint - sca*num
    public static ConstrExpr operator <=(double lhs, Sca rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(double lhs, Sca rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(double lhs, Sca rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(double _lhs, Sca _rhs)
        => throw new Exception();
    public static ConstrExpr operator <=(Sca lhs, double rhs)
        => new(ConstraintRelation.Leq, lhs, rhs);
    public static ConstrExpr operator >=(Sca lhs, double rhs)
        => new(ConstraintRelation.Geq, lhs, rhs);
    public static ConstrExpr operator ==(Sca lhs, double rhs)
        => new(ConstraintRelation.Eq, lhs, rhs);
    public static ConstrExpr operator !=(Sca _lhs, double _rhs)
        => throw new Exception();


    // op - sum
    public static SumArg operator |(Set i, Sca sca)
        => new(new(new(i), sca));
    public static SumArg operator |((Set i, Set j) sets, Sca sca)
        => new(new(new(sets.i, sets.j), sca));
    public static SumArg operator |((Set i, Set j, Set k) sets, Sca sca)
            => new(new(new(new List<Set>() { sets.i, sets.j, sets.k }), sca));
    public static SumArg operator |((Set i, Set j, Set k, Set l) sets, Sca sca)
                => new(new(new(new List<Set>() { sets.i, sets.j, sets.k, sets.l }), sca));
}
