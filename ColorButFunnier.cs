using Godot;
using System;

public partial class ColorButFunnier : ColorRect
{
	[Export]
	public double FUNNY_VALUE = 0.0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void DoFunny(Godot.Color color)
	{
		Color = color;
		GD.Print("Set color to ", color);
		GD.Print(FUNNY_VALUE);
	}
}
