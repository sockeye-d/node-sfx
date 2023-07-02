using Godot.Collections;
using System.Reflection.Metadata;

public interface IOperatorGraphNode
{
    public abstract int ArgumentCount { get; }
    public double Execute(double?[] args);
}
