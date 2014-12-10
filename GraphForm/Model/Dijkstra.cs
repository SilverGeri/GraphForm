using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GraphForm.Model
{
    [Serializable]
    class Dijkstra : GraphAlgorythmBase
    {
        private int _startNodeIndex;
        private List<Edge> _parentEdges;
        private readonly string PARENT;
        private readonly string DISTANCE;
        public Dijkstra(ref List<Node> graph, int startNodeIndex) : base(ref graph)
        {
            _startNodeIndex = startNodeIndex;
            _parentEdges = new List<Edge>();
            PARENT = "parent";
            DISTANCE = "distance";
        }

        public override void Init()
        {
            // there must be no edge width negative weight
            if (_graph.Any(x => x.Edges.Any(y => y.Weight < 0)))
            {
                RaiseNotifyEvent("Nem lehetnek negatív élsúlyok");
                return;
            }

            if (IsRunning)
            {
                return;
            }

            tables.Clear();
            tables.Add(PARENT, new DataTable());
            tables.Add(DISTANCE, new DataTable());
            tables[PARENT].Rows.Add();
            tables[DISTANCE].Rows.Add();
            for (var i = 0; i < _graph.Count; ++i)
            {
                _graph[i].State = NodeState.White;
                RaiseNodeStateChangeEvent(i, _graph[i].State);
                tables[PARENT].Columns.Add(new DataColumn((i + 1).ToString(), typeof(int)));
                tables[PARENT].Rows[0][i] = 0;
                tables[DISTANCE].Columns.Add(new DataColumn((i + 1).ToString(), typeof(double)));
                tables[DISTANCE].Rows[0][i] = double.PositiveInfinity;
                foreach (var edge in _graph[i].Edges)
                {
                    edge.EdgeState = EdgeState.Default;
                    RaiseEdgeStateChangeEvent(edge.Id, edge.EdgeState);
                }
                _parentEdges.Add(null);
            }
            tables[DISTANCE].Rows[0][_startNodeIndex] = 0;
            RaiseTableEvent(PARENT, "Szülő");
            RaiseTableEvent(DISTANCE, "Távolság");
            IsInitalised = true;
        }

        public override void Next()
        {
            if (End())
            {
                return;
            }

            if (!IsInitalised)
            {
                RaiseNotifyEvent("Inicializálatlan algoritmus!");
                return;
            }

            tables[PARENT].Rows.Add();
            tables[DISTANCE].Rows.Add();
            for (var i = 0; i < tables[PARENT].Columns.Count; ++i)
            {
                tables[PARENT].Rows[tables[PARENT].Rows.Count - 1][i] =
                    tables[PARENT].Rows[tables[PARENT].Rows.Count - 2][i];
                tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 1][i] =
                    tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 2][i];
            }

            int actNodeIndex = FindMinIndex();
            foreach (var edge in _graph[actNodeIndex].Edges)
            {
                if (_graph[edge.EndNode].State == NodeState.White &&
                    (double) tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 1][edge.EndNode] >
                    ((double) tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 1][edge.StartNode] + edge.Weight))
                {
                    // we have to change the state back
                    if (_parentEdges[edge.EndNode] != null)
                    {
                        _parentEdges[edge.EndNode].EdgeState = EdgeState.Default;
                        RaiseEdgeStateChangeEvent(_parentEdges[edge.EndNode].Id, _parentEdges[edge.EndNode].EdgeState);
                    }

                    _parentEdges[edge.EndNode] = edge;

                    tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 1][edge.EndNode] =
                        (double) tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 2][edge.StartNode] +
                        edge.Weight;
                    tables[PARENT].Rows[tables[PARENT].Rows.Count - 1][edge.EndNode] = edge.StartNode + 1;
                    edge.EdgeState = EdgeState.Selected;
                    RaiseEdgeStateChangeEvent(edge.Id, edge.EdgeState);
                }
            }
            RaiseTableEvent(PARENT);
            RaiseTableEvent(DISTANCE);
            _graph[actNodeIndex].State = NodeState.Black;
            RaiseNodeStateChangeEvent(_graph[actNodeIndex].Index, _graph[actNodeIndex].State);
        }

        public override bool End()
        {
            return _graph.All(x => x.State == NodeState.Black);
        }

        private int FindMinIndex()
        {
            var min = (double)tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 1][0];
            int retVal = _graph.FindIndex(x => x.State == NodeState.White);
            for (int i = retVal + 1; i < tables[DISTANCE].Columns.Count; ++i)
            {
                if (_graph[i].State == NodeState.White && (double)tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 1][i] < min)
                {
                    min = (double)tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 1][i];
                    retVal = i;
                }
            }
            return retVal;
        }
    }
}
