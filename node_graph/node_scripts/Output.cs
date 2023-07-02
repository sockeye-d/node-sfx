using Godot;
using System;

public partial class Output : GraphNode, IOperatorGraphNode
{
    public int ArgumentCount => 1;

    public double Execute(double?[] args)
    {
        return(double)args[0];
    }
}
