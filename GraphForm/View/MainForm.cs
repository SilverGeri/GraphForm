using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml;
using GraphForm.Model;

namespace GraphForm.View
{
    [Serializable]
    public partial class MainForm : Form
    {
        private GraphModel _model;
        private List<NodeView> nodes;
        private Dictionary<int, Edge> edges;
        private bool _isDragging;
        private Point _clickPosition;
        private Dictionary<string, TableForm> tables;

        public MainForm()
        {
            InitializeComponent();
            SpeedBar.Value = 1000;
            newUndirected.Click += NewGraph;
            newDirected.Click += NewGraph;
            SaveMenuItem.Click += SaveToFile;
            LoadMenuItem.Click += LoadFromFile;
            addButton.Click += AddNode;
            linebtn.Click += NewEdge;
            nodes = new List<NodeView>();
            edges = new Dictionary<int, Edge>();
            StartBtn.Click += StartBtnClick;
            NextBtn.Click += NextBtnClick;
            FinishBtn.Click += FinishBtnClick;
            Dictionary<int, string> algorythmDictionary = new Dictionary<int, string>
            {
                {0, "Válassz egy algoritmust"},
                {1, "Szélességi bejárás"},
                {2, "Mélységi bejárás"},
                {3, "Dijkstra algoritmus"},
                {4, "Kruskal algoritmus"},
                {5, "Bellman-Ford algoritmus"},
                {6, "Floyd algoritmus"},
                {7, "Warshall algoritmus"}
            };
            algorythmChooser.DataSource = new BindingSource(algorythmDictionary, null);
            algorythmChooser.DisplayMember = "Value";
            algorythmChooser.ValueMember = "Key";

            tables = new Dictionary<string, TableForm>();
            TopMost = false;
            newUndirected.PerformClick();
        }

        private void NewGraph(object sender, EventArgs e)
        {
            foreach (var table in tables)
            {
                table.Value.Dispose();
            }
            tables.Clear();

            if (sender == newUndirected)
            {
                _model = new GraphModel(false);
            }
            else if (sender == newDirected)
            {
                _model = new GraphModel(true);
            }
            _model.AddNodeEvent += addNodeHandler;
            _model.AddEdgeEvent += addEdgeHandler;
            _model.NotifyEvent += notifyEventHandler;
            _model.TableEvent += TableHandler;
            _model.NodeStateChangeEvent += NodeStateChanged;
            _model.EdgeStateChangeEvent += EdgeStateChanged;
            _model.StartNodeChanged += StartNodeChanged;
            
            foreach (NodeView node in nodes)
            {
                Controls.Remove(node);
            }
            nodes.Clear();
            edges.Clear();
            Refresh();
        }

        private void StartBtnClick(object sender, EventArgs e)
        {
            foreach (var table in tables)
            {
                table.Value.Dispose();
            }
            tables.Clear();
            _model.Start(algorythmChooser.SelectedIndex);
        }

        private void NextBtnClick(object sender, EventArgs e)
        {
            _model.Next();
        }

        private void FinishBtnClick(object sender, EventArgs e)
        {
            _model.Finish(SpeedBar.Value);
        }

        private void TableHandler(object sender, TableEventArgs e)
        {
            TableForm table;
            try
            {
                table = tables[e.Id];
                table.ChangeDataSource(e.Table);
            }
            catch (KeyNotFoundException)
            {
                table = new TableForm(e.Id, e.Caption, e.Table);
                table.Show();
                tables.Add(e.Id, table);
            }
        }

        private void NodeStateChanged(object sender, NodeStateChangeEventArgs e)
        {
            nodes[e.Index].State = e.State;
            Refresh();
        }

        private void EdgeStateChanged(object sender, EdgeStateChangeEventArgs e)
        {
            edges[e.Id].EdgeState = e.State;
            Refresh();
        }

