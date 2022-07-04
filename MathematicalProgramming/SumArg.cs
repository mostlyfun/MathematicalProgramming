namespace MathematicalProgramming;

public readonly ref struct SumArg
{
    // data
    internal readonly Sum Sum;
    internal SumArg(Sum sum)
        => Sum = sum;
}
