﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using ZedGraph;
using System.Net.Sockets;

namespace IboxTestTCP
{

    public partial class Form1 : Form
    {
        //[DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)]
        //public static extern uint TimeBeginPeriod(uint uMilliseconds);

        //[DllImport("winmm.dll", EntryPoint = "timeEndPeriod", SetLastError = true)]
        //public static extern uint TimeEndPeriod(uint uMilliseconds);

        [DllImport("winmm.dll")]
        internal static extern uint timeBeginPeriod(uint period);

        [DllImport("winmm.dll")]
        internal static extern uint timeEndPeriod(uint period);

        public const int MAX_PAGES = 10;
        public int HISTORY_SIZE = 2500;
        public int TCP_TIMEOUT = 2000;
        public int ConfPageCount = 10;

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
        public int ImpVariableCount, SelectedPage;
        public string LastSerialLine,LastTCPLine;
        public FileStream LogFileStream;
        public StreamWriter LogFileWriter;
        public TcpClient MyClient;
        public NetworkStream MyNetworkStream;
        public StreamReader MyNetworkReader;
        public StreamWriter MyNetworkWriter;
        bool IsTCPConnected;

        public static Stopwatch Stopper1 = new Stopwatch();
        public StatusForm St_Form = new StatusForm();

        /// <summary>
        /// InterForm Communication shared variables
        /// </summary>




        public static RollingPointPairList[] Lines = new RollingPointPairList[24];
        public static double[] LastValues = new double[24];
        public static int MaxTime, ShGrUpdateTime;
        public static PageData_t[] Pages = new PageData_t[MAX_PAGES];
        public static System.Windows.Forms.Timer ShVarUpdTimer = new System.Windows.Forms.Timer();
        public static System.Windows.Forms.Timer ShTCPTimer = new System.Windows.Forms.Timer();
        public static ComboBox ShPgSelCombo = new ComboBox();
        public static int counter;
        public static int BoxState;


        public Form1()
        {

            timeBeginPeriod(1);

            //System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;

            InitializeComponent();

            LoadDefaultPageSetupCSV();
            InitDisplayComponents();
            InitTimers();
            LoadDefaultConfig();

            St_Form.Show();



        }

        private void InitTimers()
        {
            VarupdateTimer.Interval = (Int32)VarUpdRateSel.Value;
            ShVarUpdTimer = VarupdateTimer;
            ShGrUpdateTime = ShGrUpdateTime = (Int32)GrUpdRateSel.Value;
            TCPReadTimer.Interval = 50;
            ShTCPTimer = TCPReadTimer;

        }

        private void InitDisplayComponents()
        {
            ShPgSelCombo = VarViewPgSelCombo;

            for (int i = 0; i < 24; i++)
            {
                Lines[i] = new RollingPointPairList(HISTORY_SIZE);
            }

            foreach (string s in SerialPort.GetPortNames())
            {
                SerPortList.Items.Add(s);
            }
            if (SerPortList.Items.Count != 0)
                SerPortList.SelectedIndex = 0;
            SerPBaudList.SelectedIndex = 1;
            EthCommGroup.Enabled = false;
            SerComGroup.Enabled = true;
            MyClient = new TcpClient();

        }

        private void ImpIboxCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            String FileLine;
            int LineNbr = 0;

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
                            String Substr = LineItems[7].Replace("E = H* ", "");
                            IboxCSVread[LineNbr].Mult = Double.Parse(Substr);
                            IboxCSVread[LineNbr].Offset = 0;
                        }
                        else
                            if (LineItems[7].Contains("E = (H") && !LineItems[7].Contains("*"))
                            {
                                String Substr = LineItems[7].Replace("E = (H ", "");
                                Substr = Substr.Replace(")", "");
                                Substr = Substr.Replace(" ", "");
                                IboxCSVread[LineNbr].Offset = Int16.Parse(Substr);
                                IboxCSVread[LineNbr].Mult = 1;
                            }
                            else
                                if (LineItems[7] == "E = H")
                                {
                                    IboxCSVread[LineNbr].Offset = 0;
                                    IboxCSVread[LineNbr].Mult = 1;
                                }
                                else
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
                for (int i = 0; i < ImpVariableCount; i++)
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

