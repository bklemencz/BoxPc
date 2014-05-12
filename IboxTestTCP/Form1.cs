using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ZedGraph;

namespace IboxTestTCP
{
       
    public partial class Form1 : Form
    {
        public const int MAX_PAGES = 10;
        public int ConfPageCount =10;
        public struct VarRecord_t
        {
            public String Name;
            public Int32 Page;
            public Int32 VarID;
            public Int32 Address;
            public Int16 Length;
            public Int16 Offset;
            public Double Mult;
            public Int16 Ratems;

        };

        public struct PageData_t
        {
            public String Name;
            public int VarCount;
            public VarRecord_t[] Variables;

        }
        
        public VarRecord_t[] IboxCSVread = new VarRecord_t[2500];
        public VarRecord_t[] TempVarRec = new VarRecord_t[50];
        public int ImpVariableCount,SelectedPage;
        int counter;
        
        /// <summary>
        /// InterForm Communication shared variables
        /// </summary>

        public static RollingPointPairList[] Lines = new RollingPointPairList[24];
        public static int MaxTime;
        public static PageData_t[] Pages = new PageData_t[MAX_PAGES];
        public static Timer ShVarUpdTimer = new Timer();
        public static ComboBox ShPgSelCombo = new ComboBox();
      
        public Form1()
        {
             
            
            InitializeComponent();
            LoadDefault();

            for (int i = 0; i < 24; i++ )
            {
                Lines[i] = new RollingPointPairList(2000);
            }

                VarupdateTimer.Interval = Int32.Parse(VarUpdateMs.Text);

            ShVarUpdTimer = VarupdateTimer;
            ShPgSelCombo = VarViewPgSelCombo;
            

        }

        private void ImpIboxCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            String FileLine;
            int LineNbr=0;

