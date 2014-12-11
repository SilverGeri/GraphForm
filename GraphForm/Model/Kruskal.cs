using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace GraphForm.Model
{
    [Serializable]
    class Kruskal : GraphAlgorythmBase
    {
        private List<Edge> _edges;
        private List<HashSet<Node>> _sets;
        private bool _end;
        public Kruskal(ref List<Node> graph) : base(ref graph)
        {
            _edges = new List<Edge>();
            _sets = new List<HashSet<Node>>();
        }

        public override void Init()
        {
            _edges.Clear();
            _end = false;
            foreach (var node in _graph)
            {
                node.State = NodeState.White;
                RaiseNodeStateChangeEvent(node.Index, node.State);

                _sets.Add(new HashSet<Node>());
                _sets[node.Index].Add(node);

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
                        (_sets.FindIndex(y=> y.Contains(_graph[x.StartNode])) != _sets.FindIndex(y=> y.Contains(_graph[x.EndNode]))));

            if (possibleEdges.Count == 0)
            {
                _end = true;
                return;
            }
            var nextEdge = 0;
            for (var i = 1; i < possibleEdges.Count; ++i)
            {
                if (possibleEdges[i].Weight < possibleEdges[nextEdge].Weight)
                {
                    nextEdge = i;
                }
            }

            int startSet = _sets.FindIndex(y => y.Contains(_graph[possibleEdges[nextEdge].StartNode]));
            int endSet = _sets.FindIndex(y => y.Contains(_graph[possibleEdges[nextEdge].EndNode]));

            if (startSet < endSet)
            {
                foreach (var node in _sets[endSet])
                {
                    _sets[startSet].Add(node);
                }
                _sets[endSet].Clear();
            }
            else
            {
                foreach (var node in _sets[startSet])
                {
                    _sets[endSet].Add(node);
                }
                _sets[startSet].Clear();
            }

            possibleEdges[nextEdge].EdgeState = EdgeState.Selected;
            RaiseEdgeStateChangeEvent(possibleEdges[nextEdge].Id, possibleEdges[nextEdge].EdgeState);
        }

        public override bool End()
        {
            //all nodes are in the same set
            return _end;
        }
    }
}
