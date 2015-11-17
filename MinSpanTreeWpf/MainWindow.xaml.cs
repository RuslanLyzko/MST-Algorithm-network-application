using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading.Tasks;
using MinSpanTreeWpf.Classes;

namespace MinSpanTreeWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double _diameter = 30;
        private const double _edgeLabelSize = 20;

        private const int _fontSize = 12;
        private const int _edgeFontSize = 10;

        private List<Node> _nodes;
        private List<Edge> _edges;
        
        private Node _edgeNode1, _edgeNode2;
        private SolidColorBrush _unvisitedBrush, _visitedBrush;
        private int _count;

        public MainWindow()
        {
            InitializeComponent();

            drawingCanvas.SetValue(Canvas.ZIndexProperty, 0);

            _nodes = new List<Node>();
            _edges = new List<Edge>();

            _count = 1;

            _unvisitedBrush = new SolidColorBrush(Colors.Black);
            _visitedBrush = new SolidColorBrush(Colors.DarkViolet);
        }

        private void drawingCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                Point clickPoint = e.GetPosition(drawingCanvas);

                if (HasClickedOnNode(clickPoint.X, clickPoint.Y))
                {
                    AssignEndNodes(clickPoint.X, clickPoint.Y);
                    if (_edgeNode1 != null && _edgeNode2 != null)
                    {
                        //build an edge
                        double distance = GetEdgeDistance();
                        if (distance != 0.0)
                        {
                            Edge edge = CreateEdge(_edgeNode1, _edgeNode2, distance);
                            _edges.Add(edge);
                            PaintEdge(edge);
                        }
                        ClearEdgeNodes();
                    }
                }
                else
                {
                    if (!OverlapsNode(clickPoint))
                    {
                        Node n = CreateNode(clickPoint);
                        _nodes.Add(n);
                        PaintNode(n);
                        _count++;
                        ClearEdgeNodes();
                    }
                }
            }
        }

        /// <summary>
        /// A method to detect whether the user has clicked on a node
        /// used either for edge creation or for indicating the end-points for which to find
        /// the minimum distance
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        /// <returns>Whether a user is clicked on a existing node</returns>
        private bool HasClickedOnNode(double x, double y)
        {
            bool rez = false;
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i].HasPoint(new Point(x, y)))
                {
                    rez = true;
                    break;
                }
            }
            return rez;
        }

        /// <summary>
        /// Get a node at a specific coordinate
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        /// <returns>The node that has been found or null if there is no node at the speicied coordinates</returns>
        private Node GetNodeAt(double x, double y)
        {
            Node rez = null;
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i].HasPoint(new Point(x, y)))
                {
                    rez = _nodes[i];
                    break;
                }
            }
            return rez;
        }
        /// <summary>
        /// Upon the creation of a new node,
        /// make sure that it is not overlapping an existing node
        /// </summary>
        /// <param name="p">A x,y point</param>
        /// <returns>Whether there is an overlap with an existing node</returns>
        private bool OverlapsNode(Point p)
        {
            bool rez = false;
            double distance;
            for (int i = 0; i < _nodes.Count; i++)
            {
                distance = GetDistance(p, _nodes[i].Center);
                if (distance < _diameter)
                {
                    rez = true;
                    break;
                }
            }
            return rez;
        }

        /// <summary>
        /// Use an additional dialog window to get the distance
        /// for an edge as specified by the user
        /// </summary>
        /// <returns>The distance value specified by the user</returns>
        private double GetEdgeDistance()
        {
            double distance = 0.0;
            DistanceDialog dd = new DistanceDialog();
            dd.Owner = this;

            dd.ShowDialog();
            distance = dd.Distance;

            return distance;
        }
        /// <summary>
        /// Calculate the Eucledean distance between two points
        /// </summary>
        /// <param name="p1">Point 1</param>
        /// <param name="p2">Point 2</param>
        /// <returns>The distance between the two points</returns>
        private double GetDistance(Point p1, Point p2)
        {
            double xSq = Math.Pow(p1.X - p2.X, 2);
            double ySq = Math.Pow(p1.Y - p2.Y, 2);
            double dist = Math.Sqrt(xSq + ySq);

            return dist;
        }

        private void AssignEndNodes(double x, double y)
        {
            Node currentNode = GetNodeAt(x, y);
            if (currentNode != null)
            {
                if (_edgeNode1 == null)
                {
                    _edgeNode1 = currentNode;
                    statusLabel.Content = "You have selected node " + currentNode.Label + ". Please select another node.";
                }
                else
                {
                    if (currentNode != _edgeNode1)
                    {
                        _edgeNode2 = currentNode;
                        statusLabel.Content = "Click on the canvas to create a node.";
                    }
                }
            }
        }

        /// <summary>
        /// Create a new node using the coordinates specified by a point
        /// </summary>
        /// <param name="p">A Point object that carries the coordinates for Node creation</param>
        /// <returns></returns>
        private Node CreateNode(Point p)
        {
            double nodeCenterX = p.X - _diameter / 2;
            double nodeCenterY = p.Y - _diameter / 2;
            Node newNode = new Node(new Point(nodeCenterX, nodeCenterY), p, _count.ToString(), _diameter);

            CreateCluster(newNode);

            return newNode;
        }


        private Cluster CreateCluster(Node node)
        {
            node.Cluster = new Cluster {Label = node.Label};
            node.Cluster.AddNode(node);
            return node.Cluster;
        }

        /// <summary>
        /// Paint a single node on the canvas
        /// </summary>
        /// <param name="node">A node object carrying the coordinates</param>
        private void PaintNode(Node node)
        {
            //paint the node
            Ellipse ellipse = new Ellipse();
            if (node.Visited)
                ellipse.Fill = _visitedBrush;
            else
                ellipse.Fill = _unvisitedBrush;

            ellipse.Width = _diameter;
            ellipse.Height = _diameter;

            ellipse.SetValue(Canvas.LeftProperty, node.Location.X);
            ellipse.SetValue(Canvas.TopProperty, node.Location.Y);
            ellipse.SetValue(Canvas.ZIndexProperty, 2);
            //add to the canvas
            drawingCanvas.Children.Add(ellipse);

            //paint the node label 
            TextBlock tb = new TextBlock();
            tb.Text = node.Label;
            tb.Foreground = Brushes.White;
            tb.FontSize = _fontSize;
            tb.TextAlignment = TextAlignment.Center;
            tb.SetValue(Canvas.LeftProperty, node.Center.X - (_fontSize / 4 * node.Label.Length));
            tb.SetValue(Canvas.TopProperty, node.Center.Y - _fontSize / 2);
            tb.SetValue(Canvas.ZIndexProperty, 3);

            //add to canvas on top of the cirle
            drawingCanvas.Children.Add(tb);
        }

        private Edge CreateEdge(Node node1, Node node2, double distance)
        {
            return new Edge(node1, node2, distance);
        }

        private void PaintEdge(Edge edge)
        {
            //draw the edge
            Line line = new Line();
            line.X1 = edge.FirstNode.Center.X;
            line.X2 = edge.SecondNode.Center.X;

            line.Y1 = edge.FirstNode.Center.Y;
            line.Y2 = edge.SecondNode.Center.Y;

            if (edge.Visited)
                line.Stroke = _visitedBrush;
            else
                line.Stroke = _unvisitedBrush;

            line.StrokeThickness = 1;
            line.SetValue(Canvas.ZIndexProperty, 1);
            drawingCanvas.Children.Add(line);

            //draw the distance label
            Point edgeLabelPoint = GetEdgeLabelCoordinate(edge);
            TextBlock tb = new TextBlock();
            tb.Text = edge.Length.ToString();
            tb.Foreground = Brushes.White;

            if (edge.Visited)
                tb.Background = _visitedBrush;
            else
                tb.Background = _unvisitedBrush;

            tb.Padding = new Thickness(5);
            tb.FontSize = _edgeFontSize;

            tb.MinWidth = _edgeLabelSize;
            tb.MinHeight = _edgeLabelSize;

            tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            tb.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            tb.TextAlignment = TextAlignment.Center;

            tb.SetValue(Canvas.LeftProperty, edgeLabelPoint.X);
            tb.SetValue(Canvas.TopProperty, edgeLabelPoint.Y);
            tb.SetValue(Canvas.ZIndexProperty, 2);
            drawingCanvas.Children.Add(tb);
        }

        /// <summary>
        /// Calculate the coordinates where an edge label is to be drawn
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        private Point GetEdgeLabelCoordinate(Edge edge)
        {
            double x = Math.Abs(edge.FirstNode.Location.X - edge.SecondNode.Location.X) / 2;
            double y = Math.Abs(edge.FirstNode.Location.Y - edge.SecondNode.Location.Y) / 2;

            if (edge.FirstNode.Location.X > edge.SecondNode.Location.X)
                x += edge.SecondNode.Location.X;
            else
                x += edge.FirstNode.Location.X;

            if (edge.FirstNode.Location.Y > edge.SecondNode.Location.Y)
                y += edge.SecondNode.Location.Y;
            else
                y += edge.FirstNode.Location.Y;

            return new Point(x, y);
        }

        private void ClearEdgeNodes()
        {
            _edgeNode1 = _edgeNode2 = null;
        }

        private void findMinSpanTreeBtn_Click(object sender, RoutedEventArgs e)
        {
            statusLabel.Content = "Calculating...";

            var mst = FindMinSpanTree(new Node_Edge_Clusters
            {
                Edges = _edges,
                Nodes = _nodes
            });

            foreach (Edge edge in mst)
            {
                edge.Visited = true;
                PaintEdge(edge);
                PaintNode(edge.FirstNode);
                PaintNode(edge.SecondNode);
            }
        }

        /// <summary>
        /// The method for finding the min span tree
        /// </summary>
        private List<Edge> FindMinSpanTree(Node_Edge_Clusters inputData)
        {
            var result = new List<Edge>();

            // the forest contains all visited nodes
            // List<Node> forest = new List<Node>();

            var clusters = inputData.Nodes
                .Select(p => p.Cluster)
                .Distinct()
                .ToList();

            // sort the edges by their length
            inputData.Edges.Sort();
            
            foreach (Edge currentEdge in inputData.Edges)
            {
                if (clusters.Count == 1)
                    break;

                Cluster cluster1 = currentEdge.FirstNode.Cluster;
                Cluster cluster2 = currentEdge.SecondNode.Cluster;

                if (cluster1.Label != cluster2.Label)
                {
                    result.Add(currentEdge);
                    
                    // add the length to the total cost
                    // totalCost += currentEdge.Length;

                    currentEdge.FirstNode.Visited = true;
                    currentEdge.SecondNode.Visited = true;

                    // merge two cluster and make a single one of them
                    List<Node> nodeList = cluster2.GetNodeList();
                    foreach (Node n in nodeList)
                    {
                        cluster1.AddNode(n);
                        n.Cluster = cluster1;
                    }

                    clusters.Remove(cluster2);
                }
            }

            return result;
            // return totalCost;
        }

        private void PaintAllNodes()
        {
            foreach (Node n in _nodes)
                PaintNode(n);
        }

        private void PaintAllEdges()
        {
            foreach (Edge e in _edges)
                PaintEdge(e);
        }

        private void Clear()
        {
            _nodes.Clear();
            _edges.Clear();
            _count = 1;
        }

        private void Restart()
        {
            foreach (Node node in _nodes)
            {
                node.Visited = false;
                //create a new cluster
                CreateCluster(node);
            }

            foreach (Edge e in _edges)
                e.Visited = false;
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            Clear();
            drawingCanvas.Children.Clear();
        }

        private void restartBtn_Click(object sender, RoutedEventArgs e)
        {
            Restart();
            PaintAllNodes();
            PaintAllEdges();
        }
    }
}
