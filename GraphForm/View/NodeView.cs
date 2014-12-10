using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using GraphForm.Model;

namespace GraphForm.View
{
    [Serializable]
    public partial class NodeView : Button
    {
        public NodeState State { get; set; }
        public bool IsStartNode { get; set; }
        public NodeView(string text)
        {
            Text = text;
            State = NodeState.White;
            IsStartNode = false;
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var buttonPath = new GraphicsPath();
            var newRectangle = ClientRectangle;
            newRectangle.Inflate(-10,-10);
            switch (State)
            {
                    case NodeState.White:
                        e.Graphics.DrawEllipse(Pens.Green, newRectangle);
                        BackColor = Color.Green;
                        ForeColor = Color.Black;
                        break;
                    case NodeState.Gray:
                        e.Graphics.DrawEllipse(new Pen(Color.FromArgb(80, 80, 80)), newRectangle);
                        BackColor = Color.FromArgb(80, 80, 80);
                        ForeColor = Color.Red;
                        break;
                    case NodeState.Black:
                        e.Graphics.DrawEllipse(Pens.Black, newRectangle);
                        BackColor = Color.Black;
                        ForeColor = Color.Red;
                        break;
            }

            if (IsStartNode)
            {
                BackColor = Color.Red;
                ForeColor = Color.Black;
            }
            
            newRectangle.Inflate(1,1);
            buttonPath.AddEllipse(newRectangle);
            Region = new Region(buttonPath);
            base.OnPaint(e);
        }
    }
}