        private void NewEdge(object sender, EventArgs e)
        {
            string error = "";
            int startNode, endNode;
            double weight;
            if (!int.TryParse(StartNodeBox.Text, out startNode))
            {
                error += "Hibás kezdőcsúcs!\n";
            }
            if (!int.TryParse(EndNodeBox.Text, out endNode))
            {
                error += "Hibás végcsúcs!\n";
            }
            if (!double.TryParse(edgeWeightBox.Text, out weight))
            {
                error += "Hibás élsúly!";
            }
            if (error != "")
            {
                var messageBox = new MessageForm("Hiba", error);
                messageBox.ShowDialog(this);
            }
            else
            {
                _model.AddEdge(startNode, endNode, weight);
            }
        }

        private void AddNode(object sender, EventArgs e)
        {
            _model.AddNode();
        }

        private void addNodeHandler(object sender, NodeEventArgs e)
        {
            var node = new NodeView(text: e.Text)
            {
                Height = 50,
                Width = 50,
                Location = new Point(30, 30),
            };
            node.MouseDown += mouseDown;
            node.MouseMove += mouseMove;
            node.MouseUp += mouseUp;

            Controls.Add(node);
            nodes.Add(node);
            Refresh();
        }

        private void addEdgeHandler(object sender, EdgeEventArgs e)
        {
            edges.Add(e.Id, new Edge(e.StartNode, e.EndNode, e.Weight, e.Id));
            Refresh();
        }

        private void StartNodeChanged(object sender, StartNodeChangeEventArgs e)
        {
            if (e.PrevStartNode != null)
            {
                nodes[(int) e.PrevStartNode].IsStartNode = false;
            }

            if (e.NextStartNode != null)
            {
                nodes[(int) e.NextStartNode].IsStartNode = true;
            }
            Refresh();
        }
        private void notifyEventHandler(object sender, MessageEventArgs e)
        {
            var messageBox = new MessageForm(e.Caption, e.Message);
            messageBox.ShowDialog(this);
        }

        public override void Refresh()
        {
            base.Refresh();
            var graph = CreateGraphics();
            graph.SmoothingMode = SmoothingMode.HighQuality;
            var pen = new Pen(Color.Black, 4);
            if (_model.IsDirected)
            {
                pen.StartCap = LineCap.NoAnchor;
                pen.EndCap = LineCap.ArrowAnchor;
                foreach (Edge edge in edges.Values)
                {
                    var c = new Point((nodes[edge.StartNode - 1].Location.X + nodes[edge.EndNode - 1].Location.X) / 2,
                        (nodes[edge.StartNode - 1].Location.Y + nodes[edge.EndNode - 1].Location.Y) / 2);

                    if (nodes[edge.StartNode - 1].Location.X < nodes[edge.EndNode - 1].Location.X)
                        c.X += 50;
                    else
                        c.X -= 75;

                    if (nodes[edge.StartNode - 1].Location.Y < nodes[edge.EndNode - 1].Location.Y)
                        c.Y += 50;
                    else
                        c.Y -= 75;

                    double size = Math.Sqrt(Math.Pow(c.X - nodes[edge.EndNode - 1].Location.X, 2) +
                        Math.Pow(c.Y - nodes[edge.EndNode - 1].Location.Y, 2));
                    double cos = (nodes[edge.EndNode - 1].Location.Y - c.Y) / size;
                    double sin = (nodes[edge.EndNode - 1].Location.X - c.X) / size;
                    double y = c.Y+25 + cos * (size - 15);
                    double x = c.X+25 + sin * (size - 15);
                    graph.DrawString(edge.Weight.ToString(), DefaultFont, new SolidBrush(Color.Red), c );

                    if (edge.EdgeState == EdgeState.Default)
                    {
                        pen.Color = Color.Black;
                    }
                    else if (edge.EdgeState == EdgeState.Selected)
                    {
                        pen.Color = Color.Blue;
                    }
                    graph.DrawBezier(pen, new Point(nodes[edge.StartNode - 1].Location.X + 25, nodes[edge.StartNode - 1].Location.Y + 25), c, c,
                            new Point((int)x, (int)y));
                }
            }
            else
            {
                foreach (Edge edge in edges.Values)
                {
                    var c = new Point((nodes[edge.StartNode - 1].Location.X + nodes[edge.EndNode - 1].Location.X) / 2 ,
                        (nodes[edge.StartNode - 1].Location.Y + nodes[edge.EndNode - 1].Location.Y) / 2);

                    graph.DrawString(edge.Weight.ToString(), DefaultFont, new SolidBrush(Color.Red), c);

                    if (edge.EdgeState == EdgeState.Default)
                    {
                        pen.Color = Color.Black;
                    }
                    else if (edge.EdgeState == EdgeState.Selected)
                    {
                        pen.Color = Color.Blue;
                    }
                    graph.DrawLine(pen, new Point(nodes[edge.StartNode - 1].Location.X + 25, nodes[edge.StartNode - 1].Location.Y + 25),
                        new Point(nodes[edge.EndNode - 1].Location.X + 25, nodes[edge.EndNode - 1].Location.Y + 25));
                }
            }
        }

