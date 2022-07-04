namespace MathematicalProgramming.Helpers;

internal static class HelperExtensions
{
    // Set
    public static OptList<Set> Union(this OptList<Set> first, OptList<Set> second)
    {
        if (first.Length == 0)
            return second;
        if (second.Length == 0)
            return first;

        if (first.Length == 1)
        {
            Set f1 = first[0];
            if (second.Length == 1)
            {
                Set s1 = second[0];
                if (f1.Key == s1.Key)
                    return first;
                else
                    return new(f1, s1);
            }
            else if (second.Length == 2)
            {
                Set s1 = second[0], s2 = second[1];
                if (f1.Key == s1.Key || f1.Key == s2.Key)
                    return second;
                else
                    return new(new List<Set>(3) { f1, s1, s2 });
            }
            else
            {
                var sl = second.MaybeList.Unwrap();
                bool insF1 = sl.Any(x => x.Key == f1.Key);
                if (insF1)
                    return second;
                var lst = new List<Set>(sl.Count + 1);
                lst.Add(f1);
                return new(lst);
            }
        }
        else if (first.Length == 2)
        {
            Set f1 = first[0], f2 = first[1];
            if (second.Length == 1)
            {
                Set s1 = second[0];
                if (f1.Key == s1.Key || f2.Key == s1.Key)
                    return first;
                else
                    return new(new List<Set>(3) { f1, f2, s1 });
            }
            else if (second.Length == 2)
            {
                Set s1 = second[0], s2 = second[1];
                bool addS1 = s1.Key != f1.Key && s1.Key != f2.Key;
                bool addS2 = s2.Key != f1.Key && s2.Key != f2.Key;
                int len = 2 + (addS1 ? 1 : 0) + (addS2 ? 1 : 0);
                if (len == 2)
                    return first;
                else if (len == 3)
                {
                    var lst = new List<Set>(3) { f1, f2 };
                    if (addS1)
                        lst.Add(s1);
                    else if (addS2)
                        lst.Add(s2);
                    return new(lst);
                }
                else
                    return new(new List<Set>(4) { f1, f2, s1, s2 });
            }
            else
            {
                var sl = second.MaybeList.Unwrap();
                bool addF1 = !sl.Any(x => x.Key == f1.Key);
                bool addF2 = !sl.Any(x => x.Key == f2.Key);
                int addLen = (addF1 ? 1 : 0) + (addF2 ? 1 : 0);
                if (addLen == 0)
                    return new(sl);
                else if (addLen == 1)
                {
                    var lst = new List<Set>(sl.Count + 1);
                    if (addF1)
                        lst.Add(f1);
                    else if (addF2)
                        lst.Add(f2);
                    lst.AddRange(sl);
                    return new(lst);
                }
                else
                {
                    var lst = new List<Set>(sl.Count + 2) { f1, f2 };
                    lst.AddRange(sl);
                    return new(lst);
                }
            }
        }
        else
        {
            var fl = first.MaybeList.Unwrap();
            if (second.Length == 1)
            {
                Set s1 = second[0];
                int addS1 = fl.Any(x => x.Key == s1.Key) ? 0 : 1;
                if (addS1 == 0)
                    return first;
                else
                {
                    var lst = new List<Set>(fl.Count + 1);
                    lst.AddRange(fl);
                    lst.Add(s1);
                    return new(lst);
                }
            }
            else if (second.Length == 2)
            {
                Set s1 = second[0], s2 = second[1];
                int addS1 = fl.Any(x => x.Key == s1.Key) ? 0 : 1;
                int addS2 = fl.Any(x => x.Key == s2.Key) ? 0 : 1;
                int add = addS1 + addS2;
                if (add == 0)
                    return first;
                else if (add == 1)
                {
                    var lst = new List<Set>(fl.Count + 1);
                    lst.AddRange(fl);
                    if (addS1 == 1)
                        lst.Add(s1);
                    else if (addS2 == 1)
                        lst.Add(s2);
                    return new(lst);
                }
                else
                {
                    var lst = new List<Set>(fl.Count + 2);
                    lst.AddRange(fl);
                    lst.Add(s1);
                    lst.Add(s2);
                    return new(lst);
                }
            }
            else
            {
                var sl = second.MaybeList.Unwrap();
                var lst = new List<Set>(fl);
                foreach (var item in sl)
                {
                    bool contains = false;
                    for (int f = 0; f < fl.Count; f++)
                    {
                        if (fl[f].Key == item.Key)
                        {
                            contains = true;
                            break;
                        }
                    }
                    if (!contains)
                        lst.Add(item);
                }
                return new(lst);
            }
        }
    }