        private void LoadDefaultPageSetupCSV()
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
            while (!file.EndOfStream)
            {
                InPutLine = file.ReadLine();
                InlineItems = InPutLine.Split(',');
                if (InlineItems[0] == "CONFDEF")
                {
                    PageSelectCombo.Items.Clear();
                    if (Int32.Parse(InlineItems[1]) > MAX_PAGES)
                    {
                        ConfPageCount = 10;
                    }
                    else
                    {
                        ConfPageCount = Int32.Parse(InlineItems[1]);
                    }
                }
                else
                    if (InlineItems[0] == "PAGEDEF")
                    {
                        ActPage = Int32.Parse(InlineItems[1]);
                        Pages[ActPage].Name = InlineItems[2];
                        PageSelectCombo.Items.Add(InlineItems[2]);
                        Pages[ActPage].VarCount = Int32.Parse(InlineItems[3]);
                        for (int i = 0; i < Pages[ActPage].VarCount; i++)
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
            for (int i = 0; i < ConfPageCount; i++)
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
            for (int i = 0; i < IboxCSVImpList.Items.Count; i++)
            {
                if (IboxCSVImpList.GetItemChecked(i))
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
                    IboxCSVImpList.SetItemCheckState(i, CheckState.Unchecked);
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
            for (int i = 0; i < ConfPageCount; i++)
            {
                OutPutLine = "PAGEDEF,";
                OutPutLine += i + ",";
                OutPutLine += Pages[i].Name;
                OutPutLine += "," + Pages[i].VarCount;
                file.WriteLine(OutPutLine);
                for (int j = 0; j < Pages[i].VarCount; j++)
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
            }
            else
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
            if (serialPort1.IsOpen)
            {
                SerialHandShake();
                IboxTestTCP.StatusForm.StBox.Items.Add("Page " + Pages[VarViewPgSelCombo.SelectedIndex].Name + " Downloaded to BOX!");
            }
        }

        private void RemVarFromPageButt_Click(object sender, EventArgs e)
        {
            int VarCnt = Pages[PageSelectCombo.SelectedIndex].VarCount;
            int ItemToRem = PageVarList.SelectedIndex;

            for (int i = 0; i < VarCnt; i++)
            {
                TempVarRec[i].Name = Pages[PageSelectCombo.SelectedIndex].Variables[i].Name;
                TempVarRec[i].Address = Pages[PageSelectCombo.SelectedIndex].Variables[i].Address;
                TempVarRec[i].Length = Pages[PageSelectCombo.SelectedIndex].Variables[i].Length;
                TempVarRec[i].Mult = Pages[PageSelectCombo.SelectedIndex].Variables[i].Mult;
                TempVarRec[i].Offset = Pages[PageSelectCombo.SelectedIndex].Variables[i].Offset;
                TempVarRec[i].Ratems = Pages[PageSelectCombo.SelectedIndex].Variables[i].Ratems;
            }

            for (int i = ItemToRem + 1; i < VarCnt; i++)
            {
                Pages[PageSelectCombo.SelectedIndex].Variables[i - 1].Name = TempVarRec[i].Name;
                Pages[PageSelectCombo.SelectedIndex].Variables[i - 1].Address = TempVarRec[i].Address;
                Pages[PageSelectCombo.SelectedIndex].Variables[i - 1].Length = TempVarRec[i].Length;
                Pages[PageSelectCombo.SelectedIndex].Variables[i - 1].Mult = TempVarRec[i].Mult;
                Pages[PageSelectCombo.SelectedIndex].Variables[i - 1].Offset = TempVarRec[i].Offset;
                Pages[PageSelectCombo.SelectedIndex].Variables[i - 1].Ratems = TempVarRec[i].Ratems;
            }
            Pages[PageSelectCombo.SelectedIndex].VarCount--;
            PageVarList.Items.Clear();
            for (int i = 0; i < VarCnt - 1; i++)
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
                Item[0].Text = LastValues[i].ToString();
            }

        }

        private void StreamButt_Click(object sender, EventArgs e)
        {
            if (VarupdateTimer.Enabled)
            {
                SerialReadTimer.Enabled = false;
                if (StopSerialStream())
                {
                    StreamButt.Text = "Start Stream";
                    toolStripStreamingStatus.ForeColor = Color.Red;
                    toolStripStreamingStatus.Text = "Streaming: Off";
                    VarupdateTimer.Enabled = false;
                    SerialReadTimer.Enabled = false;
                    ConnButt.Enabled = true;
                }
            }
            else
            {
                if (StartSerialStream())
                {
                    StreamButt.Text = "Stop Stream";
                    toolStripStreamingStatus.ForeColor = Color.Green;
                    toolStripStreamingStatus.Text = "Streaming: On";
                    VarupdateTimer.Enabled = true;
                    SerialReadTimer.Enabled = true;
                    ConnButt.Enabled = false;
                }
            }
        }

        private void ShowGraph_Click(object sender, EventArgs e)
        {
            Form2 NewForm = new Form2();
            NewForm.Show();
        }

        private void TCPReadTimer_Tick(object sender, EventArgs e)
        {

            
            string[] LastItems;
            int TimeRead = 0;
            if (MyClient.Connected)
            {
                while (MyNetworkStream.DataAvailable)
                {
                    try
                    {
                        LastTCPLine = MyNetworkReader.ReadLine();
                    } 
                    catch
                    {
                        MessageBox.Show("TCP Error!");
                        AddStatusMessage("TCP Read Error. Please restart!");
                    }
                    
                        if (LastTCPLine.Replace("\r","").Replace("\n","").StartsWith("/ERROR") || (LastTCPLine.Length<3))
                        {
                            AddStatusMessage("Error: " + LastTCPLine);
                            LastTCPLine = "";
                        }
                        else
                        {
                            LastItems = LastTCPLine.Replace("\n", "").Replace("\r", "").Split(',');
                            if (LastItems[0] == "") LastItems[0] = MaxTime.ToString();                  //If no timestamp use the last one
                            TimeRead = Int32.Parse(LastItems[0]);
                            MaxTime = TimeRead;

                            for (int i = 0; i < Pages[VarViewPgSelCombo.SelectedIndex].VarCount; i++)
                            {

                                if (LastItems[i + 1] != "")
                                {
                                    Lines[i].Add(TimeRead, Double.Parse(LastItems[i + 1]));
                                    LastValues[i] = Double.Parse(LastItems[i + 1]);
                                }
                                else
                                {
                                    Lines[i].Add(TimeRead, LastValues[i]);
                                }
                            }                                                       // End of variable read
                            if (LogFileEnable.Checked)                              // If logging enabled write values to the file
                            {
                                LogFileAddLastValues();
                            }                                                      // End of file writer
                            LastTCPLine = "";
                        }                                                          // End of line parse
                                                                                 // End of serial buffer read
                }                                                                  // End of serial buffer check 
            }                                                                      // End of serial open condition
            else                                                                   // If serial port is not open, shouldnt happen here
            {
                MessageBox.Show("No Active TCP Connection!");
                AddStatusMessage("No Active TCP Connection! Internal software error, please restart!");
            }
        }

        private void GrUpdRateSel_ValueChanged(object sender, EventArgs e)
        {
            ShGrUpdateTime = (Int32)GrUpdRateSel.Value;
        }

        private void VarUpdRateSel_ValueChanged(object sender, EventArgs e)
        {
            VarupdateTimer.Interval = (Int32)VarUpdRateSel.Value;
        }

        private void SerPortListUpdButton_Click(object sender, EventArgs e)
        {
            SerPortList.Items.Clear();
            foreach (string s in SerialPort.GetPortNames())
            {
                SerPortList.Items.Add(s);
            }
            //SerPortList.SelectedIndex = 0;
        }

        private void TabChange_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveDefConf();
            AddStatusMessage("Configuration Autosave done!");
        }

