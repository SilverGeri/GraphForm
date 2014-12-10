using System;
using System.Collections.Generic;
using System.Data;

namespace GraphForm.Model
{
    [Serializable]
    class BreadthSearch : GraphAlgorythmBase
    {
        private int _startNodeIndex;
        private Queue<Node> _queue;
        private readonly string PARENT;
        private readonly string DISTANCE;
        public BreadthSearch(ref List<Node> graph, int startNodeIndex) : base(ref graph)
        {
            _startNodeIndex = startNodeIndex;
            _queue = new Queue<Node>();
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
            for (var i=0; i<_graph.Count; ++i)
            {
                _graph[i].State = NodeState.White;
                RaiseNodeStateChangeEvent(i, _graph[i].State);
                tables[PARENT].Columns.Add(new DataColumn((i+1).ToString(), typeof(int)));
                tables[PARENT].Rows[0][i] = 0;
                tables[DISTANCE].Columns.Add(new DataColumn((i+1).ToString(), typeof(double)));
                tables[DISTANCE].Rows[0][i] = double.PositiveInfinity;
                foreach (var edge in _graph[i].Edges)
                {
                    edge.EdgeState = EdgeState.Default;
                    RaiseEdgeStateChangeEvent(edge.Id, edge.EdgeState);
                }
            }
            tables[DISTANCE].Rows[0][_startNodeIndex] = 0;
            RaiseTableEvent(PARENT, "Szülő");
            RaiseTableEvent(DISTANCE, "Távolság");
            _queue.Enqueue(_graph[_startNodeIndex]);
            _graph[_startNodeIndex].State = NodeState.Gray;
            RaiseNodeStateChangeEvent(_startNodeIndex, _graph[_startNodeIndex].State);
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
            
            var actNode = _queue.Dequeue();
            actNode.State = NodeState.Black;
            RaiseNodeStateChangeEvent(actNode.Index, actNode.State);
            tables[PARENT].Rows.Add();
            tables[DISTANCE].Rows.Add();
            for (var i = 0; i< tables[PARENT].Columns.Count; ++i)
            {
                tables[PARENT].Rows[tables[PARENT].Rows.Count - 1][i] =
                    tables[PARENT].Rows[tables[PARENT].Rows.Count - 2][i];
                tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 1][i] =
                    tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 2][i];
            }
            foreach (var edge in actNode.Edges)
            {
                if (_graph[edge.EndNode].State == NodeState.White)
                {
                    tables[PARENT].Rows[tables[PARENT].Rows.Count - 1][edge.EndNode] = edge.StartNode + 1;
                    tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 1][edge.EndNode] = (double)tables[DISTANCE].Rows[tables[DISTANCE].Rows.Count - 2][edge.StartNode] + edge.Weight;
                    _queue.Enqueue(_graph[edge.EndNode]);
                    _graph[edge.EndNode].State = NodeState.Gray;
                    RaiseNodeStateChangeEvent(edge.EndNode, _graph[edge.EndNode].State);
                    edge.EdgeState = EdgeState.Selected;
                    RaiseEdgeStateChangeEvent(edge.Id, edge.EdgeState);
                }
            }
            RaiseTableEvent(PARENT);
            RaiseTableEvent(DISTANCE);
        }

        public override bool End()
        {
            return _queue.Count == 0;
        }
    }
}
