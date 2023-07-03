using Godot;
using System;
using System.Collections.Generic;

public partial class TimeNode : BaseNode
{
    public TimeNode(Dictionary<int, BaseNode> left, double[] defaultValues, string id) : base(left, defaultValues, id)
    {
    }

    public override double Calculate(double[] args)
    {
        return Time;
    }
}
