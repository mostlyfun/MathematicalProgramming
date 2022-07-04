using ILOG.Concert;

Model mod = new();

int n = 3;
var a = mod.Set("A", n);
var b = mod.Set("B", a, a => Enumerable.Range(0, a));
var amin1 = mod.Set(n - 1);
//(var B, var b) = mod.Set("B", n);
var X = mod.Cont1("X", n, 0, 1);
var Y = mod.Cont1("Y", n, _ => 0, i => i * 3);
var Z = mod.Cont1("Z", n, 0, 1);

mod.Obj(ObjType.Maximize, X[0] + Y[2]);
mod += a | X[a] <= Sum(b | 3);
mod += (a, b) | X[a] == Y[b] + 1;
mod += amin1 | Z[amin1] == 0;

Console.WriteLine(new string('\n', 10) + mod);

var lp = mod.BuildAndSolve(new LpBuilder()).BuiltModel;
Console.WriteLine(new string('\n', 5) + lp);

var cpBuilder = new CplexBuilder(true);
var (cplex, isSolved) = mod.BuildAndSolve(cpBuilder);
cplex.ExportModel("test.lp");
Console.WriteLine("isSolved = " + isSolved);

INumVar x = cpBuilder.Var1("X")[0];
INumVar v = cpBuilder.Var1("Z")[n - 1];

v.TryMap(v => cplex.GetValue(v)).LogIfErr()
    .Run((double x) => Console.WriteLine("val = " + x));

//Console.WriteLine(val);
