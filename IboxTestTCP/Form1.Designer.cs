namespace IboxTestTCP
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.plot1 = new OxyPlot.WindowsForms.Plot();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.CountLabel = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.UpdAllButt = new System.Windows.Forms.Button();
            this.SaveToFile = new System.Windows.Forms.Button();
            this.SaveDefConfButt = new System.Windows.Forms.Button();
            this.RemVarFromPageButt = new System.Windows.Forms.Button();
            this.AddVarToPageButt = new System.Windows.Forms.Button();
            this.RateSelectCombo = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.PageVarList = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.PageSelectCombo = new System.Windows.Forms.ComboBox();
            this.IboxImpIDText = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.IboxImpSRText = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.IboxImpOffText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.IboxImpMulText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.IboxImpLnText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.IBoxImpAddText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.IboxCSVImpList = new System.Windows.Forms.CheckedListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(-1, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1362, 526);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tabControl2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1354, 497);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Variable View";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Location = new System.Drawing.Point(3, 6);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(1348, 485);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1340, 456);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1340, 456);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "tabPage5";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.plot1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1354, 497);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Graph View";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // plot1
            // 
            this.plot1.Location = new System.Drawing.Point(6, 6);
            this.plot1.Name = "plot1";
            this.plot1.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plot1.Size = new System.Drawing.Size(1342, 485);
            this.plot1.TabIndex = 0;
            this.plot1.Text = "plot1";
            this.plot1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plot1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plot1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.CountLabel);
            this.tabPage3.Controls.Add(this.button2);
            this.tabPage3.Controls.Add(this.UpdAllButt);
            this.tabPage3.Controls.Add(this.SaveToFile);
            this.tabPage3.Controls.Add(this.SaveDefConfButt);
            this.tabPage3.Controls.Add(this.RemVarFromPageButt);
            this.tabPage3.Controls.Add(this.AddVarToPageButt);
            this.tabPage3.Controls.Add(this.RateSelectCombo);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.PageVarList);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.PageSelectCombo);
            this.tabPage3.Controls.Add(this.IboxImpIDText);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.IboxImpSRText);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.IboxImpOffText);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.IboxImpMulText);
            this.tabPage3.Controls.Add(this.label3);
            this.tabPage3.Controls.Add(this.IboxImpLnText);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.IBoxImpAddText);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.IboxCSVImpList);
            this.tabPage3.Controls.Add(this.button1);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1354, 497);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Variable Settings";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // CountLabel
            // 
            this.CountLabel.AutoSize = true;
            this.CountLabel.Location = new System.Drawing.Point(703, 51);
            this.CountLabel.Name = "CountLabel";
            this.CountLabel.Size = new System.Drawing.Size(25, 17);
            this.CountLabel.TabIndex = 27;
            this.CountLabel.Text = "ID:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1249, 178);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 31);
            this.button2.TabIndex = 26;
            this.button2.Text = "Reset Orig";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // UpdAllButt
            // 
            this.UpdAllButt.Location = new System.Drawing.Point(1249, 142);
            this.UpdAllButt.Name = "UpdAllButt";
            this.UpdAllButt.Size = new System.Drawing.Size(100, 31);
            this.UpdAllButt.TabIndex = 25;
            this.UpdAllButt.Text = "Update ";
            this.UpdAllButt.UseVisualStyleBackColor = true;
            // 
            // SaveToFile
            // 
            this.SaveToFile.Location = new System.Drawing.Point(1249, 105);
            this.SaveToFile.Name = "SaveToFile";
            this.SaveToFile.Size = new System.Drawing.Size(100, 31);
            this.SaveToFile.TabIndex = 24;
            this.SaveToFile.Text = "Save To File";
            this.SaveToFile.UseVisualStyleBackColor = true;
            // 
            // SaveDefConfButt
            // 
            this.SaveDefConfButt.Location = new System.Drawing.Point(951, 50);
            this.SaveDefConfButt.Name = "SaveDefConfButt";
            this.SaveDefConfButt.Size = new System.Drawing.Size(100, 31);
            this.SaveDefConfButt.TabIndex = 23;
            this.SaveDefConfButt.Text = "Save Default";
            this.SaveDefConfButt.UseVisualStyleBackColor = true;
            this.SaveDefConfButt.Click += new System.EventHandler(this.SaveDefConfButt_Click);
            // 
            // RemVarFromPageButt
            // 
            this.RemVarFromPageButt.Location = new System.Drawing.Point(297, 371);
            this.RemVarFromPageButt.Name = "RemVarFromPageButt";
            this.RemVarFromPageButt.Size = new System.Drawing.Size(100, 31);
            this.RemVarFromPageButt.TabIndex = 20;
            this.RemVarFromPageButt.Text = "Remove Sel";
            this.RemVarFromPageButt.UseVisualStyleBackColor = true;
            // 
            // AddVarToPageButt
            // 
            this.AddVarToPageButt.Location = new System.Drawing.Point(297, 334);
            this.AddVarToPageButt.Name = "AddVarToPageButt";
            this.AddVarToPageButt.Size = new System.Drawing.Size(100, 31);
            this.AddVarToPageButt.TabIndex = 19;
            this.AddVarToPageButt.Text = "Add To Page";
            this.AddVarToPageButt.UseVisualStyleBackColor = true;
            this.AddVarToPageButt.Click += new System.EventHandler(this.AddVarToPageButt_Click);
            // 
            // RateSelectCombo
            // 
            this.RateSelectCombo.FormattingEnabled = true;
            this.RateSelectCombo.Items.AddRange(new object[] {
            "5",
            "10",
            "20",
            "50",
            "100",
            "200",
            "500",
            "1000"});
            this.RateSelectCombo.Location = new System.Drawing.Point(518, 467);
            this.RateSelectCombo.Name = "RateSelectCombo";
            this.RateSelectCombo.Size = new System.Drawing.Size(169, 24);
            this.RateSelectCombo.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(400, 470);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(121, 17);
            this.label8.TabIndex = 17;
            this.label8.Text = "Sample Rate(ms):";
            // 
            // PageVarList
            // 
            this.PageVarList.FormattingEnabled = true;
            this.PageVarList.ItemHeight = 16;
            this.PageVarList.Location = new System.Drawing.Point(403, 51);
            this.PageVarList.Name = "PageVarList";
            this.PageVarList.Size = new System.Drawing.Size(284, 404);
            this.PageVarList.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(400, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 17);
            this.label7.TabIndex = 15;
            this.label7.Text = "Select Page:";
            // 
            // PageSelectCombo
            // 
            this.PageSelectCombo.FormattingEnabled = true;
            this.PageSelectCombo.Items.AddRange(new object[] {
            "Start",
            "Drive",
            "Idle",
            "Thermal",
            "Fueling",
            "Purge",
            "Spark",
            "Knock"});
            this.PageSelectCombo.Location = new System.Drawing.Point(494, 21);
            this.PageSelectCombo.Name = "PageSelectCombo";
            this.PageSelectCombo.Size = new System.Drawing.Size(193, 24);
            this.PageSelectCombo.TabIndex = 14;
            this.PageSelectCombo.SelectedIndexChanged += new System.EventHandler(this.PageSelectCombo_SelectedIndexChanged);
            // 
            // IboxImpIDText
            // 
            this.IboxImpIDText.Enabled = false;
            this.IboxImpIDText.Location = new System.Drawing.Point(297, 77);
            this.IboxImpIDText.Name = "IboxImpIDText";
            this.IboxImpIDText.Size = new System.Drawing.Size(100, 22);
            this.IboxImpIDText.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(294, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "ID:";
            // 
            // IboxImpSRText
            // 
            this.IboxImpSRText.Enabled = false;
            this.IboxImpSRText.Location = new System.Drawing.Point(297, 306);
            this.IboxImpSRText.Name = "IboxImpSRText";
            this.IboxImpSRText.Size = new System.Drawing.Size(100, 22);
            this.IboxImpSRText.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(294, 286);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(121, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Sample Rate(ms):";
            // 
            // IboxImpOffText
            // 
            this.IboxImpOffText.Enabled = false;
            this.IboxImpOffText.Location = new System.Drawing.Point(297, 257);
            this.IboxImpOffText.Name = "IboxImpOffText";
            this.IboxImpOffText.Size = new System.Drawing.Size(100, 22);
            this.IboxImpOffText.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(294, 237);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Offset:";
            // 
            // IboxImpMulText
            // 
            this.IboxImpMulText.Enabled = false;
            this.IboxImpMulText.Location = new System.Drawing.Point(297, 212);
            this.IboxImpMulText.Name = "IboxImpMulText";
            this.IboxImpMulText.Size = new System.Drawing.Size(100, 22);
            this.IboxImpMulText.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(294, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Multiplier:";
            // 
            // IboxImpLnText
            // 
            this.IboxImpLnText.Enabled = false;
            this.IboxImpLnText.Location = new System.Drawing.Point(297, 167);
            this.IboxImpLnText.Name = "IboxImpLnText";
            this.IboxImpLnText.Size = new System.Drawing.Size(100, 22);
            this.IboxImpLnText.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(294, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Length:";
            // 
            // IBoxImpAddText
            // 
            this.IBoxImpAddText.Enabled = false;
            this.IBoxImpAddText.Location = new System.Drawing.Point(297, 122);
            this.IBoxImpAddText.Name = "IBoxImpAddText";
            this.IBoxImpAddText.Size = new System.Drawing.Size(100, 22);
            this.IBoxImpAddText.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(294, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Address:";
            // 
            // IboxCSVImpList
            // 
            this.IboxCSVImpList.FormattingEnabled = true;
            this.IboxCSVImpList.Location = new System.Drawing.Point(9, 58);
            this.IboxCSVImpList.Name = "IboxCSVImpList";
            this.IboxCSVImpList.Size = new System.Drawing.Size(282, 429);
            this.IboxCSVImpList.TabIndex = 1;
            this.IboxCSVImpList.SelectedIndexChanged += new System.EventHandler(this.IboxCSVImpList_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(282, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "Import Ibox CSV";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.ImpIboxCSV_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.setupToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1364, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(59, 24);
            this.setupToolStripMenuItem.Text = "Setup";
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem1.Text = "Help";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 556);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1364, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1364, 578);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private OxyPlot.WindowsForms.Plot plot1;
        private System.Windows.Forms.CheckedListBox IboxCSVImpList;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox IboxImpMulText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox IboxImpLnText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox IBoxImpAddText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox IboxImpOffText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox IboxImpSRText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox IboxImpIDText;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox PageSelectCombo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListBox PageVarList;
        private System.Windows.Forms.Button SaveToFile;
        private System.Windows.Forms.Button SaveDefConfButt;
        private System.Windows.Forms.Button RemVarFromPageButt;
        private System.Windows.Forms.Button AddVarToPageButt;
        private System.Windows.Forms.ComboBox RateSelectCombo;
        private System.Windows.Forms.Button UpdAllButt;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label CountLabel;
    }
}

