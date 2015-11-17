using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinSpanTreeWpf.Classes
{
    public class Edge : IComparable<Edge>
    {
        public Edge()
        {
            Visited = false;
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
