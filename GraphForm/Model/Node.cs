using System;
using System.Collections.Generic;

namespace GraphForm.Model
{
    [Serializable]
    public enum NodeState { White, Gray, Black }
    [Serializable]
    public class Node
    {
        public NodeState State { get; set; }
        public List<Edge> Edges { get; set; }
        public int Index { get; private set; } 

        public Node(int index)
        {
            State = NodeState.White;
            Edges = new List<Edge>();
            Index = index;
        }
    }
}
