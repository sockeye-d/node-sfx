using Godot;
using Godot.NativeInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public override string ToString()
        {
            return $"From port {FromPort} on {From} to {ToPort} on {To}";
        }
    }

    [Export]
    double SAMPLE_RATE = 44100.0;

    AudioStreamPlayer _player;
    AudioStreamGeneratorPlayback _playbackStream;

    double _phase = 0;

    Godot.Collections.Array<Godot.Collections.Dictionary> _oldConnectionList;
    Dictionary<string, List<NodeConnection>> _nodeConnectionCache = null;

    public override void _Ready()
    {
        _oldConnectionList = GetNode<GraphEdit>("NodeGraph/GraphEdit").GetConnectionList();
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
            double treeRes = _ExecuteNodeTree(GetNode<GraphEdit>("NodeGraph/GraphEdit"));
            GetNode<Label>("Label").Text = treeRes.ToString();
        }
    }

    private void _AddMono(double x)
    {
        _playbackStream.PushFrame(new Vector2((float)x, (float)x));
    }

    private Dictionary<string, List<NodeConnection>> _GetNodeConnections(GraphEdit graph)
    {
        Godot.Collections.Array<Godot.Collections.Dictionary> tree = graph.GetConnectionList();
        Dictionary<string, List<NodeConnection>> connections = new Dictionary<string, List<NodeConnection>>();
        for (int i = 0; i < tree.Count; i++)
        {
            if (connections.TryGetValue((string)tree[i]["to"], out List<NodeConnection> nodeConnections))
            {
                nodeConnections.Add(new NodeConnection(tree[i]));
            }
            else
            {
                connections.Add((string)tree[i]["to"], new NodeConnection[] { new NodeConnection(tree[i]) }.ToList());
            }
        }
        return connections;
    }

    private double _ExecuteNodeTree(GraphEdit graph)
    {
        //if (!graph.GetConnectionList().RecursiveEqual(_oldConnectionList))
        //{
        //    _oldConnectionList = graph.GetConnectionList();
        //    _nodeConnectionCache = _GetNodeConnections(graph);
        //    GD.Print($"Graph changed");
        //    foreach (KeyValuePair<string, List<NodeConnection>> connection in _nodeConnectionCache)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        foreach (NodeConnection nodeConnection in connection.Value)
        //        {
        //            sb.Append(nodeConnection.ToString());
        //        }
        //        GD.Print(connection.Key, ": ", sb.ToString());
        //        GD.Print();
        //    }
        //    GD.Print(_nodeConnectionCache["Output"][0].ToString());
        //}
        Dictionary<string, List<NodeConnection>> tree = _GetNodeConnections(graph);
        //if (tree == null)
        //{
        //    return 0.0;
        //}
        return (double)_ExecuteNode(tree, tree["Output"][0].To);
    }

    private double? _ExecuteNode(Dictionary<string, List<NodeConnection>> tree, string path)
    {
        var currentNode = GetNode<IOperatorGraphNode>($"NodeGraph/GraphEdit/{path}");
        GD.Print(currentNode);
        double?[] args = new double?[currentNode.ArgumentCount];
        GD.Print(currentNode.ArgumentCount);
        for (int i = 0; i < args.Length; i++)
        {
            args[i] = null;// _ExecuteNode(tree, tree[path][i].From);
        }
        return currentNode.Execute(args);
    }
}