            // Set filter options and filter index.
            openFileDialog1.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            // Process input if the user clicked OK.
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Open the selected file to read.
                FileStream inStr = new FileStream(openFileDialog1.FileName, FileMode.Open);
                var file = new StreamReader(inStr, Encoding.ASCII);
                while (!file.EndOfStream)
                {
                    FileLine = file.ReadLine();
                    String[] LineItems = FileLine.Split(',');
                    if ((LineItems[2] == "CAN 1C GPEC") && LineItems[5].StartsWith("00"))
                    {
                        IboxCSVread[LineNbr].Name = LineItems[4];

                        IboxCSVread[LineNbr].Address = Int32.Parse(LineItems[5], System.Globalization.NumberStyles.HexNumber);
                        IboxCSVread[LineNbr].Length = Int16.Parse(LineItems[6]);
                        if (LineItems[7].Contains("E = H*"))
                        {
                            String Substr = LineItems[7].Replace("E = H* ","");
                            IboxCSVread[LineNbr].Mult = Double.Parse(Substr);
                            IboxCSVread[LineNbr].Offset = 0;
                        } else
                            if (LineItems[7].Contains("E = (H") && !LineItems[7].Contains("*"))
                            {
                                String Substr = LineItems[7].Replace("E = (H ", "");
                                Substr = Substr.Replace(")", "");
                                Substr = Substr.Replace(" ", "");
                                IboxCSVread[LineNbr].Offset = Int16.Parse(Substr);
                                IboxCSVread[LineNbr].Mult = 1;
                            } else
                                if (LineItems[7] == "E = H")
                                {
                                    IboxCSVread[LineNbr].Offset = 0;
                                    IboxCSVread[LineNbr].Mult = 1;
                                } else
                                    if (LineItems[7].Contains("E = (H") && LineItems[7].Contains("*"))
                                    {
                                        String Substr = LineItems[7].Substring(LineItems[7].IndexOf("*") + 2);
                                        IboxCSVread[LineNbr].Mult = Double.Parse(Substr);
                                        Substr = LineItems[7].Remove(LineItems[7].IndexOf("*"));
                                        Substr = Substr.Replace("E = (H ", "");
                                        Substr = Substr.Replace(")", "");
                                        Substr = Substr.Replace(" ", "");
                                        IboxCSVread[LineNbr].Offset = Int16.Parse(Substr);
                                    }
                        if (LineItems[10].StartsWith("("))
                        {
                            IboxCSVread[LineNbr].Ratems = Int16.Parse(LineItems[10].Replace(" ms", "").Replace("(1)", "").Replace("(2)", "").Replace("(3)", ""));
                        }
                        IboxCSVread[LineNbr].VarID = LineNbr;
                        LineNbr++;
                    }
                }
                ImpVariableCount = LineNbr;
                IboxCSVImpList.Items.Clear();
                for (int i=0; i<ImpVariableCount;i++)
                {
                    IboxCSVImpList.Items.Add(IboxCSVread[i].Name);

                }
            }
                

        }

        private void IboxCSVImpList_SelectedIndexChanged(object sender, EventArgs e)
        {
            IBoxImpAddText.Text = IboxCSVread[IboxCSVImpList.SelectedIndex].Address.ToString("X");
            IboxImpLnText.Text = IboxCSVread[IboxCSVImpList.SelectedIndex].Length.ToString();
            IboxImpMulText.Text = IboxCSVread[IboxCSVImpList.SelectedIndex].Mult.ToString();
            IboxImpOffText.Text = IboxCSVread[IboxCSVImpList.SelectedIndex].Offset.ToString();
            IboxImpSRText.Text = IboxCSVread[IboxCSVImpList.SelectedIndex].Ratems.ToString();
            IboxImpIDText.Text = IboxCSVread[IboxCSVImpList.SelectedIndex].VarID.ToString();
        }

        private void PageSelectCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedPage = PageSelectCombo.SelectedIndex;
            PageVarList.Items.Clear();
            for (int i = 0; i < Pages[SelectedPage].VarCount; i++)
            {
                PageVarList.Items.Add(Pages[SelectedPage].Variables[i].Name);
            }
            PageVarList.Invalidate();
            PageVarList.Update();
        }

        private void LoadDefault ()
        {
                int ActPage;
                String InPutLine;
                String[] InlineItems;
                
                for (int i = 0; i < MAX_PAGES; i++)
                {
                    Pages[i].Variables = new VarRecord_t[50];
                }
            
                FileStream inStr = new FileStream("DefaultConfig.csv", FileMode.Open);
                var file = new StreamReader(inStr, Encoding.ASCII);
                while(!file.EndOfStream)
                {
                    InPutLine = file.ReadLine();
                    InlineItems = InPutLine.Split(',');
                    if (InlineItems[0] == "CONFDEF")
                    {
                        PageSelectCombo.Items.Clear();
                        if (Int32.Parse(InlineItems[1]) > MAX_PAGES)
                        {
                            ConfPageCount = 10;
                        } else
                        {
                            ConfPageCount = Int32.Parse(InlineItems[1]);
                        }
                    } else
                    if(InlineItems[0] == "PAGEDEF")
                    {
                        ActPage = Int32.Parse(InlineItems[1]);
                        Pages[ActPage].Name = InlineItems[2];
                        PageSelectCombo.Items.Add(InlineItems[2]);
                        Pages[ActPage].VarCount = Int32.Parse(InlineItems[3]);
                        for (int i = 0; i < Pages[ActPage].VarCount;i++ )
                        {
                            InPutLine = file.ReadLine();
                            InlineItems = InPutLine.Split(',');
                            Pages[ActPage].Variables[i].Name = InlineItems[0];
                            Pages[ActPage].Variables[i].Address = Int32.Parse(InlineItems[1]);
                            Pages[ActPage].Variables[i].Length = Int16.Parse(InlineItems[2]);
                            Pages[ActPage].Variables[i].Mult = Double.Parse(InlineItems[3]);
                            Pages[ActPage].Variables[i].Offset = Int16.Parse(InlineItems[4]);
                            Pages[ActPage].Variables[i].Ratems = Int16.Parse(InlineItems[5]);
                        }
                    }


                }
                file.Close();
                inStr.Close();
                for (int i=0; i<ConfPageCount;i++)
                {
                    VarViewPgSelCombo.Items.Add(Pages[i].Name);
                }
                PageSelectCombo.SelectedIndex = 0;
                VarViewPgSelCombo.SelectedIndex = 0;

                for (int i = 0; i < Pages[0].VarCount; i++)
                {
                    String BoxName = "Var" + (i + 1).ToString() + "Gr";
                    var Item = this.Controls.Find(BoxName, true);
                    Item[0].Text = Pages[0].Variables[i].Name;
                    Item[0].Visible = true;
                }
        }

        private void AddVarToPageButt_Click(object sender, EventArgs e)
        {
            for (int i=0; i<IboxCSVImpList.Items.Count; i++)
            {
                if(IboxCSVImpList.GetItemChecked(i))
                {

                   // Pages[SelectedPage].Name = this.RateSelectCombo.GetItemText(this.RateSelectCombo.SelectedItem);
                    Pages[SelectedPage].Variables[Pages[SelectedPage].VarCount].Name = IboxCSVread[i].Name;
                    Pages[SelectedPage].Variables[Pages[SelectedPage].VarCount].Address = IboxCSVread[i].Address;
                    Pages[SelectedPage].Variables[Pages[SelectedPage].VarCount].Length = IboxCSVread[i].Length;
                    Pages[SelectedPage].Variables[Pages[SelectedPage].VarCount].Mult = IboxCSVread[i].Mult;
                    Pages[SelectedPage].Variables[Pages[SelectedPage].VarCount].Offset = IboxCSVread[i].Offset;
                    Pages[SelectedPage].Variables[Pages[SelectedPage].VarCount].Ratems = IboxCSVread[i].Ratems;
                    PageVarList.Items.Add(IboxCSVread[i].Name);
                    Pages[SelectedPage].VarCount++;
                }
            }
            PageVarList.Invalidate();
            PageVarList.Update();
        }

        private void SaveDefConfButt_Click(object sender, EventArgs e)
        {
                String OutPutLine;
                FileStream inStr = new FileStream("DefaultConfig.csv", FileMode.Create);
                var file = new StreamWriter(inStr, Encoding.ASCII);
                OutPutLine = "Name,Address,Length,Multiplier,Offset,Samplerate";
                file.WriteLine(OutPutLine);
                OutPutLine = "CONFDEF," + ConfPageCount.ToString();
                file.WriteLine(OutPutLine);
                for(int i=0; i<ConfPageCount;i++)
                {
                    OutPutLine = "PAGEDEF,";
                    OutPutLine += i + ",";
                    OutPutLine += Pages[i].Name;
                    OutPutLine += "," + Pages[i].VarCount;
                    file.WriteLine(OutPutLine);
                    for(int j=0;j<Pages[i].VarCount;j++)
                    {
                        OutPutLine = Pages[i].Variables[j].Name + ",";
                        OutPutLine += Pages[i].Variables[j].Address.ToString() + ",";
                        OutPutLine += Pages[i].Variables[j].Length.ToString() + ",";
                        OutPutLine += Pages[i].Variables[j].Mult.ToString() + ",";
                        OutPutLine += Pages[i].Variables[j].Offset.ToString() + ",";
                        OutPutLine += Pages[i].Variables[j].Ratems.ToString();
                        file.WriteLine(OutPutLine);
                    }
                }
                file.Flush();
                file.Close();
                inStr.Close();
        }

        private void PageVarList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ActPage = PageSelectCombo.SelectedIndex;
            Int32 RateIndex;
            PageVarAddText.Text = Pages[ActPage].Variables[PageVarList.SelectedIndex].Address.ToString("X");
            PageVarLenText.Text = Pages[ActPage].Variables[PageVarList.SelectedIndex].Length.ToString();
            PageVarMulText.Text = Pages[ActPage].Variables[PageVarList.SelectedIndex].Mult.ToString();
            PageVarOffText.Text = Pages[ActPage].Variables[PageVarList.SelectedIndex].Offset.ToString();
            if ((RateIndex = RateSelectCombo.FindStringExact(Pages[ActPage].Variables[PageVarList.SelectedIndex].Ratems.ToString())) == ListBox.NoMatches)
            {
                RateSelectCombo.SelectedIndex = RateSelectCombo.FindStringExact("50");
            } else
            {
                RateSelectCombo.SelectedIndex = RateIndex;
            }

            
        }

        private void RateSelectCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ActPage = PageSelectCombo.SelectedIndex;
            Pages[ActPage].Variables[PageVarList.SelectedIndex].Ratems = Int16.Parse(this.RateSelectCombo.GetItemText(this.RateSelectCombo.SelectedItem));
        }

        private void VarViewPgSelCombo_SelectedIndexChanged(object sender, EventArgs e)
        {

            for (int i = 0; i < 24; i++)
            {
                String BoxName = "Var" + (i + 1).ToString() + "Gr";
                var Item = this.Controls.Find(BoxName, true);
                Item[0].Visible = false;
            }

            for (int i = 0; i < Pages[VarViewPgSelCombo.SelectedIndex].VarCount; i++)
            {
                String BoxName = "Var" + (i + 1).ToString() + "Gr";
                var Item = this.Controls.Find(BoxName, true);
                Item[0].Text = Pages[VarViewPgSelCombo.SelectedIndex].Variables[i].Name;
                Item[0].Visible = true;
            }
        }

        private void RemVarFromPageButt_Click(object sender, EventArgs e)
        {
            int VarCnt = Pages[PageSelectCombo.SelectedIndex].VarCount;
            int ItemToRem = PageVarList.SelectedIndex;
            
            for (int i=0;i<VarCnt;i++)
            {
                TempVarRec[i].Name = Pages[PageSelectCombo.SelectedIndex].Variables[i].Name;
                TempVarRec[i].Address = Pages[PageSelectCombo.SelectedIndex].Variables[i].Address;
                TempVarRec[i].Length = Pages[PageSelectCombo.SelectedIndex].Variables[i].Length;
                TempVarRec[i].Mult = Pages[PageSelectCombo.SelectedIndex].Variables[i].Mult;
                TempVarRec[i].Offset = Pages[PageSelectCombo.SelectedIndex].Variables[i].Offset;
                TempVarRec[i].Ratems = Pages[PageSelectCombo.SelectedIndex].Variables[i].Ratems;
            }

            for(int i=ItemToRem+1; i<VarCnt;i++)
            {
                Pages[PageSelectCombo.SelectedIndex].Variables[i-1].Name = TempVarRec[i].Name;
                Pages[PageSelectCombo.SelectedIndex].Variables[i-1].Address = TempVarRec[i].Address;
                Pages[PageSelectCombo.SelectedIndex].Variables[i-1].Length = TempVarRec[i].Length;
                Pages[PageSelectCombo.SelectedIndex].Variables[i-1].Mult = TempVarRec[i].Mult;
                Pages[PageSelectCombo.SelectedIndex].Variables[i-1].Offset = TempVarRec[i].Offset;
                Pages[PageSelectCombo.SelectedIndex].Variables[i-1].Ratems = TempVarRec[i].Ratems;
            }
            Pages[PageSelectCombo.SelectedIndex].VarCount--;
            PageVarList.Items.Clear();
            for(int i=0;i<VarCnt-1;i++)
            {
                PageVarList.Items.Add(Pages[PageSelectCombo.SelectedIndex].Variables[i].Name);
            }
            PageVarList.Invalidate();
            PageVarList.Update();
        }

        private void IboxCSVImpList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            AddVarToPageButt.Enabled = true;
        }

        private void VarupdateTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < Pages[VarViewPgSelCombo.SelectedIndex].VarCount; i++)
            {
                String BoxName = "Var" + (i).ToString();
                var Item = this.Controls.Find(BoxName, true);
                Item[0].Text = counter.ToString();
                Lines[i].Add(counter, counter * 2*(i+1));
                if (counter > MaxTime) MaxTime = counter;
            }
            counter++;

        }

        private void ConnectButt_Click(object sender, EventArgs e)
        {
            if (VarupdateTimer.Enabled)
                VarupdateTimer.Enabled = false;
            else
            {
                VarupdateTimer.Interval = Int32.Parse(VarUpdateMs.Text);
                VarupdateTimer.Enabled = true;

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 NewForm = new Form2();
            NewForm.Show();
        }

    }
}