    // Terms
    public static OptList<Term> Neg(this OptList<Term> terms)
    {
        if (terms.MaybeList.IsSome)
        {
            var lst = terms.MaybeList.Unwrap();
            for (int i = 0; i < lst.Count; i++)
                lst[i] = -lst[i];
            return new(lst);
        }
        if (terms.Val2.IsSome)
            return new(-terms.Val1.Unwrap(), -terms.Val2.Unwrap());
        if (terms.Val1.IsSome)
            return new(-terms.Val1.Unwrap());
        return new();
    }
    public static OptList<Term> Mul(this OptList<Term> terms, double num)
    {
        if (terms.MaybeList.IsSome)
        {
            var lst = terms.MaybeList.Unwrap();
            for (int i = 0; i < lst.Count; i++)
                lst[i] = lst[i] * num;
            return new(lst);
        }
        if (terms.Val2.IsSome)
            return new(terms.Val1.Unwrap() * num, terms.Val2.Unwrap() * num);
        if (terms.Val1.IsSome)
            return new(terms.Val1.Unwrap() * num);
        return new();
    }
    public static OptList<Term> Div(this OptList<Term> terms, double num)
    {
        if (terms.MaybeList.IsSome)
        {
            var lst = terms.MaybeList.Unwrap();
            for (int i = 0; i < lst.Count; i++)
                lst[i] = lst[i] / num;
            return new(lst);
        }
        if (terms.Val2.IsSome)
            return new(terms.Val1.Unwrap() / num, terms.Val2.Unwrap() / num);
        if (terms.Val1.IsSome)
            return new(terms.Val1.Unwrap() / num);
        return new();
    }
    public static OptList<Term> Mul(this OptList<Term> terms, Sca sca)
    {
        if (terms.MaybeList.IsSome)
        {
            var lst = terms.MaybeList.Unwrap();
            for (int i = 0; i < lst.Count; i++)
                lst[i] = lst[i] * sca;
            return new(lst);
        }
        if (terms.Val2.IsSome)
            return new(terms.Val1.Unwrap() * sca, terms.Val2.Unwrap() * sca);
        if (terms.Val1.IsSome)
            return new(terms.Val1.Unwrap() * sca);
        return new();
    }
    public static OptList<Term> Div(this OptList<Term> terms, Sca sca)
    {
        if (terms.MaybeList.IsSome)
        {
            var lst = terms.MaybeList.Unwrap();
            for (int i = 0; i < lst.Count; i++)
                lst[i] = lst[i] / sca;
            return new(lst);
        }
        if (terms.Val2.IsSome)
            return new(terms.Val1.Unwrap() / sca, terms.Val2.Unwrap() / sca);
        if (terms.Val1.IsSome)
            return new(terms.Val1.Unwrap() / sca);
        return new();
    }
    // Sums
    public static OptList<Sum> Neg(this OptList<Sum> terms)
    {
        if (terms.MaybeList.IsSome)
        {
            var lst = terms.MaybeList.Unwrap();
            for (int i = 0; i < lst.Count; i++)
                lst[i] = -lst[i];
            return new(lst);
        }
        if (terms.Val2.IsSome)
            return new(-terms.Val1.Unwrap(), -terms.Val2.Unwrap());
        if (terms.Val1.IsSome)
            return new(-terms.Val1.Unwrap());
        return new();
    }
    public static OptList<Sum> Mul(this OptList<Sum> terms, double num)
    {
        if (terms.MaybeList.IsSome)
        {
            var lst = terms.MaybeList.Unwrap();
            for (int i = 0; i < lst.Count; i++)
                lst[i] = lst[i] * num;
            return new(lst);
        }
        if (terms.Val2.IsSome)
            return new(terms.Val1.Unwrap() * num, terms.Val2.Unwrap() * num);
        if (terms.Val1.IsSome)
            return new(terms.Val1.Unwrap() * num);
        return new();
    }
    public static OptList<Sum> Div(this OptList<Sum> terms, double num)
    {
        if (terms.MaybeList.IsSome)
        {
            var lst = terms.MaybeList.Unwrap();
            for (int i = 0; i < lst.Count; i++)
                lst[i] = lst[i] / num;
            return new(lst);
        }
        if (terms.Val2.IsSome)
            return new(terms.Val1.Unwrap() / num, terms.Val2.Unwrap() / num);
        if (terms.Val1.IsSome)
            return new(terms.Val1.Unwrap() / num);
        return new();
    }
    public static OptList<Sum> Mul(this OptList<Sum> terms, Sca sca)
    {
        if (terms.MaybeList.IsSome)
        {
            var lst = terms.MaybeList.Unwrap();
            for (int i = 0; i < lst.Count; i++)
                lst[i] = lst[i] * sca;
            return new(lst);
        }
        if (terms.Val2.IsSome)
            return new(terms.Val1.Unwrap() * sca, terms.Val2.Unwrap() * sca);
        if (terms.Val1.IsSome)
            return new(terms.Val1.Unwrap() * sca);
        return new();
    }
    public static OptList<Sum> Div(this OptList<Sum> terms, Sca sca)
    {
        if (terms.MaybeList.IsSome)
        {
            var lst = terms.MaybeList.Unwrap();
            for (int i = 0; i < lst.Count; i++)
                lst[i] = lst[i] / sca;
            return new(lst);
        }
        if (terms.Val2.IsSome)
            return new(terms.Val1.Unwrap() / sca, terms.Val2.Unwrap() / sca);
        if (terms.Val1.IsSome)
            return new(terms.Val1.Unwrap() / sca);
        return new();
    }


    // singletons
    internal static readonly Dictionary<string, (int Dim, int Ind)> EmptyGetConcrVars = new(0);


    // Keys
    internal static string ParKey() => string.Format("__PAR__{0}", ParCounter++);
    static int ParCounter = 0;
    internal static string ConKey() => string.Format("__CON__{0}", ConCounter++);
    static int ConCounter = 0;
}
