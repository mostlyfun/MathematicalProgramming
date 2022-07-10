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
var W = mod.Int2("W", n, n, 1, 10);

mod.Obj(ObjType.Maximize, X[0] + Y[2] + W[0, 0]);
mod += a | X[a] <= Sum(b | 3);
mod += (a, b) | X[a] == Y[b] + 1;
mod += amin1 | Z[amin1] == 0;
mod += Y[0] <= W[0, 0];

Console.WriteLine(new string('\n', 10) + mod);

//var lp = mod.BuildAndSolve(new LpBuilder()).Unwrap().BuiltModel;
//Console.WriteLine(new string('\n', 5) + lp);
//File.WriteAllText("play.lp", lp.ToString());

var solver = LpSolver.NewCplex(@"C:\Users\User\Documents\uur\Github\dotnet\MathematicalProgramming\Playground\bin\Debug\net6.0\tmp").Unwrap();
var lps = mod.BuildAndSolve(solver);
Console.WriteLine(lps);

Console.WriteLine(string.Join(", ", solver.GetVal2(W).Select(x => x.Length)));
Console.WriteLine(string.Join(", ", solver.GetVal2(W)[0]));


//var cpBuilder = new CplexBuilder(true);
//var (cplex, isSolved) = mod.BuildAndSolve(cpBuilder).Unwrap();
//cplex.ExportModel("test.lp");
//Console.WriteLine("isSolved = " + isSolved);

//INumVar x = cpBuilder.Var1("X")[2];
//INumVar v = cpBuilder.Var1("Z")[n - 1];

//x.TryMap(v => cplex.GetValue(v)).LogIfErr()
//    .Run((double x) => Console.WriteLine("val = " + x));

//Console.WriteLine(val);
