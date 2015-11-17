using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MinSpanTreeWpf.Classes
{
    public class Node
    {
        public Node()
        {
            Visited = false;
        }

        public Point Location { get; set; }

        public Point Center { get; set; }

        public double Diameter { get; set; }

        public string Label { get; set; }

        public Cluster Cluster { get; set; }

        public double TotalCost { get; set; }

        public Edge EdgeVisitor { get; set; }

        public bool Visited { get; set; }

        /// <summary>
        /// Calculates whether the node contains a specific point.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool HasPoint(Point p)
        {
            double xSq = Math.Pow(p.X - Center.X,2);
            double ySq = Math.Pow(p.Y - Center.Y,2);
            double dist = Math.Sqrt(xSq + ySq);

            return (dist <= (Diameter/2));
        }
    }
}
