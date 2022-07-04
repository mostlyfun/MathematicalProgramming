namespace MathematicalProgramming.Helpers;

public class OptListEnumerator<T> : IEnumerator<T>
{
    // data
    readonly OptList<T> list;
    readonly int len;
    int ind;
    // ctor
    internal OptListEnumerator(OptList<T> list)
    {
        this.list = list;
        len = list.Length;
        ind = -1;
    }
    // method
    public T Current => list[ind];
    object? IEnumerator.Current => list[ind];
    public void Dispose() { }
    public bool MoveNext()
    {
        ind++;
        return ind < len;
    }
    public void Reset()
        => ind = -1;
}
