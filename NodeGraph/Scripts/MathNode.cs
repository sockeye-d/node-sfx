using System;
using System.Collections.Generic;

public class MathNode : BaseNode
{
    int _selectedFunction;

    private Func<double, double, double>[] _functions = new Func<double, double, double>[]
    {
        (x, y) => x + y,
        (x, y) => x - y,
        (x, y) => x * y,
        (x, y) => x / y,
        (x, y) => Math.Pow(x, y),
        (x, y) => Math.Sqrt(x),
        (x, y) => Math.Abs(x),
        (x, y) => Math.Min(x, y),
        (x, y) => Math.Max(x, y),
        (x, y) => x < y? 0 : 1,
        (x, y) => x > y? 0 : 1,
        (x, y) => Math.Sign(x),
        (x, y) => Math.Round(x),
        (x, y) => Math.Floor(x),
        (x, y) => Math.Ceiling(x),
        (x, y) => x % y,
        (x, y) => Math.Sin(x),
        (x, y) => Math.Cos(x),
        (x, y) => Math.Tan(x),
        (x, y) => Math.Asin(x),
        (x, y) => Math.Acos(x),
        (x, y) => Math.Atan(x),
    };

    public MathNode(Dictionary<int, BaseNode> left, double[] defaultValues, string id, int selectedFunction) : base(left, defaultValues, id)
    {
        _selectedFunction = selectedFunction;
    }

    public override double Calculate(double[] args)
    {
        return _functions[_selectedFunction].Invoke(args[0], args[1]);
    }
}
