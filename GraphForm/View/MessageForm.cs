using System.Drawing;
using System.Windows.Forms;

namespace GraphForm.View
{
    public partial class MessageForm : Form
    {
        public MessageForm(string caption, string message)
        {
            InitializeComponent();
            Text = caption;
            Message.Text = message;
           /* Message.TextAlign = ContentAlignment.MiddleCenter;
            Message.Dock = DockStyle.Fill;*/
            OKButton.Focus();
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private void OKButton_Click(object sender, System.EventArgs e)
        {
            Dispose();
        }
    }
}
