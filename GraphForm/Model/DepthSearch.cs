using System;
using System.Collections.Generic;
using System.Data;

namespace GraphForm.Model
{
    [Serializable]
    class DepthSearch : GraphAlgorythmBase
    {
        private int _startNodeIndex;
        private int _depth;
        private int _finish;
        private Stack<Node> _stack;
        private readonly string DATA;
        public DepthSearch(ref List<Node> graph, int startNodeIndex) : base(ref graph)
        {
            _stack = new Stack<Node>();
            _startNodeIndex = startNodeIndex;
            _depth = 1;
            _finish = 1;
            DATA = "data";
        }
        
        public override void Init()
        {
            if (IsRunning)
            {
                return;
            }

            tables.Clear();
            tables.Add(DATA, new DataTable());
            tables[DATA].Columns.Add(new DataColumn("Csúcs", typeof(int)));
            tables[DATA].Columns.Add(new DataColumn("Mélység", typeof(int)));
            tables[DATA].Columns.Add(new DataColumn("Befejezés", typeof(int)));
            tables[DATA].Columns.Add(new DataColumn("Szülő", typeof(int)));
            for (var i = 0; i < _graph.Count; ++i)
            {
                _graph[i].State = NodeState.White;
                RaiseNodeStateChangeEvent(i, _graph[i].State);
                var row = tables[DATA].NewRow();
                row[0] = i + 1;
                row[1] = 0;
                row[2] = 0;
                row[3] = 0;
                tables[DATA].Rows.Add(row);

                foreach (var edge in _graph[i].Edges)
                {
                    edge.EdgeState = EdgeState.Default;
                    RaiseEdgeStateChangeEvent(edge.Id, edge.EdgeState);
                }
            }
            tables[DATA].Rows[_startNodeIndex][1] = _depth;
            ++_depth;
            RaiseTableEvent(DATA, "Adatok");
            _stack.Push(_graph[_startNodeIndex]);
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

            Node actnode = _stack.Peek();
            actnode.State = NodeState.Gray;
            RaiseNodeStateChangeEvent(actnode.Index, actnode.State);
            var nextNodeEdge = actnode.Edges.FindIndex(x => _graph[x.EndNode].State == NodeState.White);

            if (nextNodeEdge == -1) //no more white neighbour
            {
                _stack.Pop();
                tables[DATA].Rows[actnode.Index][2] = _finish;
                RaiseTableEvent(DATA);
                ++_finish;
                actnode.State = NodeState.Black;
                RaiseNodeStateChangeEvent(actnode.Index, actnode.State);
            }
            else
            {
                actnode.Edges[nextNodeEdge].EdgeState = EdgeState.Selected;
                RaiseEdgeStateChangeEvent(actnode.Edges[nextNodeEdge].Id, actnode.Edges[nextNodeEdge].EdgeState);
                _stack.Push(_graph[actnode.Edges[nextNodeEdge].EndNode]);
                tables[DATA].Rows[_stack.Peek().Index][1] = _depth;
                ++_depth;
                tables[DATA].Rows[_stack.Peek().Index][3] = actnode.Index + 1;
                RaiseTableEvent(DATA);
                _stack.Peek().State = NodeState.Gray;
                RaiseNodeStateChangeEvent(_stack.Peek().Index, _stack.Peek().State);
            }
        }

        public override bool End()
        {
            return _graph[_startNodeIndex].State == NodeState.Black;
        }
    }
}
