using System;

namespace GraphForm.Model
{
    [Serializable]
    public enum EdgeState { Default, Selected }

    [Serializable]
    public class Edge
    {
        public Edge(int startNode, int endNode, double weight, int id)
        {
            Weight = weight;
            StartNode = startNode;
            EndNode = endNode;
            EdgeState = EdgeState.Default;
            Id = id;
        }

        public int StartNode { get; private set; }
        public int EndNode { get; private set; }
        public double Weight { get; private set; }
        public EdgeState EdgeState { get; set; }
        public int Id { get; private set; }
    }
}
