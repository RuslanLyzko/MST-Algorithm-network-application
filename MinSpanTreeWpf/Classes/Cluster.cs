using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MinSpanTreeWpf.Classes
{
    public class Cluster
    {
        private readonly List<Node> _nodes;

        public string Label { get; set; }

        public Cluster()
        {
            _nodes = new List<Node>();
        }

        public void AddNode(Node node)
        {
            if (!_nodes.Contains(node))
            {
                _nodes.Add(node);
                node.Cluster = this;
            }
        }

        public List<Node> GetNodeList()
        {
            return _nodes;
        }
    }
}
