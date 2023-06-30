using Godot;
using System;
using NodeSFX;

namespace NodeSFX.node_graph.node_scripts
{
    public partial class Math : GraphNode, IOperatorGraphNode
    {
        static private Func<double, double, double>[] functions =
        {
            (x, y) => x + y,
            (x, y) => x - y,
            (x, y) => x * y,
            (x, y) => x / y,
        };

        public object[] Execute(object[] args)
        {
            return new object[]
            {
                functions[GetNode<OptionButton>("Option").Selected].Invoke((double)args[0], (double)args[1])
            };
        }
    }
}