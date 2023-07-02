using System.Collections.Generic;

public class OutputNode : BaseNode
{
    public OutputNode(Dictionary<int, BaseNode> left, double[] defaultValues, string id) : base(left, defaultValues, id)
    {
    }

    public override double Calculate(double[] args)
    {
        return args[0];
    }
}
