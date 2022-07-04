namespace MathematicalProgramming;

public static class Extensions
{
    // sum
    public static Sum Sum(SumArg sumArg)
        => sumArg.Sum;


    // dependent sets
    public static Set Set(this Set depSet, Func<int, IEnumerable<int>> generator)
        => depSet.Model.Set(depSet, generator);
    public static Set Set(this Set depSet1, Set depSet2, Func<int, int, IEnumerable<int>> generator)
        => depSet1.Model.Set(depSet1, depSet2, generator);
    public static Set Set(this Set depSet1, Set depSet2, Set depSet3, Func<int, int, int, IEnumerable<int>> generator)
        => depSet1.Model.Set(depSet1, depSet2, depSet3, generator);
}
