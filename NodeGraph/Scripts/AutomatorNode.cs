using Godot;
using System;
using System.Collections.Generic;

public class AutomatorNode : BaseNode
{
    private Vector2[] _keyframes = null;
    public AutomatorNode(Dictionary<int, BaseNode> left, double[] defaultValues, string id, Vector2[] keyframes) : base(left, defaultValues, id)
    {
        _keyframes = keyframes;
    }

    public override double Calculate(double[] args)
    {
        double time = Math.Clamp(Mathf.Remap(args[0], args[1], args[2], 0, 1), 0 + double.Epsilon, 1 - double.Epsilon);
        double height = 0;
        for(int i = 0; i < _keyframes.Length; i++)
        {
            if (_keyframes[i].X > time)
            {
                height = Mathf.Remap(time, _keyframes[i - 1].X, _keyframes[i].X, _keyframes[i - 1].Y, _keyframes[i].Y);
                break;
            }
        }
        return Mathf.Lerp(args[3], args[4], height);
    }
}
