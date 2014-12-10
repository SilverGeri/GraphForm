using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphForm.Model
{
    [Serializable]
    class Kruskal : GraphAlgorythmBase
    {
        private List<Edge> _edges;
        public Kruskal(ref List<Node> graph) : base(ref graph)
        {
            _edges = new List<Edge>();
        }

        public override void Init()
        {
            _edges.Clear();
            foreach (var node in _graph)
            {
                node.State = NodeState.White;
                RaiseNodeStateChangeEvent(node.Index, node.State);

                foreach (var edge in node.Edges)
                {
                    edge.EdgeState = EdgeState.Default;
                    RaiseEdgeStateChangeEvent(edge.Id, edge.EdgeState);
                    _edges.Add(edge);
                }
            }
            IsInitalised = true;
        }

        public override void Next()
        {
            if (End()) return;
            if (!IsInitalised)
            {
                RaiseNotifyEvent("Inicializálatlan algoritmus!");
                return;
            }
            var possibleEdges =
                _edges.FindAll(
                    x =>
                        x.EdgeState == EdgeState.Default &&
                        (_graph[x.StartNode].State == NodeState.White || _graph[x.EndNode].State == NodeState.White));
            var nextEdge = 0;
            for (var i = 1; i < possibleEdges.Count; ++i)
            {
                if (possibleEdges[i].Weight < possibleEdges[nextEdge].Weight)
                {
                    nextEdge = i;
                }
            }

            possibleEdges[nextEdge].EdgeState = EdgeState.Selected;
            RaiseEdgeStateChangeEvent(possibleEdges[nextEdge].Id, possibleEdges[nextEdge].EdgeState);

            _graph[possibleEdges[nextEdge].StartNode].State = NodeState.Black;
            RaiseNodeStateChangeEvent(possibleEdges[nextEdge].StartNode, _graph[possibleEdges[nextEdge].StartNode].State);
            _graph[possibleEdges[nextEdge].EndNode].State = NodeState.Black;
            RaiseNodeStateChangeEvent(possibleEdges[nextEdge].EndNode, _graph[possibleEdges[nextEdge].EndNode].State);
        }

        public override bool End()
        {
            return _graph.All(x => x.State == NodeState.Black);
        }
    }
}
