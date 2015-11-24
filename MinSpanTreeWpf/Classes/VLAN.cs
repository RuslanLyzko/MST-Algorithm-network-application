using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace MinSpanTreeWpf.Classes
{
    public class VLAN
    {
        public VLAN()
        {
            NodeIds = new List<int>();
        }

        public Color Color { get; set; }

        public SolidColorBrush Brush => new SolidColorBrush(Color);

        public List<int> NodeIds { get; private set; }

        public override string ToString()
        {
            return NodeIds.Count == 0
                ? "<empty>"
                : string.Join(", ", NodeIds);
        }
    }
}
