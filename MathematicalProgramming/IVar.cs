namespace MathematicalProgramming;

public enum VarType { Binary, Integer, Continuous }
public interface IVar
{
    string Key { get; }
    VarType VarType { get; }
    int Dim { get; }
}
public interface IVar2 : IVar { }
public interface IVar3 : IVar { }
public interface IVar4 : IVar { }
