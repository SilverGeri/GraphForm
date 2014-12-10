using System;
using System.Collections.Generic;
using GraphForm.Model;

namespace GraphForm.View
{
    [Serializable]
    class SaveHelper
    {
        public GraphModel Model { get; set; }
        public List<NodeView> Nodes { get; set; }
        public Dictionary<int, Edge> Edges { get; set; }
    }
}
