using System;
using System.Collections.Generic;
using System.Data;

namespace GraphForm.Model
{
    [Serializable]
    class BellmanFord : GraphAlgorythmBase
    {
        private int _startNodeIndex;
        private int _counter;
        private readonly string PARENT;
        private readonly string DISTANCE;
        private List<Edge> _edges;
        private List<Edge> _parentEdges;
        public BellmanFord(ref List<Node> graph, int startNodeIndex) : base(ref graph)
        {
            _startNodeIndex = startNodeIndex;
            _counter = 0;
            _edges = new List<Edge>();
            _parentEdges = new List<Edge>();
            PARENT = "parent";
            DISTANCE = "distance";
        }

        public override void Init()
        {
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
                    _edges.Add(edge);
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

            bool isValid = true;

            tables[PARENT].Rows.Add();
            tables[DISTANCE].Rows.Add();
            for (var i = 0; i < tables[PARENT].Columns.Count; ++i)
            {
                tables[PARENT].Rows[tables[PARENT].Rows.Count - 1][i] =
                    tables[PARENT].Rows[tables[PARENT].Rows.Count - 2][i];
                tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 1][i] =
                    tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 2][i];
            }
            RaiseTableEvent(PARENT);
            RaiseTableEvent(DISTANCE);

            foreach (var edge in _edges)
            {
                if ((double) tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 1][edge.EndNode] >
                    (double) tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 1][edge.StartNode] + edge.Weight)
                {
                    if (_counter == _graph.Count - 1)
                    {
                        isValid = false;
                    }

                    if (_parentEdges[edge.EndNode] != null)
                    {
                        _parentEdges[edge.EndNode].EdgeState = EdgeState.Default;
                        RaiseEdgeStateChangeEvent(_parentEdges[edge.EndNode].Id, _parentEdges[edge.EndNode].EdgeState);
                    }

                    tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 1][edge.EndNode] =
                        (double)tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 1][edge.StartNode] +
                        edge.Weight;
                    tables[PARENT].Rows[tables[PARENT].Rows.Count - 1][edge.EndNode] = edge.StartNode + 1;
                    _parentEdges[edge.EndNode] = edge;
                    RaiseTableEvent(PARENT);
                    RaiseTableEvent(DISTANCE);
                    edge.EdgeState = EdgeState.Selected;
                    RaiseEdgeStateChangeEvent(edge.Id, edge.EdgeState);
                }
            }

            RaiseTableEvent(PARENT);
            RaiseTableEvent(DISTANCE);

            ++_counter;
            if (!isValid)
            {
                RaiseNotifyEvent("A gráf tartalmaz negatív kört, az algoritmus nem megbízható ebben az esetben");
            }
        }

        public override bool End()
        {
            return _counter == _graph.Count;
        }
    }
}