        private void SaveDefConf()
        {
            String OutPutLine;
            FileStream inStr = new FileStream("DefaultSetup.csv", FileMode.Create);
            var file = new StreamWriter(inStr, Encoding.ASCII);

            OutPutLine = "SerialConn," + SerConnCheck.Checked.ToString();
            file.WriteLine(OutPutLine);
            OutPutLine = "EthConn," + EthConnCheck.Checked.ToString();
            file.WriteLine(OutPutLine);
            OutPutLine = "EthIP," + EthIPTxtBox.Text;
            file.WriteLine(OutPutLine);
            OutPutLine = "EthPort," + EthPortTxtBox.Text;
            file.WriteLine(OutPutLine);
            if (SerPortList.Items.Count != 0)
                OutPutLine = "Serport," + SerPortList.SelectedItem.ToString();
            else
                OutPutLine = "Serport,0";
            file.WriteLine(OutPutLine);
            OutPutLine = "SerBaud," + SerPBaudList.SelectedItem.ToString();
            file.WriteLine(OutPutLine);
            OutPutLine = "VarUpd," + VarUpdRateSel.Value.ToString();
            file.WriteLine(OutPutLine);
            OutPutLine = "GraphUpd," + GrUpdRateSel.Value.ToString();
            file.WriteLine(OutPutLine);
            OutPutLine = "PageSel," + VarViewPgSelCombo.SelectedItem.ToString();
            file.WriteLine(OutPutLine);
            OutPutLine = "LogFileEnable," + LogFileEnable.Checked.ToString();
            file.WriteLine(OutPutLine);
            OutPutLine = "LogFileAutoName," + LogFileAutoName.Checked.ToString();
            file.WriteLine(OutPutLine);
            OutPutLine = "LogFileIncVIN," + LogFileIncVIN.Checked.ToString();
            file.WriteLine(OutPutLine);
            OutPutLine = "LogFileBaseName," + LogFileBaseName.Text;
            file.WriteLine(OutPutLine);
            OutPutLine = "LogFileFolder," + LogFileFolder.Text;
            file.WriteLine(OutPutLine);
            file.Close();
        }

