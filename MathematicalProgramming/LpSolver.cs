using System.Data;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MathematicalProgramming;

public class LpSolver : LpBuilder, IDisposable
{
    // enum
    enum Solver { Cplex }
    // data
    readonly Solver solver;
    readonly string pathSolver;
    readonly string pathLp, pathSol;
    Opt<DataSet> dsSol;
    public Opt<string> SolutionStatusString { get; private set; }
    public Opt<double> ObjectiveValue { get; private set; }


    // ctor
    LpSolver(string tmpFolder, Solver solver, string pathSolver)
    {
        this.solver = solver;
        this.pathSolver = pathSolver;

        string rnd = Path.GetRandomFileName();
        pathLp = Path.Combine(tmpFolder, rnd + ".lp");
        pathSol = Path.Combine(tmpFolder, rnd + ".sol");

        (dsSol, SolutionStatusString, ObjectiveValue) = ResetSoln();
    }
    (Opt<DataSet> xmlSol, Opt<string> solutionStatusString, Opt<double>) ResetSoln()
    {
        if (File.Exists(pathSol))
            File.Delete(pathSol);
        if (File.Exists(pathLp))
            File.Delete(pathLp);
        return (default, default, default);
    }
    public static Res<LpSolver> NewCplex(string tmpFolder, string pathCplex = "cplex")
    {
        if (tmpFolder == null)
            tmpFolder = "";
        var dirOk = Directory.Exists(tmpFolder) ? Ok() : Try(() => Directory.CreateDirectory(tmpFolder));
        if (dirOk.IsErr)
            return Err<LpSolver>(dirOk.ErrorMessage.Unwrap());
        return new LpSolver(tmpFolder, Solver.Cplex, pathCplex);
    }
    public void Dispose()
    {
        var _ = ResetSoln();
    }


    // method - solve
    protected internal override Res<bool> Solve(StringBuilder builtModel)
    {
        return solver switch
        {
            Solver.Cplex => SolveCplex(builtModel.Replace('*', ' ')),
            _ => false,
        };
    }
    Res<bool> SolveCplex(StringBuilder builtModel)
    {
        (dsSol, SolutionStatusString, ObjectiveValue) = ResetSoln();

        var commands = new string[]
        {
            $"read \"{pathLp}\"",
            "optimize",
            $"write \"{pathSol}\" sol",
        };
        string arguments =
            (false ? string.Join(' ', commands.Select(c => "-c " + c))
                        : string.Join(' ', commands));
        Console.WriteLine("ARGS: " + arguments);

        using var exec = new Execution();
        // todo: fix this!
        var cleanModel = builtModel
            .Replace("1,7976931348623157E+308", "inf")
            .Replace('*', ' ')
            .Replace("-0", "0")
            ;
        var write = Try(() => File.WriteAllText(pathLp, cleanModel.ToString()));
        if (write.IsErr)
            return Err<bool>("Failed to write the lp file. " + write.ErrorMessage.Unwrap());

        var run = exec.Exe(pathSolver, arguments);
        if (run.IsErr)
            return Err<bool>($"Failed to run {pathSol} for the lp file. {run.ErrorMessage.Unwrap()}");

        if (!File.Exists(pathSol))
            return Ok(false); // presolve - infeasible
            //return Err<bool>($"Failed to create solution for the lp file.");

        var resDataSet = TryMap(() =>
        {
            var xmlString = File.ReadAllText(pathSol);
            var stringReader = new StringReader(xmlString);
            var dsSet = new DataSet();
            dsSet.ReadXml(stringReader);
            return dsSet;
        });
        if (resDataSet.IsErr)
            return Err<bool>($"Failed to parse the solution file {pathSol}. {resDataSet.ErrorMessage.Unwrap()}");
        var sol = resDataSet.Unwrap();
        dsSol = Some(sol);

        var resStatus = Try(() =>
        {
            var header = sol.Tables["header"];
            if (header == null)
                throw new Exception("Cannot find header in solution file.");
            if (header.Rows.Count != 1)
                throw new Exception("Error reading the solution.");
            var headerRow = header.Rows[0];
            SolutionStatusString = Some((string)headerRow["solutionStatusString"]);
            var objVal = (string)headerRow["objectiveValue"];
            if (double.TryParse(objVal, out var val))
                ObjectiveValue = Some(val);
        });
        if (resStatus.IsErr)
            return Err<bool>($"Failed to parse status from the solution file {pathSol}. {resStatus.ErrorMessage.Unwrap()}");


        return Ok(IsSolved(SolutionStatusString.Unwrap()));
    }
    static bool IsSolved(string solutionStatusString)
    {
        return solutionStatusString == "optimal"
            || solutionStatusString == "integer optimal solution"
            || solutionStatusString == "integer optimal, tolerance"
            || solutionStatusString.Contains("optimal");
    }
    class Execution : IDisposable
    {
        // data
        bool disposed;
        Opt<Process> maybeProcess;


