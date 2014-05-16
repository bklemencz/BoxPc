using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IboxTestTCP
{
    public partial class StatusForm : Form
    {
        public static ListBox StBox = new ListBox();
        public StatusForm()
        {
            InitializeComponent();
            StBox = listBox1;
        }

        private void ClearLogButt_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}