        private void LoadDefaultConfig()
        {
            String LastLine;
            String[] LineItems;
            FileStream inStr = new FileStream("DefaultSetup.csv", FileMode.Open);
            var file = new StreamReader(inStr, Encoding.ASCII);
            while (!file.EndOfStream)
            {
                LastLine = file.ReadLine();
                LineItems = LastLine.Split(',');
                switch (LineItems[0])
                {
                    case "SerialConn":
                        if (LineItems[1] == "True") SerConnCheck.Checked = true;
                        SerComGroup.Enabled = true;
                        break;
                    case "EthConn":
                        if (LineItems[1] == "True") EthConnCheck.Checked = true;
                        EthCommGroup.Enabled = true;
                        break;
                    case "EthIP":
                        EthIPTxtBox.Text = LineItems[1];
                        break;
                    case "EthPort":
                        EthPortTxtBox.Text = LineItems[1];
                        break;
                    case "Serport":
                        int Selindex = SerPortList.FindStringExact(LineItems[1]);
                        if (Selindex >= 0) SerPortList.SelectedIndex = Selindex;
                        break;
                    case "SerBaud":
                        SerPBaudList.SelectedIndex = SerPBaudList.FindStringExact(LineItems[1]);
                        break;
                    case "VarUpd":
                        VarUpdRateSel.Value = Int32.Parse(LineItems[1]);
                        break;
                    case "GraphUpd":
                        GrUpdRateSel.Value = Int32.Parse(LineItems[1]);
                        break;
                    case "PageSel":
                        VarViewPgSelCombo.SelectedIndex = VarViewPgSelCombo.FindStringExact(LineItems[1]);
                        break;
                    case "LogFileEnable":
                        if (LineItems[1] == "True") LogFileEnable.Checked = true;
                        break;
                    case "LogFileAutoName":
                        if (LineItems[1] == "True") LogFileAutoName.Checked = true;
                        break;
                    case "LogFileIncVIN":
                        if (LineItems[1] == "True") LogFileIncVIN.Checked = true;
                        break;
                    case "LogFileBaseName":
                        LogFileBaseName.Text = LineItems[1];
                        break;
                    case "LogFileFolder":
                        LogFileFolder.Text = LineItems[1];
                        break;
                    default:
                        break;
                }

            }
            file.Close();
        }

