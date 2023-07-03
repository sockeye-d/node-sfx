using Godot;
using System;
using System.Collections.Generic;

public partial class InlineComment : BaseNode
{
    public InlineComment(Dictionary<int, BaseNode> left, double[] defaultValues, string id) : base(left, defaultValues, id)
    {
    }

    public override double Calculate(double[] args)
    {
        return args[0];
    }
}
