using System.Data;
using System.Windows.Forms;

namespace GraphForm.View
{
    public partial class TableForm : Form
    {
        public string Id { get; private set; }
        public TableForm(string id, string caption, DataTable data)
        {
            InitializeComponent();
            Id = id;
            Table.Size = Size;
            Table.DataSource = data;
            Text = caption;
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        public void ChangeDataSource(DataTable data)
        {
            Table.DataSource = data;
            Refresh();
        }
    }
}
