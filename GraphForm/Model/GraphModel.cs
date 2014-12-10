using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphForm.Model   
{
    [Serializable]
    public delegate void NodeEventHandler(object source, NodeEventArgs e);
    [Serializable]
    public delegate void EdgeEventHandler(object source, EdgeEventArgs e);
    [Serializable]
    public delegate void StartNodeChangeEventHandler(object source, StartNodeChangeEventArgs e);

    [Serializable]
    public class NodeEventArgs : EventArgs
    {
        public string Text { get; private set; }

        public NodeEventArgs(string text)
        {
            Text = text;
        }
    }

    [Serializable]
    public class EdgeEventArgs : EventArgs
    {
        public int StartNode { get; private set; }
        public int EndNode { get; private set; }
        public double Weight { get; private set; }
        public int Id { get; private set; }

        public EdgeEventArgs(int startNode, int endNode, double weight, int id)
        {
            Weight = weight;
            EndNode = endNode;
            StartNode = startNode;
            Id = id;
        }
    }

    [Serializable]
    public class StartNodeChangeEventArgs : EventArgs
    {
        public int? PrevStartNode;
        public int? NextStartNode;

        public StartNodeChangeEventArgs(int? prevStartNode, int? nextStartNode)
        {
            PrevStartNode = prevStartNode;
            NextStartNode = nextStartNode;
        }
    }

    [Serializable]
    class GraphModel
    {
        public bool IsDirected { get; private set; }
        public event NodeEventHandler AddNodeEvent;
        public event EdgeEventHandler AddEdgeEvent;
        public event MessageEventHandler NotifyEvent;
        public event StartNodeChangeEventHandler StartNodeChanged;
        public event TableEventHandler TableEvent;
        public event NodeStateChangeEventHandler NodeStateChangeEvent;
        public event EdgeStateChangeEventHandler EdgeStateChangeEvent;
        private List<Node> graphList;
        private GraphAlgorythmBase algorythm;
        private int _id;
        private int _edgeId;
        private int? _startNodeIndex;
        public GraphModel(bool isDirected)
        {
            _id = 0;
            _edgeId = 0;
            IsDirected = isDirected;
            graphList = new List<Node>();
        }

        public void AddNode()
        {
            if (AddNodeEvent != null) AddNodeEvent(this, new NodeEventArgs((_id + 1).ToString()));
            graphList.Add(new Node(_id));
            
            ++_id;
        }

        public void AddEdge(int startNode, int endNode, double weight)
        {
            string error = "";
            if (startNode < 1 || startNode > graphList.Count)
            {
                error += "Érvénytelen kezdőcsúcs\n";
            }

            if (endNode < 1 || endNode > graphList.Count)
            {
                error += "Érvénytelen végcsúcs\n";
            }

            if (endNode == startNode)
            {
                error += "Hurokél nem támogatott!\n";
            }

            if (graphList[startNode - 1].Edges.Any(x => x.EndNode == endNode - 1))
            {
                error += "Többszörös élek nem támogatottak\n";
            }

            if (error != "")
            {
                if (NotifyEvent != null) NotifyEvent(this, new MessageEventArgs(error, "Hiba"));
            }
            else
            {
                graphList[startNode - 1].Edges.Add(new Edge(startNode-1, endNode-1, weight, _edgeId));
                if (!IsDirected)
                {
                    graphList[endNode - 1].Edges.Add(new Edge(endNode - 1, startNode - 1, weight, _edgeId));
                }
                if (AddEdgeEvent != null) AddEdgeEvent(this, new EdgeEventArgs(startNode, endNode, weight, _edgeId));
                ++_edgeId;
            }
        }

        /*
         * the ids must match in view and model
         * 0: no algorythm is selected (null)
         * 1: breadthSearch
         * 2: depthSearch
         * 3: Dijkstra
         * 4: Kruskal
         * 5: Bellman-Ford
         * 6: Floyd
         * 7: Warshall
         */
        public void Start(int index)
        {
            if (algorythm != null && algorythm.IsRunning)
            {
                return;
            }
            SetDefault();
            switch (index)
            {
                case 0:
                    algorythm = null;
                    break;
                case 1:
                    if (_startNodeIndex == null)
                    {
                        algorythm = null;
                        if (NotifyEvent != null)
                            NotifyEvent(this, new MessageEventArgs("Válassz egy kezdőcsúcsot!", "Hiba"));
                    }
                    else
                    {
                        algorythm = new BreadthSearch(ref graphList, (int) _startNodeIndex);
                    }
                    break;
                case 2:
                    if (_startNodeIndex == null)
                    {
                        algorythm = null;
                        if (NotifyEvent != null)
                            NotifyEvent(this, new MessageEventArgs("Válassz egy kezdőcsúcsot!", "Hiba"));
                    }
                    else
                    {
                        algorythm = new DepthSearch(ref graphList, (int) _startNodeIndex);
                    }
                    break;
                case 3:
                    if (_startNodeIndex == null)
                    {
                        algorythm = null;
                        if (NotifyEvent != null)
                            NotifyEvent(this, new MessageEventArgs("Válassz egy kezdőcsúcsot!", "Hiba"));
                    }
                    else
                    {
                        algorythm = new Dijkstra(ref graphList, (int) _startNodeIndex);
                    }
                    break;
                case 4:
                    algorythm = new Kruskal(ref graphList);
                    break;
                case 5:
                    if (_startNodeIndex == null)
                    {
                        algorythm = null;
                        if (NotifyEvent != null)
                            NotifyEvent(this, new MessageEventArgs("Válassz egy kezdőcsúcsot!", "Hiba"));
                    }
                    else
                    {
                        algorythm = new BellmanFord(ref graphList, (int) _startNodeIndex);
                    }
                    break;
                case 6:
                    algorythm = new Floyd(ref graphList);
                    break;
                case 7:
                     algorythm = new Warshall(ref graphList);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(index.ToString());
            }

            if (algorythm != null)
            {
                if (TableEvent != null) algorythm.TableEvent += (source, args) => TableEvent(source, args);
                if (NodeStateChangeEvent != null)
                    algorythm.NodeStateChangeEvent += (source, args) => NodeStateChangeEvent(source, args);
                if (NotifyEvent != null) algorythm.NotifyEvent += (source, args) => NotifyEvent(source, args);
                if (EdgeStateChangeEvent != null)
                    algorythm.EdgeStateChangeEvent += (source, args) => EdgeStateChangeEvent(source, args);
                algorythm.Init();
            }
        }

        public void SetStartNode(int? startNodeIndex)
        {
            int? nextStartNode = startNodeIndex == _startNodeIndex ? null : startNodeIndex;
            if (StartNodeChanged != null)
                StartNodeChanged(this, new StartNodeChangeEventArgs(_startNodeIndex, nextStartNode));
            _startNodeIndex = nextStartNode;
        }

        public void Finish(int value)
        {
            if (algorythm != null && !algorythm.IsRunning)
            algorythm.Finish(value);
        }

        public void Next()
        {
            if (algorythm != null && !algorythm.IsRunning)
            {
                algorythm.Next();
            }
        }

        public void SetDefault()
        {
            algorythm = null;
            foreach (var node in graphList)
            {
                node.State = NodeState.White;
                if (NodeStateChangeEvent != null)
                    NodeStateChangeEvent(this, new NodeStateChangeEventArgs(node.Index, node.State));
                foreach (var edge in node.Edges)
                {
                    edge.EdgeState = EdgeState.Default;
                    if (EdgeStateChangeEvent != null)
                        EdgeStateChangeEvent(this, new EdgeStateChangeEventArgs(edge.Id, edge.EdgeState));
                }
            }
        }
    }
}
