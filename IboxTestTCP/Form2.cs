using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using ZedGraph;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IboxTestTCP
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            for (int i=0; i<IboxTestTCP.Form1.Pages[IboxTestTCP.Form1.ShPgSelCombo.SelectedIndex].VarCount;i++)
            {
                GrVarListCheckBox.Items.Add(IboxTestTCP.Form1.Pages[IboxTestTCP.Form1.ShPgSelCombo.SelectedIndex].Variables[i].Name);
            }
            GraphPane mypane = zedGraphControl1.GraphPane;
            //mypane.Title = IboxTestTCP.Form1.Pages[IboxTestTCP.Form1.ShPgSelCombo.SelectedIndex].Name;
            mypane.Title = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IboxTestTCP.Form1.ShVarUpdTimer.Enabled = true;
            button1.Text = Form1.Pages[0].Name;
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
