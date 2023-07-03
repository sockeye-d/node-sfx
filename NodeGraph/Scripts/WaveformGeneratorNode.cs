using Godot;
using System;
using System.Collections.Generic;

public class WaveformGeneratorNode : BaseNode
{
    int _selectedFunction;

    private Func<double, double, double, double, double>[] _functions = new Func<double, double, double, double, double>[]
    {
        (x, freq, amp, phase) => Math.Sin(Mathf.Tau * x * freq + Mathf.Tau * phase) * amp, // sine wave
        (x, freq, amp, phase) => (4 * Math.Abs((x * freq - 0.25 + phase) % 1 - 0.5) - 1) * amp, // Triangle wave
        (x, freq, amp, phase) => (2 * ((freq * x + phase) % 1) - 1) * amp, // Sawtooth wave
        (x, freq, amp, phase) => (2 * Math.Floor(2 * ((x * freq + phase) % 1)) - 1) * amp, // Square wave
    };

    public WaveformGeneratorNode(Dictionary<int, BaseNode> left, double[] defaultValues, string id, int selectedFunction) : base(left, defaultValues, id)
    {
        _selectedFunction = selectedFunction;
    }

    public override double Calculate(double[] args)
    {
        return _functions[_selectedFunction].Invoke(Time, args[0], args[1], args[2]);
    }
}