        // ctor
        public Execution()
        {
            disposed = false;
            maybeProcess = None<Process>();
        }


        // method
        public Res Exe(string prog, string args)
        {
            using var process = new Process();
            maybeProcess = process;
            process.StartInfo.FileName = prog;
            process.StartInfo.Arguments = args;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            try
            {
                process.WaitForExit();
                maybeProcess = None<Process>();
                return Ok();
            }
            catch (Exception e)
            {
                process.Kill();
                return Err(e.Message + "\n" + e.InnerException?.Message + "\n" + e.StackTrace);
            }
        }
        public void Dispose()
        {
            if (disposed)
                return;

            if (maybeProcess.IsSome)
            {
                var pr = maybeProcess.Unwrap();
                pr.Kill();
                pr.Dispose();
            }


            disposed = true;
            GC.SuppressFinalize(this);
        }
    }


    // method - get - value
    DataTable GetVariableDt()
        => dsSol.Unwrap().Tables["variable"];
    public override double GetVal0(string keyVar0)
    {
        var variable = GetVariableDt();
        var maybeRow = variable.Select(string.Format("name = '{0}'", keyVar0)).Take(1).FirstOrNone();
        var row = maybeRow.UnwrapOrThrow("Cannot find variable " + keyVar0);
        return double.Parse((string)row["value"]);
    }
    public override double[] GetVal1(string keyVar1, int len1)
    {
        // todo: no need for list if we know the size beforehand
        var variable = GetVariableDt();
        string beg = keyVar1 + "(";
        var rows = variable.AsEnumerable().Where(row => ((string)row["name"]).StartsWith(beg));
        var arr = new double[len1];
        foreach (DataRow row in rows)
        {
            var name = ((string)row["name"]).AsSpan();
            var sind = name.Slice(beg.Length, name.Length - beg.Length - 1);
            int i = int.Parse(sind);
            arr[i] = double.Parse((string)row["value"]);
        }
        return arr;
    }
    public override double[][] GetVal2(string keyVar2, int len1, int len2)
    {
        var variable = GetVariableDt();
        string beg = keyVar2 + "(";
        var rows = variable.AsEnumerable().Where(row => ((string)row["name"]).StartsWith(beg));
        var arr = new double[len1][];
        for (int i = 0; i < arr.Length; i++)
            arr[i] = new double[len2];
        FillVal2(beg, arr, rows);
        return arr;
    }
    public override double[][] GetJagVal2(string keyJagVar2, int len1, Func<int, int> getLen2)
    {
        var variable = GetVariableDt();
        string beg = keyJagVar2 + "(";
        var rows = variable.AsEnumerable().Where(row => ((string)row["name"]).StartsWith(beg));
        var arr = new double[len1][];
        for (int i = 0; i < arr.Length; i++)
            arr[i] = new double[getLen2(i)];
        FillVal2(beg, arr, rows);
        return arr;
    }
    static void FillVal2(string beg, double[][] arr, EnumerableRowCollection<DataRow> rows)
    {
        foreach (DataRow row in rows)
        {
            var name = ((string)row["name"]).AsSpan();
            var sind = name.Slice(beg.Length, name.Length - beg.Length - 1);
            int com1 = sind.IndexOf(',');
            int i = int.Parse(sind[..com1]);
            int j = int.Parse(sind[(com1 + 1)..]);
            arr[i][j] = double.Parse((string)row["value"]);
        }
    }
    public override double[][][] GetVal3(string keyVar3, int len1, int len2, int len3)
    {
        var variable = GetVariableDt();
        string beg = keyVar3 + "(";
        var rows = variable.AsEnumerable().Where(row => ((string)row["name"]).StartsWith(beg));
        var arr = new double[len1][][];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = new double[len2][];
            for (int j = 0; j < arr[i].Length; j++)
                arr[i][j] = new double[len3];
        }
        FillVal3(beg, arr, rows);
        return arr;
    }
    public override double[][][] GetJagVal3(string keyJagVar3, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3)
    {
        var variable = GetVariableDt();
        string beg = keyJagVar3 + "(";
        var rows = variable.AsEnumerable().Where(row => ((string)row["name"]).StartsWith(beg));
        var arr = new double[len1][][];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = new double[getLen2(i)][];
            for (int j = 0; j < arr[i].Length; j++)
                arr[i][j] = new double[getLen3(i, j)];
        }
        FillVal3(beg, arr, rows);
        return arr;
    }
    static void FillVal3(string beg, double[][][] arr, EnumerableRowCollection<DataRow> rows)
    {
        foreach (DataRow row in rows)
        {
            var name = ((string)row["name"]).AsSpan();
            var sind = name.Slice(beg.Length, name.Length - beg.Length - 1);
            int com1 = sind.IndexOf(',');
            int i = int.Parse(sind[..com1]);
            
            sind = sind[(com1 + 1)..];
            int com2 = sind.IndexOf(',');
            int j = int.Parse(sind[..com2]);
            int k = int.Parse(sind[(com2 + 1)..]);

            arr[i][j][k] = double.Parse((string)row["value"]);
        }
    }
    public override double[][][][] GetVal4(string keyVar4, int len1, int len2, int len3, int len4)
    {
        var variable = GetVariableDt();
        string beg = keyVar4 + "(";
        var rows = variable.AsEnumerable().Where(row => ((string)row["name"]).StartsWith(beg));
        var arr = new double[len1][][][];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = new double[len2][][];
            for (int j = 0; j < arr[i].Length; j++)
            {
                arr[i][j] = new double[len3][];
                for (int k = 0; k < arr[i][j].Length; k++)
                    arr[i][j][k] = new double[len4];
            }
        }
        FillVal4(beg, arr, rows);
        return arr;
    }
    public override double[][][][] GetJagVal4(string keyJagVar4, int len1, Func<int, int> getLen2, Func<int, int, int> getLen3, Func<int, int, int, int> getLen4)
    {
        var variable = GetVariableDt();
        string beg = keyJagVar4 + "(";
        var rows = variable.AsEnumerable().Where(row => ((string)row["name"]).StartsWith(beg));
        var arr = new double[len1][][][];
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = new double[getLen2(i)][][];
            for (int j = 0; j < arr[i].Length; j++)
            {
                arr[i][j] = new double[getLen3(i, j)][];
                for (int k = 0; k < arr[i][j].Length; k++)
                    arr[i][j][k] = new double[getLen4(i, j, k)];
            }
        }
        FillVal4(beg, arr, rows);
        return arr;
    }
    static void FillVal4(string beg, double[][][][] arr, EnumerableRowCollection<DataRow> rows)
    {
        foreach (DataRow row in rows)
        {
            var name = ((string)row["name"]).AsSpan();
            var sind = name.Slice(beg.Length, name.Length - beg.Length - 1);
            int com1 = sind.IndexOf(',');
            int i = int.Parse(sind[..com1]);

            sind = sind[(com1 + 1)..];
            int com2 = sind.IndexOf(',');
            int j = int.Parse(sind[..com2]);

            sind = sind[(com2 + 1)..];
            int com3 = sind.IndexOf(',');
            int k = int.Parse(sind[..com3]);
            int l = int.Parse(sind[(com3 + 1)..]);

            arr[i][j][k][l] = double.Parse((string)row["value"]);
        }
    }
}
