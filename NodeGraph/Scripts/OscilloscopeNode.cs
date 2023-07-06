using Godot;
using System;
using System.Collections.Generic;

public partial class OscilloscopeNode : BaseNode
{
    private int _sampleCount;
    private float _heightScale;
    private float _heightOffset;
    private Queue<double> _queue = new Queue<double>();

    private Control _surface;

    public OscilloscopeNode(Dictionary<int, BaseNode> left, double[] defaultValues, string id, Control surface) : base(left, defaultValues, id)
    {
        _surface = surface;
        _surface.Draw += _DrawOscilloscope;
    }

    public override double Calculate(double[] args)
    {
        _sampleCount = (int)args[1];
        _heightScale = (float)args[2];
        _heightOffset = (float)(args[3] + 0.5);
        _queue.Enqueue(args[0]);
        while (_queue.Count > _sampleCount)
        {
            _queue.Dequeue();
        }
        return args[0];
    }

    private void _DrawOscilloscope()
    {
        if (_queue.Count > 1)
        {
            Vector2 surfaceSize = _surface.CustomMinimumSize;
            Vector2[] positions = new Vector2[_queue.Count];

            int x = 0;
            foreach (double y in _queue)
            {
                positions[x] = new Vector2(x / (float)(_sampleCount - 1), Math.Clamp((float)y * _heightScale + _heightOffset, 0, 1)) * surfaceSize;
                x++;
            }
            _surface.DrawPolyline(positions, Color.FromHsv(0.3f, 1.0f, 1.0f), 1, true);
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        _surface.Draw -= _DrawOscilloscope;
    }
}
