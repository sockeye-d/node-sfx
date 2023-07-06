using Godot;
using System;
using System.Collections.Generic;
using System.Text;

public abstract class BaseNode : IDisposable
{
    public static double Time;
    public static double DeltaTime;
    public static double SampleRate;

    public string Identifier;
    public Dictionary<int, BaseNode> Left;
    public double[] NodeArgs;

    public virtual bool Drawable { get => false; }

    protected BaseNode(Dictionary<int, BaseNode> left, double[] defaultValues, string id)
    {
        Left = left;
        NodeArgs = defaultValues;
        Identifier = id;
    }

    public double ExecuteNode()
    {
        double[] args = new double[NodeArgs.Length];
        for (int i = 0; i < args.Length; i++)
        {
            if (Left.ContainsKey(i))
            {
                args[i] = Left[i].ExecuteNode();
            }
            else
            {
                args[i] = NodeArgs[i];
            }
        }

        return Calculate(args);
    }

    public abstract double Calculate(double[] args);

    public override string ToString()
    {
        StringBuilder sb = new();
        foreach (var kvp in Left)
        {
            sb.AppendLine($"{Identifier} is connnected to {kvp.Value.Identifier} on port {kvp.Key}");
            string previousNodeString = kvp.Value.ToString();
            foreach (string line in previousNodeString.Split('\n'))
            {
                if (!string.IsNullOrEmpty(line))
                {
                    sb.AppendLine($"\t{line}");
                }
            }
        }
        return $"{sb}";
    }

    public virtual void Dispose()
    {
        foreach (var kvp in Left)
        {
            kvp.Value.Dispose();
        }
    }
}
