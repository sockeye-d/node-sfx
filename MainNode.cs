using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class MainNode : Node2D
{
    [Export]
    double SAMPLE_RATE = 44100.0;

    public AudioStreamPlayer _player;
    public AudioStreamGeneratorPlayback _playbackStream;

    public double _phase = 0;

    public override void _Ready()
    {
        _player = GetNode<AudioStreamPlayer>("Player");
        _player.Play();
        _playbackStream = (AudioStreamGeneratorPlayback)_player.GetStreamPlayback();
    }

    public override void _Process(double delta)
    {
        double volume = GetNode<HSlider>("NodeGraph/VolumeSlider").Value;
        double increment = 440.0 / SAMPLE_RATE;
        double to_fill = _playbackStream.GetFramesAvailable();

        while (to_fill > 0)
        {
            _AddMono(Mathf.Sin(_phase * Mathf.Tau) * volume);
            _phase += increment % 1.0;


            to_fill--;
        }
        if (Input.IsActionJustPressed("run_tree"))
        {
            GD.Print(_GenerateTreeFromGodotConnections(GetNode<GraphEdit>("NodeGraph/GraphEdit").GetConnectionList(), "Output").ExecuteNode());
        }
    }

    private void _AddMono(double x)
    {
        _playbackStream.PushFrame(new Vector2((float)x, (float)x));
    }

    private GraphNode _GetGraphNode(string name)
    {
        return GetNode<GraphNode>($"NodeGraph/GraphEdit/{name}");
    }

    private double[] _GetNodeArgs(GraphNode node)
    {
        List<double> args = new List<double>();
        foreach (Node child in node.GetChildren().ToList().OrderBy(x => x.Name.ToString()))
        {
            if (child.Name.ToString().Contains("Port"))
            {
                args.Add(((Godot.Range)child).Value);
            }
        }
        return args.ToArray();
    }

    private BaseNode _GetNodeFromGraphNode(GraphNode node)
    {
        double[] args = _GetNodeArgs(node);
        NodePath nodePath = node.GetPath();
        string path = nodePath.GetName(nodePath.GetNameCount() - 1);
        string nodeType = path;
        if (nodeType.StartsWith("@"))
        {
            nodeType = nodeType.Split("@")[1];
        }
        switch (nodeType)
        {
            case "Math":
                return new MathNode(new(), args, path, node.GetNode<OptionButton>("Option").Selected);
            case "Output":
                return new OutputNode(new(), args, path);
            default:
                return null;
        }
    }

    private BaseNode _GenerateTreeFromGodotConnections(Godot.Collections.Array<Godot.Collections.Dictionary> connections, string current)
    {
        BaseNode currentNode = _GetNodeFromGraphNode(_GetGraphNode(current));
        foreach (var connection in connections)
        {
            if ((string)connection["to"] == current)
            {
                currentNode.Left.Add((int)connection["to_port"], _GenerateTreeFromGodotConnections(connections, (string)connection["from"]));
            }
        }
        return currentNode;
    }
}