        private void mouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) return;
            var draggable = sender as Control;
            if (draggable == null) return;
            _isDragging = true;
            draggable.Capture = true;
            _clickPosition = e.Location;
        }

        private void mouseMove(object sender, MouseEventArgs e)
        {
            var draggable = sender as Control;
            if (_isDragging && draggable != null)
            {
                var currentPosition = e.Location;
                draggable.Left += currentPosition.X - _clickPosition.X;
                draggable.Top += currentPosition.Y - _clickPosition.Y;
                Refresh();
            }
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            var draggable = sender as NodeView;
            if (draggable == null) return;

            if (e.Button == MouseButtons.Right)
            {
                _model.SetStartNode(Convert.ToInt32((draggable).Text) - 1);
                return;
            }
            
            _isDragging = false;
            draggable.Capture = false;
            Refresh();
        }

        private void SaveToFile(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "xml files | *.xml";
            dialog.DefaultExt = "txt";
            dialog.ShowDialog();

            XmlWriter writer = XmlWriter.Create(dialog.FileName);
            writer.WriteStartDocument();
            writer.WriteStartElement("graph");
            writer.WriteAttributeString("directed", _model.IsDirected ? "true" : "false");
            writer.WriteStartElement("nodes");
            foreach (var node in nodes)
            {
                writer.WriteStartElement("node");
                writer.WriteAttributeString("x", node.Location.X.ToString());
                writer.WriteAttributeString("y", node.Location.Y.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.WriteStartElement("edges");
            foreach (var edge in edges.Values)
            {
                writer.WriteStartElement("edge");
                writer.WriteAttributeString("start", edge.StartNode.ToString());
                writer.WriteAttributeString("end", edge.EndNode.ToString());
                writer.WriteAttributeString("weight", edge.Weight.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndDocument();
            writer.Close();
        }

        private void LoadFromFile(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "xml files | *.xml";
            dialog.ShowDialog();
            XmlReader reader = XmlReader.Create(dialog.FileName);
            while (reader.Read())
            {
                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "graph"))
                {
                    if (reader.GetAttribute("directed") == "true")
                    {
                        newDirected.PerformClick();
                    }
                    else if (reader.GetAttribute("directed") == "false")
                    {
                        newUndirected.PerformClick();
                    }
                }

                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "node"))
                {
                    _model.AddNode();
                    nodes[nodes.Count - 1].Left = Convert.ToInt32(reader.GetAttribute("x"));
                    nodes[nodes.Count - 1].Top = Convert.ToInt32(reader.GetAttribute("y"));
                }

                if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "edge"))
                {
                    _model.AddEdge(Convert.ToInt32(reader.GetAttribute("start")), Convert.ToInt32(reader.GetAttribute("end")), Convert.ToDouble(reader.GetAttribute("weight")));
                }
            }

        }
    }
}
