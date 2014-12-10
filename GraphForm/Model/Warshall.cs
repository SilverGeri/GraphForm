using System;
using System.Collections.Generic;
using System.Data;

namespace GraphForm.Model
{
    [Serializable]
    class Warshall : GraphAlgorythmBase
    {
        private bool[,] _adjMatrix;
        private int k;
        private string REACH;
        public Warshall(ref List<Node> graph) : base(ref graph)
        {
            REACH = "reach";
            _adjMatrix = new bool[_graph.Count,_graph.Count];
        }

        public override void Init()
        {
            if (IsRunning)
            {
                return;
            }

            k = 0;
            tables.Clear();
            tables.Add(REACH, new DataTable());
            tables[REACH].Columns.Add(new DataColumn("Csúcs", typeof(int)));

            //creating the adjacent matrix
            for (var i = 0; i < _graph.Count; ++i)
            {
                tables[REACH].Columns.Add(new DataColumn((i + 1).ToString(), typeof(string)));
                for (var j = 0; j < _graph.Count; ++j)
                {
                    if (i == j)
                    {
                        _adjMatrix[i, j] = true;
                    }
                    else
                    {
                        _adjMatrix[i, j] = false;
                    }
                }
            }

            foreach (var node in _graph)
            {
                foreach (var edge in node.Edges)
                {
                    _adjMatrix[edge.StartNode, edge.EndNode] = true;
                }
            }

            for (var i = 0; i < _graph.Count; ++i)
            {
                tables[REACH].Rows.Add();
                tables[REACH].Rows[i][0] = i + 1;
                for (var j = 1; j <= _graph.Count; ++j)
                {
                    if (_adjMatrix[i, j - 1])
                    {
                        tables[REACH].Rows[i][j] = "Igaz";
                    }
                    else
                    {
                        tables[REACH].Rows[i][j] = "Hamis";
                    }
                }
            }

            RaiseTableEvent(REACH, "Elérhetőségi tábla");
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

            for (var i = 0; i < _graph.Count; ++i)
            {
                for (var j = 0; j < _graph.Count; ++j)
                {
                    _adjMatrix[i, j] = _adjMatrix[i, j] || (_adjMatrix[i, k] && _adjMatrix[k, j]);

                    if (_adjMatrix[i, j])
                    {
                        tables[REACH].Rows[i][j + 1] = "Igaz";
                    }
                    else
                    {
                        tables[REACH].Rows[i][j + 1] = "Hamis";
                    }
                    RaiseTableEvent(REACH);
                }
            }

            ++k;
        }

        public override bool End()
        {
            return k == _graph.Count;
        }
    }
}
