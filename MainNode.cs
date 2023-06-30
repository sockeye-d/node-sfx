using Godot;
using Godot.NativeInterop;
using NodeSFX;
using System;
using System.Collections.Generic;

public partial class MainNode : Node2D
{
    private class NodeConnection
    {
        public string From;
        public int FromPort;
        public string To;
        public int ToPort;

        public NodeConnection(string from, int fromPort, string to, int toPort)
        {
            From = from;
            FromPort = fromPort;
            To = to;
            ToPort = toPort;
        }

        public NodeConnection(Godot.Collections.Dictionary dict)
        {
            From = (string)dict["from"];
            FromPort = (int)dict["from_port"];
            To = (string)dict["to"];
            ToPort = (int)dict["to_port"];
        }
    }

    [Export]
    double SAMPLE_RATE = 44100.0;

    AudioStreamPlayer _player;
    AudioStreamGeneratorPlayback _playbackStream;

    double _phase = 0;

    int _oldTreeHash;
    Dictionary<string, NodeConnection> _nodeConnectionCache = null;

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
    }

    private void _AddMono(double x)
    {
        _playbackStream.PushFrame(new Vector2((float)x, (float)x));
    }

    private Dictionary<string, NodeConnection> _GetNodeConnections(GraphEdit graph)
    {
        Godot.Collections.Array<Godot.Collections.Dictionary> tree = graph.GetConnectionList();
        Dictionary<string, NodeConnection> connections = new Dictionary<string, NodeConnection>();
        for(int i = 0; i < tree.Count; i++)
        {
            connections.Add((string)tree[i]["to"], new NodeConnection(tree[i]));
        }
        return connections;
    }

    private object[] _ExecuteNodeTree(GraphEdit graph)
    {
        if (graph.GetHashCode() != _oldTreeHash)
        {
            _nodeConnectionCache = _GetNodeConnections(graph);
        }
        Dictionary<string, NodeConnection> tree = _nodeConnectionCache;
        return _ExecuteNode(tree, tree["Output"].To);
    }

    private object[] _ExecuteNode(Dictionary<string, NodeConnection> tree, string path)
    {
        return GetNode<IOperatorGraphNode>(path).Execute(_ExecuteNode(tree, tree[path].From));
    }
}
