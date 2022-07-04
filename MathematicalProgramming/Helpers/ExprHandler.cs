namespace MathematicalProgramming.Helpers;

internal struct ExprHandler<C, V>
{
    internal Func<C, int[], double> Handle;
    internal ExprHandler<C, V>[] Sums;
}
