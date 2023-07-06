using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public partial class MainNode : Control
{
    [Export]
    double SAMPLE_RATE = 44100.0;

    public AudioStreamPlayer _player;
    public AudioStreamGeneratorPlayback _playbackStream;

    public double _time = 0;
    private BaseNode _nodeTree;

    public void OnNodeGraphConnectionChanged()
    {
        _GenerateTree();
    }

    public void OnRestartPressed()
    {
        _time = 0;
        _GenerateTree();
    }

    public void OnPlayPressed()
    {
        if (_player.Playing)
        {
            _player.Stop();
        }
        else
        {
            _player.Play();
            _GenerateTree();
            _playbackStream = (AudioStreamGeneratorPlayback)_player.GetStreamPlayback();
        }
    }


    public override void _Ready()
    {
        _player = GetNode<AudioStreamPlayer>("Player");
        _GenerateTree();
    }

    public override void _Process(double delta)
    {
        if (_player.Playing)
        {
            PlayGeneratedAudio(delta);
        }
        GetNode<TextureButton>("Ui/Restart").Disabled = !_player.Playing;
    }
    public void InitializeAudio()
    {
        _time = 0.0;
        _player.Play();
        _playbackStream = (AudioStreamGeneratorPlayback)_player.GetStreamPlayback();
        _player.Stop();
    }

    public void PlayGeneratedAudio(double deltaTime)
    {
        double volume = GetNode<HSlider>("Ui/VolumeSlider").Value;
        double increment = 1.0 / SAMPLE_RATE;
        BaseNode.DeltaTime = increment;
        double to_fill = Math.Min(_playbackStream.GetFramesAvailable(), Math.Round(1 / deltaTime) * SAMPLE_RATE);
        while (to_fill > 0)
        {
            BaseNode.Time = _time;
            double value = _nodeTree.ExecuteNode();
            _AddMono(value * volume);
            GetNode<Label>("Label").Text = value.ToString();
            _time += increment;
            to_fill--;
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
            case "WaveformGenerator":
                return new WaveformGeneratorNode(new(), args, path, node.GetNode<OptionButton>("Option").Selected);
            case "Time":
                return new TimeNode(new(), args, path);
            case "InlineComment":
                return new InlineComment(new(), args, path);
            case "Oscilloscope":
                return new OscilloscopeNode(new(), args, path, node.GetNode<Control>("Surface"));
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

    private void _GenerateTree()
    {
        _nodeTree?.Dispose();
        _nodeTree = _GenerateTreeFromGodotConnections(GetNode<GraphEdit>("NodeGraph/GraphEdit").GetConnectionList(), "Output");
    }
}
