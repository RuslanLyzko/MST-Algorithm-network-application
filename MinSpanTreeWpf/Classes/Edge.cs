using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinSpanTreeWpf.Classes
{
    public class Edge : IComparable<Edge>
    {
        public Edge(Node firstNode, Node secondNode)
        {
            FirstNode = firstNode;
            SecondNode = secondNode;

            Visited = false;
        }

        public Edge(Node firstNode, Node secondNode, double length)
            : this(firstNode, secondNode)
        {
            Length = length;
        }

        public Node FirstNode { get; set; }

        public Node SecondNode { get; set; }

        public double Length { get; set; }

        public int CompareTo(Edge otherEdge)
        {
            return Length.CompareTo(otherEdge.Length);
        }

        public bool Visited { get; set; }
    }
}
