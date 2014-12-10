using System;
using System.Collections.Generic;
using System.Data;

namespace GraphForm.Model
{
    [Serializable]
    class Floyd :GraphAlgorythmBase
    {
        private readonly string DISTANCE;
        private double[,] _adjMatrix;
        private int k;
        public Floyd(ref List<Node> graph) : base(ref graph)
        {
            _adjMatrix = new double[_graph.Count,_graph.Count];
            DISTANCE = "distance";
        }

        public override void Init()
        {
            if (IsRunning)
            {
                return;
            }

            k = 0;
            tables.Clear();
            tables.Add(DISTANCE, new DataTable());
            tables[DISTANCE].Columns.Add(new DataColumn("Csúcs", typeof(int)));
            
            //creating the adjacent matrix
            for (var i = 0; i < _graph.Count; ++i)
            {
                tables[DISTANCE].Columns.Add(new DataColumn((i+1).ToString(), typeof(double)));
                for (var j = 0; j < _graph.Count; ++j)
                {
                    if (i == j)
                    {
                        _adjMatrix[i, j] = 0;
                    }
                    else
                    {
                        _adjMatrix[i, j] = double.PositiveInfinity;
                    }
                }
            }

            foreach (var node in _graph)
            {
                foreach (var edge in node.Edges)
                {
                    _adjMatrix[edge.StartNode, edge.EndNode] = edge.Weight;
                }
            }

            for (var i = 0; i < _graph.Count; ++i)
            {
                tables[DISTANCE].Rows.Add();
                tables[DISTANCE].Rows[i][0] = i + 1;
                for (var j = 1; j <= _graph.Count; ++j)
                {
                    tables[DISTANCE].Rows[i][j] = _adjMatrix[i,j-1];
                }
            }

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

            for (var i = 0; i < _graph.Count; ++i)
            {
                for (var j = 0; j < _graph.Count; ++j)
                {
                    if (_adjMatrix[i, j] > _adjMatrix[i, k] + _adjMatrix[k, j])
                    {
                        _adjMatrix[i, j] = _adjMatrix[i, k] + _adjMatrix[k, j];
                        tables[DISTANCE].Rows[i][j + 1] = _adjMatrix[i, j];
                        RaiseTableEvent(DISTANCE);
                    }
                }
            }

            ++k;

            if (k == _graph.Count)
            {
                bool containsNegativeCycle = false;
                for (int i = 0; i < _graph.Count && !containsNegativeCycle; ++i)
                {
                    containsNegativeCycle = _adjMatrix[i, i] < 0;
                }

                if (containsNegativeCycle)
                {
                    RaiseNotifyEvent("A gráf tartalmaz negatív kört, az algoritmus nem megbízható ebben az esetben");
                }
            }
        }

        public override bool End()
        {
            return k == _graph.Count;
        }
    }
}
