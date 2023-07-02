using Godot;
using System;

public partial class Math : GraphNode, IOperatorGraphNode
{
    public int ArgumentCount => 2;

    private static readonly Func<double, double, double>[] functions =
    {
            (x, y) => x + y,
            (x, y) => x - y,
            (x, y) => x * y,
            (x, y) => x / y,
        };

    public double Execute(double?[] args)
    {
        double[] inputs = new double[args.Length];
        for (int i = 0; i < inputs.Length; i++)
        {
            if (args[i] == null)
            {
                inputs[i] = GetNode<SpinBox>($"Port{i}").Value;
            }
            else
            {
                inputs[i] = (double)args[i];
            }
        }
        return functions[GetNode<OptionButton>("Option").Selected].Invoke(inputs[0], inputs[1]);
    }

    public override void _Process(double delta)
    {
        NodePath path = GetPath();
        Title = path.GetName(path.GetNameCount() - 1);
    }
}
