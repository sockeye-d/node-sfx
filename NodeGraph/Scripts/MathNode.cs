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
