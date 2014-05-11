﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using System.IO;

namespace IboxTestTCP
{
    

       
    
    
    public partial class Form1 : Form
    {
        public const int MAX_PAGES = 10;
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
        public PlotModel myModel = new PlotModel();

        public PageData_t[] Pages = new PageData_t[MAX_PAGES];
        public VarRecord_t[] IboxCSVread = new VarRecord_t[2500];
        public int ImpVariableCount,SelectedPage;
      
        public Form1()
        {
             
             
            InitializeComponent();

            for (int i = 0; i < MAX_PAGES; i++ )
            {
                Pages[i].Variables = new VarRecord_t[50];
            }

            LoadDefault();
            PageSelectCombo.SelectedIndex = 0;

                myModel.Background = OxyColors.White;
            myModel.PlotAreaBackground = OxyColors.White;
            
            var Axis1 = new LinearAxis();
            var Axis2 = new LinearAxis();
            var Xaxis = new LinearAxis();

            var Series1 = new FunctionSeries(Math.Sin, 0, 10, 0.1, "Sin(x)");
            var Series2 = new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)");
            

            Axis1.MajorGridlineColor = OxyColors.Gray;
            Axis1.MajorGridlineStyle = LineStyle.Dash;
            Axis1.MinorGridlineStyle = LineStyle.Dot;
            Axis1.AxislineColor = OxyColors.Red;
            Axis1.TextColor = OxyColors.Red;
            Axis1.Position = AxisPosition.Right;
            Axis1.Key = "MPH";
            
            
            
            
            Axis2.AxisDistance = 50;
            Axis2.AxislineColor = OxyColors.Blue;
            Axis2.TextColor = OxyColors.Blue;
            Axis2.Position = AxisPosition.Right; 
            Axis2.Key = "RPM";

            Xaxis.MajorGridlineStyle = LineStyle.Dash;
            Xaxis.MinorGridlineStyle = LineStyle.Dot;
            Xaxis.Position = AxisPosition.Bottom;

            
            
            myModel.LegendTitle = null;
            myModel.LegendOrientation = LegendOrientation.Horizontal;
            myModel.LegendPlacement = LegendPlacement.Outside;
            myModel.LegendPosition = LegendPosition.TopRight;
            myModel.LegendBackground = OxyColor.FromAColor(200, OxyColors.White);
            myModel.LegendBorder = OxyColors.Black;
            
            Series1.YAxisKey = "RPM";
            Series1.Color = OxyColors.Red;
            Series2.YAxisKey = "MPH";
            Series2.Color = OxyColors.Blue;
            
            myModel.Axes.Add(Axis1);
            myModel.Axes.Add(Axis2);
            myModel.Axes.Add(Xaxis);
            myModel.Series.Add(Series1);
            myModel.Series.Add(Series2);
            this.plot1.Model = myModel;

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
            CountLabel.Text = Pages[SelectedPage].VarCount.ToString();
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
                FileStream inStr = new FileStream("DefaultConfig.csv", FileMode.Open);
                var file = new StreamReader(inStr, Encoding.ASCII);
                while(!file.EndOfStream)
                {
                    InPutLine = file.ReadLine();
                    InlineItems = InPutLine.Split(',');
                    if(InlineItems[0] == "PAGEDEF")
                    {
                        ActPage = Int32.Parse(InlineItems[1]);
                        Pages[ActPage].Name = InlineItems[2];
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
        
        }

        private void AddVarToPageButt_Click(object sender, EventArgs e)
        {
            for (int i=0; i<IboxCSVImpList.Items.Count; i++)
            {
                if(IboxCSVImpList.GetItemChecked(i))
                {
                    
                    Pages[SelectedPage].Name = PageSelectCombo.SelectedItem.ToString();
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
                for(int i=0; i<MAX_PAGES;i++)
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
        }
    }
}