namespace MathematicalProgramming.Helpers;

internal readonly struct OptSca
{
    // data
    internal readonly Opt<Sca> Sca;


    // ctor
    public OptSca()
        => Sca = default;
    internal OptSca(Sca sca)
        => Sca = sca;
    public static implicit operator OptSca(Sca sca)
        => new(sca);


    // method
    public bool IsSome
        => Sca.IsSome;
    public bool IsNone
        => Sca.IsNone;
    public Sca Unwrap()
        => Sca.Unwrap();


    // op
    public static OptSca operator -(OptSca sca)
        => sca.Sca.IsNone ? default : new(-sca.Sca.Unwrap());
    public static OptSca operator +(OptSca s1, OptSca s2)
    {
        if (s1.Sca.IsSome)
        {
            if (s2.Sca.IsSome)
                return s1.Sca.Unwrap() + s2.Sca.Unwrap();
            else
                return s1;
        }
        else
            return s2;
    }
    public static OptSca operator -(OptSca s1, OptSca s2)
    {
        if (s1.Sca.IsSome)
        {
            if (s2.Sca.IsSome)
                return s1.Sca.Unwrap() - s2.Sca.Unwrap();
            else
                return s1;
        }
        else
            return -s2;
    }
    public static OptSca operator *(OptSca s1, OptSca s2)
    {
        if (s1.Sca.IsSome)
        {
            if (s2.Sca.IsSome)
                return s1.Sca.Unwrap() * s2.Sca.Unwrap();
            else
                return default;
        }
        else
            return default;
    }
    public static OptSca operator /(OptSca s1, OptSca s2)
    {
        if (s1.Sca.IsSome)
        {
            if (s2.Sca.IsSome)
                return s1.Sca.Unwrap() / s2.Sca.Unwrap();
            else
                return default;
        }
        else
            return default;
    }
    public static OptSca operator ^(OptSca s1, OptSca s2)
    {
        if (s1.Sca.IsSome)
        {
            if (s2.Sca.IsSome)
                return s1.Sca.Unwrap() ^ s2.Sca.Unwrap();
            else
                return default;
        }
        else
            return default;
    }
    public static OptSca operator %(OptSca s1, OptSca s2)
    {
        if (s1.Sca.IsSome)
        {
            if (s2.Sca.IsSome)
                return s1.Sca.Unwrap() % s2.Sca.Unwrap();
            else
                return default;
        }
        else
            return default;
    }

    // op - num
    public static OptSca operator +(OptSca sca, double num)
    {
        if (sca.Sca.IsSome)
            return sca.Sca.Unwrap() + num;
        else
            return new(num);
    }
    public static OptSca operator +(double num, OptSca sca)
    {
        if (sca.Sca.IsSome)
            return sca.Sca.Unwrap() + num;
        else
            return new(num);
    }
    public static OptSca operator -(OptSca sca, double num)
    {
        if (sca.Sca.IsSome)
            return sca.Sca.Unwrap() - num;
        else
            return new(-num);
    }
    public static OptSca operator -(double num, OptSca sca)
    {
        if (sca.Sca.IsSome)
            return num - sca.Sca.Unwrap();
        else
            return new(num);
    }
    public static OptSca operator *(OptSca sca, double num)
    {
        if (sca.Sca.IsSome)
            return sca.Sca.Unwrap() * num;
        else
            return default;
    }
    public static OptSca operator *(double num, OptSca sca)
    {
        if (sca.Sca.IsSome)
            return sca.Sca.Unwrap() * num;
        else
            return default;
    }
    public static OptSca operator /(OptSca sca, double num)
    {
        if (sca.Sca.IsSome)
            return sca.Sca.Unwrap() / num;
        else
            return default;
    }
    public static OptSca operator /(double num, OptSca sca)
    {
        if (sca.Sca.IsSome)
            return num / sca.Sca.Unwrap();
        else
            return default;
    }
    public static OptSca operator ^(OptSca sca, double num)
    {
        if (sca.Sca.IsSome)
            return sca.Sca.Unwrap() ^ num;
        else
            return default;
    }
    public static OptSca operator ^(double num, OptSca sca)
    {
        if (sca.Sca.IsSome)
            return num ^ sca.Sca.Unwrap();
        else
            return default;
    }
    public static OptSca operator %(OptSca sca, double num)
    {
        if (sca.Sca.IsSome)
            return sca.Sca.Unwrap() % num;
        else
            return default;
    }
    public static OptSca operator %(double num, OptSca sca)
    {
        if (sca.Sca.IsSome)
            return num % sca.Sca.Unwrap();
        else
            return default;
    }
}