        private void SerialConnect()
        {
            if (!serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.PortName = SerPortList.SelectedItem.ToString();
                    AddStatusMessage("Serial Port: " + serialPort1.PortName);
                    serialPort1.BaudRate = Int32.Parse(SerPBaudList.SelectedItem.ToString());
                    AddStatusMessage("Serial Baud: " + serialPort1.BaudRate.ToString());
                    serialPort1.Encoding = Encoding.ASCII;
                    serialPort1.NewLine = "\n";
                    serialPort1.Open();
                    PurgeSerial();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Serial Port Open Error:\n" + ex.Message.ToString());

                }
            }
            else
            {
                bool IsOKToClose = false;
                PurgeSerial();
                serialPort1.WriteLine("/DISCONNECT");
                while (!IsOKToClose)
                {
                    try
                    {
                        string GotLine = serialPort1.ReadLine();
                        if (GotLine.Contains("/OK"))
                        {
                            IsOKToClose = true;
                            serialPort1.Close();
                            BoxStatustoolStrip.ForeColor = Color.Red;
                            BoxStatustoolStrip.Text = "BOX Offline";
                            toolStripStatusLabel2.ForeColor = Color.Red;
                            toolStripStatusLabel2.Text = "Conf Empty";
                            toolStripStreamingStatus.ForeColor = Color.Red;
                            toolStripStreamingStatus.Text = "Streaming: Off";
                        }
                    }
                    catch (Exception Ex)
                    {

                        MessageBox.Show(Ex.Message + "\nSerial Error!");
                    }
                }
            }

        }

        private bool SerialHandShake()
        {
            string LastLine;
            string[] LineItems;
            if (serialPort1.IsOpen)
            {

                //////////////////////////////////////// 
                //Read First Line After Version Request
                ////////////////////////////////////////
                #region SerialVerReq
                try
                {
                    serialPort1.DiscardOutBuffer();
                    serialPort1.DiscardInBuffer();
                    string OutPutLine = "/VER";
                    serialPort1.WriteLine(OutPutLine);
                    serialPort1.ReadLine();
                    serialPort1.WriteLine(OutPutLine);
                    //AddStatusMessage("Sent: " + OutPutLine);
                    LastLine = serialPort1.ReadLine();
                    //AddStatusMessage("Last: " + LastLine);
                    if (LastLine.StartsWith("/ERROR"))
                    {
                        AddStatusMessage("Serial Error: " + LastLine);
                        return false;
                    }
                    LastLine = LastLine.TrimStart('/');
                    LineItems = LastLine.Split('/');
                    if (LineItems[2] != "")
                    {
                        if (LineItems[1] == "BKBOX")
                        {
                            AddStatusMessage("BOX ID OK! Downloading Page Config...");
                            BoxStatustoolStrip.ForeColor = Color.Green;
                            BoxStatustoolStrip.Text = "Box Online. " + LineItems[2].Replace("\r", "").Replace("\n", "");
                            BoxState = 2;
                        }
                        else
                        {
                            MessageBox.Show("Box not connected, or Version Not Supported!\nSERAIL PORT CLOSED!");
                            serialPort1.Close();
                            BoxState = 0;
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Box not connected, or Wrong Port Number!\nSERAIL PORT CLOSED!");
                        serialPort1.Close();
                        BoxState = 0;
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString() + "\nBox Not Connected Or Wrong Port Number!\nSERAIL PORT CLOSED!");
                    AddStatusMessage("Box Not Connected Or Wrong Port Number!");
                    serialPort1.Close();
                    BoxState = 0;
                    return false;

                }
            }
                #endregion SerialVerReq

            ////////////////////////////////////////////////////
            // Try to send the variable information of the page
            ////////////////////////////////////////////////////
            #region SerialSendPageInfo
            if (serialPort1.IsOpen)
            {
                try
                {
                    PurgeSerial();
                    LastLine = "/PAGE/" + Pages[VarViewPgSelCombo.SelectedIndex].VarCount.ToString();
                    serialPort1.WriteLine(LastLine);

                    LastLine = serialPort1.ReadLine();

                    if (LastLine.Contains("/OK"))
                    {
                        for (int i = 0; i < Pages[VarViewPgSelCombo.SelectedIndex].VarCount; i++)
                        {
                            LastLine = "/VAR";
                            LastLine += '/' + Pages[VarViewPgSelCombo.SelectedIndex].Variables[i].Name;
                            LastLine += '/' + Pages[VarViewPgSelCombo.SelectedIndex].Variables[i].Address.ToString();
                            LastLine += '/' + Pages[VarViewPgSelCombo.SelectedIndex].Variables[i].Length.ToString();
                            LastLine += '/' + Pages[VarViewPgSelCombo.SelectedIndex].Variables[i].Mult.ToString();
                            LastLine += '/' + Pages[VarViewPgSelCombo.SelectedIndex].Variables[i].Offset.ToString();
                            LastLine += '/' + Pages[VarViewPgSelCombo.SelectedIndex].Variables[i].Ratems.ToString();

                            serialPort1.WriteLine(LastLine);

                            LastLine = serialPort1.ReadLine();  //Wait for ACK

                            if (!LastLine.Contains("/OK"))      //ACK Not OK
                            {
                                SerialErrorParse(LastLine);
                                MessageBox.Show("Communication Protocol Error!\nSERAIL PORT CLOSED!");
                                serialPort1.Close();
                                i = Pages[VarViewPgSelCombo.SelectedIndex].VarCount;
                                BoxState = 0;
                                return false;
                            }
                        }
                        BoxState = 3;
                        toolStripStatusLabel2.ForeColor = Color.Green;
                        toolStripStatusLabel2.Text = "Conf OK";
                        PurgeSerial();
                        AddStatusMessage("Page Configuration Successfully downloaded to the BOX! New State: 3");
                        return false;
                    }
                }
                catch (Exception Ex)
                {

                    MessageBox.Show(Ex.Message + "\nCommunication Protocol Error!\nSERAIL PORT CLOSED!");
                    serialPort1.Close();
                    BoxState = 0;
                    return false;
                }
            #endregion SerialSendPageInfo

            }
            if (!serialPort1.IsOpen)
            {
                MessageBox.Show("Serial Port Not Open, Connect first!");
                return false;
            }
            return true;
        }

        private void SerialErrorParse(string LastLine)
        {
            // To implement error handling code
        }

        private bool StartSerialStream()
        {
            string LastLine;
            if (serialPort1.IsOpen && (BoxState == 3))
            {
                try
                {
                    serialPort1.WriteLine("/STRTSTREAM");
                    LastLine = serialPort1.ReadLine().Replace("\r", "").Replace("\n", "");
                    if (LastLine == "/OK")
                    {
                        PurgeSerial();
                        BoxState = 4;
                        AddStatusMessage("Serial Streaming started! New Box State: " + BoxState.ToString());
                        return true;
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Communication Protocol Error!\nSERAIL PORT CLOSED!");
                    serialPort1.Close();
                    BoxState = 0;
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Serial Port Not Opened");
                return false;
            }
            return false;
        }

        private void AddStatusMessage(string message)
        {
            IboxTestTCP.StatusForm.StBox.Items.Add(DateTime.Now.ToString("yyMMdd h:mm:ss") + "->" + message);
            IboxTestTCP.StatusForm.StBox.SelectedIndex = IboxTestTCP.StatusForm.StBox.Items.Count - 1;
            IboxTestTCP.StatusForm.StBox.SelectedIndex = -1;
        }

        private bool StopSerialStream()
        {
            string LastLine;
            if (serialPort1.IsOpen && (BoxState == 4))
            {
                try
                {
                    PurgeSerial();
                    serialPort1.WriteLine("/DISCONNECT");
                    //AddStatusMessage("Sent: /Disconnect");
                    LastLine = serialPort1.ReadLine().Replace("\r", "").Replace("\n", "");
                    // AddStatusMessage("Rec: " + LastLine);
                    if (LastLine == "/OK")
                    {
                        PurgeSerial();
                        BoxState = 3;
                        AddStatusMessage("Serial Streaming stopped! New Box State: " + BoxState.ToString());

                        return true;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Communication Protocol Error!\nSERAIL PORT CLOSED!");
                    //serialPort1.Close();
                    //BoxState = 0;
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Serial Port Not Opened");
                return false;
            }
            return false;
        }

        private void ConnButt_Click(object sender, EventArgs e)
        {

            SerialConnect();
            if (serialPort1.IsOpen)
            {
                // AddStatusMessage("Connecting on serial bus:" + serialPort1.PortName);
                SerialHandShake();
                ConnButt.Text = "Disconnect";
                if (LogFileEnable.Checked) LogFileOpen();
                StatusReqButt.Enabled = true;
                BoxUpdateButt.Enabled = true;
                StreamButt.Enabled = true;
            }
            if (!serialPort1.IsOpen)
            {
                AddStatusMessage("Disconnecting on serial bus:" + serialPort1.PortName);
                if (LogFileEnable.Checked)
                {
                    LogFileWriter.Flush();
                    LogFileWriter.Close();
                    AddStatusMessage("Log File:  " + LogFileStream.Name + " Closed!");
                    LogFileStream.Close();
                    StreamButt.Enabled = false;
                }
                ConnButt.Text = "Connect Test";
                SerialReadTimer.Enabled = false;
                BoxUpdateButt.Enabled = false;
                StatusReqButt.Enabled = false;
            }
        }

        private void LogFileOpen()
        {
            string LogFileName = LogFileFolder.Text + "\\" + DateTime.Now.ToString("yyMMdd_HHmmss_") + LogFileBaseName.Text + "_" + Pages[VarViewPgSelCombo.SelectedIndex].Name + ".csv";
            LogFileStream = new FileStream(LogFileName, FileMode.Create);
            LogFileWriter = new StreamWriter(LogFileStream, Encoding.ASCII);
            AddStatusMessage("Log file Created: " + LogFileName);
            string OutPutline = "TimeStamp";
            for (int i = 0; i < Pages[VarViewPgSelCombo.SelectedIndex].VarCount; i++)
            {
                OutPutline += "," + Pages[VarViewPgSelCombo.SelectedIndex].Variables[i].Name;
            }
            LogFileWriter.WriteLine(OutPutline);
        }

        private void LogFileAddLastValues()
        {
            string WriteLn = "";
            WriteLn += MaxTime.ToString();
            for (int i = 0; i < Pages[VarViewPgSelCombo.SelectedIndex].VarCount; i++)
            {
                WriteLn += "," + LastValues[i].ToString();//string.Format("{0:0.0000}"));
            }
            LogFileWriter.WriteLine(WriteLn);
        }

        private void SerialReadTimer_Tick(object sender, EventArgs e)
        {
            char LastSerialChar;
            string[] LastItems;
            int TimeRead = 0;
            if (serialPort1.IsOpen)
            {
                while (serialPort1.BytesToRead > 0)
                {
                    LastSerialChar = (char)serialPort1.ReadChar();
                    if (LastSerialChar != '\n')
                    {
                        LastSerialLine += LastSerialChar;
                    }
                    else
                    {
                        if (LastSerialLine.StartsWith("/ERROR"))
                        {
                            AddStatusMessage(LastSerialLine);
                            LastSerialLine = "";
                        }
                        else
                        {
                            LastItems = LastSerialLine.Replace("\n", "").Replace("\r", "").Split(',');
                            if (LastItems[0] == "") LastItems[0] = MaxTime.ToString();                  //If no timestamp use the last one
                            TimeRead = Int32.Parse(LastItems[0]);
                            MaxTime = TimeRead;

                            for (int i = 0; i < Pages[VarViewPgSelCombo.SelectedIndex].VarCount; i++)
                            {

                                if (LastItems[i + 1] != "")
                                {
                                    Lines[i].Add(TimeRead, Double.Parse(LastItems[i + 1]));
                                    LastValues[i] = Double.Parse(LastItems[i + 1]);
                                }
                                else
                                {
                                    Lines[i].Add(TimeRead, LastValues[i]);
                                }
                            }                                                       // End of variable read
                            if (LogFileEnable.Checked)                              // If logging enabled write values to the file
                            {
                                LogFileAddLastValues();
                            }                                                      // End of file writer
                            LastSerialLine = "";
                        }                                                          // End of line parse
                    }                                                              // End of serial buffer read
                }                                                                  // End of serial buffer check 
            }                                                                      // End of serial open condition
            else                                                                   // If serial port is not open, shouldnt happen here
            {
                MessageBox.Show("Serial Port not open!");
                AddStatusMessage("Serial Port Not Open. Internal software error, please restart!");
            }
        }

        private void PageStatusReq_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                SerReqPageStatus();
            }
            else
            {
                MessageBox.Show("Serial Port Not Open, Connect first!");
            }
        }

        private void SerReqPageStatus()
        {
            string[] Lines = new string[30];
            string mess = "";
            PurgeSerial();
            serialPort1.WriteLine("/PAGESTATUS");
            for (int i = 0; i < (Pages[VarViewPgSelCombo.SelectedIndex].VarCount + 1); i++)
            {
                Lines[i] = serialPort1.ReadLine();
                mess += Lines[i] + "\n";
            }
            MessageBox.Show(mess);
            PurgeSerial();
        }

        private void PurgeSerial()
        {
            serialPort1.DiscardInBuffer();
            serialPort1.DiscardOutBuffer();
        }

        private void BoxUpdateButt_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                SerialHandShake();
                IboxTestTCP.StatusForm.StBox.Items.Add("BOX Updated to current page");
                PurgeSerial();
            }
        }

        private void SerConnCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (SerConnCheck.Checked)
                SerComGroup.Enabled = true;
            else
                SerComGroup.Enabled = false;
        }

        private void EthConnCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (EthConnCheck.Checked)
                EthCommGroup.Enabled = true;
            else
                EthCommGroup.Enabled = false;
        }

        private void LogFileSelectFolderButt_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK) LogFileFolder.Text = fbd.SelectedPath;
        }

        private void TCPConnButt_Click(object sender, EventArgs e)
        {

            if (!MyClient.Connected)
            {
                try
                {
                    AddStatusMessage("Connecting to BOX at: " + EthIPTxtBox.Text + " Port: " + EthPortTxtBox.Text);
                    MyClient.Connect(System.Net.IPAddress.Parse(EthIPTxtBox.Text), Int32.Parse(EthPortTxtBox.Text));
                    if (MyClient.Connected)
                    {
                        AddStatusMessage("Connected to TCP Server!");
                        TCPConnButt.Text = "TCP Disconnect";
                        MyNetworkStream = MyClient.GetStream();
                        MyNetworkStream.ReadTimeout = TCP_TIMEOUT;
                        MyNetworkStream.WriteTimeout = TCP_TIMEOUT;
                        MyNetworkReader = new StreamReader(MyNetworkStream);
                        MyNetworkWriter = new StreamWriter(MyNetworkStream);
                        TCPHandShake();
                        if (LogFileEnable.Checked) LogFileOpen();

                    }
                }
                catch
                {
                    AddStatusMessage("TCP Connection Timeout!");
                }
            }
            else
            {
                AddStatusMessage("Closing TCP Connection!");             
                MyNetworkWriter.Flush();
                MyNetworkWriter.Close();
                MyNetworkWriter.Dispose();
                MyNetworkReader.Close();
                MyNetworkReader.Dispose();
                MyNetworkStream.Close();
                MyClient.Close();
                AddStatusMessage("TCP Connection Closed!");
                TCPConnButt.Text = "TCP Connect";
                LogFileWriter.Flush();
                LogFileWriter.Close();
                AddStatusMessage("Log File:  " + LogFileStream.Name + " Closed!");
                LogFileStream.Close();
                BoxStatustoolStrip.ForeColor = Color.Red;
                BoxStatustoolStrip.Text = "BOX Offline";
                toolStripStatusLabel2.ForeColor = Color.Red;
                toolStripStatusLabel2.Text = "Conf Empty";
                toolStripStreamingStatus.ForeColor = Color.Red;
                toolStripStreamingStatus.Text = "Streaming: Off";
                MyClient = new TcpClient();
            }


        }

        private bool TCPHandShake()
        {
            string LastLine;
            string[] LineItems;
            
            if (MyClient.Connected)
            {
                
                //////////////////////////////////////////////////////////////////////
                /// GETTING BOX VERSION INFORMATION
                //////////////////////////////////////////////////////////////////////
 
                try
                {
                    //MyNetworkWriter.WriteLine("");
                   // LastLine = MyNetworkReader.ReadLine();
                    MyNetworkWriter.WriteLine("/VER");
                    MyNetworkWriter.Flush();
                    AddStatusMessage("Getting First TCP Line! ");
                    LastLine = MyNetworkReader.ReadLine();

                    if (LastLine.StartsWith("/ERROR"))
                    {
                        AddStatusMessage("TCP Error: " + LastLine);
                        return false;
                    }

                    LastLine = LastLine.TrimStart('/');
                    LineItems = LastLine.Split('/');
                    if (LineItems[2] != "")
                    {
                        if (LineItems[1] == "BKBOX")
                        {
                            AddStatusMessage("BOX ID OK! Downloading Page Config...");
                            BoxStatustoolStrip.ForeColor = Color.Green;
                            BoxStatustoolStrip.Text = "Box Online. " + LineItems[2].Replace("\r", "").Replace("\n", "");
                            BoxState = 2;
                        }
                        else
                        {
                            MessageBox.Show("Box not connected, or Version Not Supported!");
                            BoxState = 0;
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Box not connected, or Wrong IP!");
                        BoxState = 0;
                        return false;
                    }
                } catch
                {
                    AddStatusMessage("TCP Communication Timeout Error! Please disconnect and reconnect!");
                    return false;
                }
                ///////////////////////////////////////////////////////////////////////////////
                //////// DOWNLOADING PAGE DATA
                ///////////////////////////////////////////////////////////////////////////////
                try
                {
                    LastLine = "/PAGE/" + Pages[VarViewPgSelCombo.SelectedIndex].VarCount.ToString();
                    MyNetworkWriter.WriteLine(LastLine);
                    MyNetworkWriter.Flush();
                    LastLine = MyNetworkReader.ReadLine();
                    if (LastLine.StartsWith("/OK"))
                    {
                        for (int i = 0; i < Pages[VarViewPgSelCombo.SelectedIndex].VarCount; i++)
                        {
                            
                            LastLine = "/VAR";
                            LastLine += '/' + Pages[VarViewPgSelCombo.SelectedIndex].Variables[i].Name;
                            LastLine += '/' + Pages[VarViewPgSelCombo.SelectedIndex].Variables[i].Address.ToString();
                            LastLine += '/' + Pages[VarViewPgSelCombo.SelectedIndex].Variables[i].Length.ToString();
                            LastLine += '/' + Pages[VarViewPgSelCombo.SelectedIndex].Variables[i].Mult.ToString();
                            LastLine += '/' + Pages[VarViewPgSelCombo.SelectedIndex].Variables[i].Offset.ToString();
                            LastLine += '/' + Pages[VarViewPgSelCombo.SelectedIndex].Variables[i].Ratems.ToString();
                            MyNetworkWriter.WriteLine(LastLine);
                            MyNetworkWriter.Flush();

                            LastLine = MyNetworkReader.ReadLine();  //Wait for ACK

                            if (!LastLine.Contains("/OK"))      //ACK Not OK
                            {
                                MessageBox.Show("Communication Protocol Error! Please Restart!");
                                i = Pages[VarViewPgSelCombo.SelectedIndex].VarCount;
                                BoxState = 0;
                                return false;
                            }
                        }
                        BoxState = 3;
                        toolStripStatusLabel2.ForeColor = Color.Green;
                        toolStripStatusLabel2.Text = "Conf OK";
                        AddStatusMessage("Page Configuration Successfully downloaded to the BOX! New State: 3");
                    }
                }
                catch
                {
                    AddStatusMessage("ERROR while downloading variable information! Please disconnect and reconnect!");
                    return false;
                }
            
            }                           // End of client connected check
            else
            {
                AddStatusMessage("Not Connected to the BOX! Please Reconnect!");
                return false;
            }
            return true;
        }   // End of tcphandshake

        private void TCPStreamButt_Click(object sender, EventArgs e)
        {
            if (VarupdateTimer.Enabled)
            {
                TCPReadTimer.Enabled = false;
                if (StopTCPStream())
                {
                    TCPStreamButt.Text = "TCP Strt Stream";
                    toolStripStreamingStatus.ForeColor = Color.Red;
                    toolStripStreamingStatus.Text = "Streaming: Off";
                    VarupdateTimer.Enabled = false;
                    TCPReadTimer.Enabled = false;
                    TCPConnButt.Enabled = true;
                }
            }
            else
            {
                if (StartTCPStream())
                {
                    TCPStreamButt.Text = "TCP Stop Stream";
                    toolStripStreamingStatus.ForeColor = Color.Green;
                    toolStripStreamingStatus.Text = "Streaming: On";
                    VarupdateTimer.Enabled = true;
                    TCPReadTimer.Enabled = true;
                    TCPConnButt.Enabled = false;
                }
            }
        }

        private void PurgeTCP()
        {           
            MyNetworkWriter.Flush();
            MyNetworkReader.DiscardBufferedData();
        }

        private bool StartTCPStream()
        {
            string LastLine;
            if (MyClient.Connected && (BoxState == 3))
            {
                try
                {
                    MyNetworkWriter.WriteLine("/STRTSTREAM");
                    MyNetworkWriter.Flush();
                    LastLine = MyNetworkReader.ReadLine().Replace("\r", "").Replace("\n", "");
                    if (LastLine == "/OK")
                    {
                        //PurgeSerial();
                        BoxState = 4;
                        AddStatusMessage("TCP Streaming started! New Box State: " + BoxState.ToString());
                        return true;
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Communication Protocol Error!\nPlease disconnect and reconnect!");
                    BoxState = 0;
                    return false;
                }
            }
            else
            {
                MessageBox.Show("No active TCP Connection, please connect first!");
                return false;
            }
            return false;
        }

        private bool StopTCPStream()
        {
            string LastLine;
            if (MyClient.Connected && (BoxState == 4))
            {
                try
                {
                    PurgeTCP();
                    MyNetworkWriter.WriteLine("/DISCONNECT");
                    MyNetworkWriter.Flush();
                    LastLine = MyNetworkReader.ReadLine().Replace("\r", "").Replace("\n", "");
                    
                    if (LastLine == "/OK")
                    {
                        PurgeTCP();
                        BoxState = 3;
                        AddStatusMessage("TCP Streaming stopped! New Box State: " + BoxState.ToString());

                        return true;
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Communication Protocol Error!\nPlease Disconnect and reconnect!");
                    
                    return false;
                }
            }
            else
            {
                MessageBox.Show("No TCP Connection");
                return false;
            }
            return false;
        }                                   
    }
}
