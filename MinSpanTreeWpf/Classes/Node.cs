using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MinSpanTreeWpf.Classes
{
    public class Node
    {
        private Point _center;
        private readonly double _diameter;

        public Node(Point location, string label, double diameter)
        {
            _diameter = diameter;
            Location = location;
            Label = label;
            Cluster = new Cluster {Label = label};
            Visited = false;
        }

        public Node(Point location, Point center, string label, double diameter)
            :this(location,label,diameter)
        {
            _center = center;
        }

        public Node(Point location, Point center, string label, double diameter, Cluster cluster)
            :this(location,center,label,diameter)
        {
            Cluster = cluster;
        }

        public Point Location { get; }

        public Point Center
        {
            get { return _center; }
            set { _center = value; }
        }

        public double Diameter => _diameter;

        public string Label { get; }

        public Cluster Cluster { get; set; }

        public double TotalCost { get; set; }

        public Edge EdgeVisitor { get; set; }

        /// <summary>
        /// Calculates whether the node contains a specific point.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool HasPoint(Point p)
        {
            double xSq = Math.Pow(p.X - _center.X,2);
            double ySq = Math.Pow(p.Y - _center.Y,2);
            double dist = Math.Sqrt(xSq + ySq);

            return (dist <= (_diameter/2));
        }

        public bool Visited { get; set; }
    }
}
