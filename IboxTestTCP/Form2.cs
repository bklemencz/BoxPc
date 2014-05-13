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
        public Color[] LineColors = new Color[24];
        public GraphPane myPane = new GraphPane();
        public LineItem[] Curves = new LineItem[24];
        public YAxis[] AxisY = new YAxis[25];
        
        public Form2()
        {
            InitializeComponent();
            myPane = zedGraphControl1.GraphPane;
            InitColors();
            InitControls();
            initGraph2();

            
            //InitGraph();
            
            
        }

        public void InitControls()
        {
            GrVarListCheckBox.CheckOnClick = true;
            for (int i = 0; i < IboxTestTCP.Form1.Pages[IboxTestTCP.Form1.ShPgSelCombo.SelectedIndex].VarCount; i++)
            {
                GrVarListCheckBox.Items.Add(IboxTestTCP.Form1.Pages[IboxTestTCP.Form1.ShPgSelCombo.SelectedIndex].Variables[i].Name);
            }
        }

        public void InitColors()
        {
            LineColors[0] = Color.Red;
            LineColors[1] = Color.Blue;
            LineColors[2] = Color.Green;
            LineColors[3] = Color.Black;
            LineColors[4] = Color.Gray;
            LineColors[5] = Color.Purple;
            for (int i=7; i<24; i++)
            {
                LineColors[i] = Color.Red;
            }
       }

        public void initGraph2()
        {
            RollingPointPairList dummy = new RollingPointPairList(2000);
            LineItem myCurve = myPane.AddCurve("",dummy, Color.Transparent, SymbolType.None);
            myCurve.YAxisIndex = 0;
            myPane.Title.IsVisible = false;
            myPane.XAxis.Title.IsVisible = false;
            myPane.YAxis.Scale.IsVisible = false;
            myPane.YAxis.Title.IsVisible = false;
            
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MajorGrid.Color = Color.Gray;
            
            myPane.YAxis.MinorGrid.IsVisible = true;
            myPane.YAxis.MinorGrid.Color = Color.Gray;

            myPane.XAxis.MajorGrid.IsVisible = true;
            myPane.XAxis.MajorGrid.Color = Color.Gray;
            myPane.XAxis.MinorGrid.IsVisible = true;
            myPane.XAxis.MinorGrid.Color = Color.Gray;

            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0f);
            //myPane.XAxis.MinorGrid
            
        }

        public void InitGraph()
        {
     
            // Set the titles and axis labels
            myPane.Title.Text = "Demonstration of Multi Y Graph";
            myPane.Title.IsVisible = false;
            myPane.XAxis.Title.Text = "Time, s";
            myPane.YAxis.Title.Text = "Velocity, m/s";
            myPane.Y2Axis.Title.Text = "Acceleration, m/s2";

            // Make up some data points based on the Sine function
            PointPairList vList = new PointPairList();
            PointPairList aList = new PointPairList();
            PointPairList dList = new PointPairList();
            PointPairList eList = new PointPairList();

            // Fabricate some data values
            for (int i = 0; i < 30; i++)
            {
                double time = (double)i;
                double acceleration = 2.0;
                double velocity = acceleration * time;
                double distance = acceleration * time * time / 2.0;
                double energy = 100.0 * velocity * velocity / 2.0;
                aList.Add(time, acceleration);
                vList.Add(time, velocity);
                eList.Add(time, energy);
                dList.Add(time, distance);
            }

            // Generate a red curve with diamond symbols, and "Velocity" in the legend
            LineItem myCurve = myPane.AddCurve("Velocity",
               vList, Color.Red, SymbolType.None);
            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);

            // Generate a blue curve with circle symbols, and "Acceleration" in the legend
            myCurve = myPane.AddCurve("Acceleration",
               aList, Color.Blue, SymbolType.None);
            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);
            // Associate this curve with the Y2 axis
            myCurve.IsY2Axis = true;

            // Generate a green curve with square symbols, and "Distance" in the legend
            myCurve = myPane.AddCurve("Distance",
               dList, Color.Green, SymbolType.None);
            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);
            // Associate this curve with the second Y axis
            myCurve.YAxisIndex = 1;

            // Generate a Black curve with triangle symbols, and "Energy" in the legend
            myCurve = myPane.AddCurve("Energy",
               eList, Color.Black, SymbolType.None);
            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);
            // Associate this curve with the Y2 axis
            myCurve.IsY2Axis = true;
            // Associate this curve with the second Y2 axis
            myCurve.YAxisIndex = 1;

            // Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;

            // Make the Y axis scale red
            myPane.YAxis.Scale.FontSpec.FontColor = Color.Red;
            myPane.YAxis.Title.FontSpec.FontColor = Color.Red;
            myPane.YAxis.Title.IsVisible = false;
            // turn off the opposite tics so the Y tics don't show up on the Y2 axis
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;
            // Don't display the Y zero line
            myPane.YAxis.MajorGrid.IsZeroLine = false;
            // Align the Y axis labels so they are flush to the axis
            myPane.YAxis.Scale.Align = AlignP.Inside;
            myPane.YAxis.Scale.Max = 100;

            // Enable the Y2 axis display
            myPane.Y2Axis.IsVisible = true;
            // Make the Y2 axis scale blue
            myPane.Y2Axis.Scale.FontSpec.FontColor = Color.Blue;
            myPane.Y2Axis.Title.FontSpec.FontColor = Color.Blue;
            myPane.Y2Axis.Title.IsVisible = false;
            // turn off the opposite tics so the Y2 tics don't show up on the Y axis
            myPane.Y2Axis.MajorTic.IsOpposite = false;
            myPane.Y2Axis.MinorTic.IsOpposite = false;
            // Display the Y2 axis grid lines
            myPane.Y2Axis.MajorGrid.IsVisible = true;
            // Align the Y2 axis labels so they are flush to the axis
            myPane.Y2Axis.Scale.Align = AlignP.Inside;
            myPane.Y2Axis.Scale.Min = 1.5;
            myPane.Y2Axis.Scale.Max = 3;

            // Create a second Y Axis, green
            YAxis yAxis3 = new YAxis("Distance, m");
            myPane.YAxisList.Add(yAxis3);
            yAxis3.Scale.FontSpec.FontColor = Color.Green;
            yAxis3.Title.FontSpec.FontColor = Color.Green;
            yAxis3.Title.IsVisible = false;
            yAxis3.Color = Color.Green;
            // turn off the opposite tics so the Y2 tics don't show up on the Y axis
            yAxis3.MajorTic.IsInside = false;
            yAxis3.MinorTic.IsInside = false;
            yAxis3.MajorTic.IsOpposite = false;
            yAxis3.MinorTic.IsOpposite = false;
            // Align the Y2 axis labels so they are flush to the axis
            yAxis3.Scale.Align = AlignP.Inside;

            Y2Axis yAxis4 = new Y2Axis("Energy");
            yAxis4.IsVisible = true;
            myPane.Y2AxisList.Add(yAxis4);
            // turn off the opposite tics so the Y2 tics don't show up on the Y axis
            yAxis4.MajorTic.IsInside = false;
            yAxis4.MinorTic.IsInside = false;
            yAxis4.MajorTic.IsOpposite = false;
            yAxis4.MinorTic.IsOpposite = false;
            // Align the Y2 axis labels so they are flush to the axis
            yAxis4.Scale.Align = AlignP.Inside;
            yAxis4.Type = AxisType.Log;
            yAxis4.Title.IsVisible = false;
            yAxis4.Scale.Min = 100;

            // Fill the axis background with a gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGoldenrodYellow, 45.0f);

            zedGraphControl1.AxisChange();

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            IboxTestTCP.Form1.ShVarUpdTimer.Enabled = true;
            button1.Text = Form1.Pages[0].Name;
            GrUpdTimer1.Enabled = true;
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GrVarListCheckBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int ActualItem = GrVarListCheckBox.SelectedIndex;
            if (!GrVarListCheckBox.GetItemChecked(ActualItem))
            {
                
                Curves[ActualItem] = myPane.AddCurve(IboxTestTCP.Form1.Pages[IboxTestTCP.Form1.ShPgSelCombo.SelectedIndex].Variables[ActualItem].Name, 
                IboxTestTCP.Form1.Lines[ActualItem], LineColors[ActualItem], SymbolType.None);
                AxisY[ActualItem] = new YAxis(IboxTestTCP.Form1.Pages[IboxTestTCP.Form1.ShPgSelCombo.SelectedIndex].Variables[ActualItem].Name);
                
                myPane.YAxisList.Add(AxisY[ActualItem]);
                AxisY[ActualItem].Title.IsVisible = false;
                AxisY[ActualItem].Scale.FontSpec.FontColor = LineColors[ActualItem];
               
                AxisY[ActualItem].Title.FontSpec.FontColor = LineColors[ActualItem];
                Curves[ActualItem].YAxisIndex = ActualItem+1;
                Curves[ActualItem].Line.Width = 3;
                Curves[ActualItem].IsVisible = true;
            } else
            {
                myPane.CurveList.Remove(Curves[ActualItem]);
                myPane.YAxisList.Remove(AxisY[ActualItem]);
            }
            
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            zedGraphControl1.Update();
        }

        private void GrUpdTimer1_Tick(object sender, EventArgs e)
        {
            if (IboxTestTCP.Form1.MaxTime < 100)
            {
                myPane.XAxis.Scale.Max = 100;
                myPane.XAxis.Scale.Min = 0;
            } else
            {
                myPane.XAxis.Scale.Min = IboxTestTCP.Form1.MaxTime - 100;
                myPane.XAxis.Scale.Max = IboxTestTCP.Form1.MaxTime;
            }
           
            zedGraphControl1.AxisChange();
            zedGraphControl1.Invalidate();
            zedGraphControl1.Update();
        }
    }
}
