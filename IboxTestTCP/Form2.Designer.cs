﻿namespace IboxTestTCP
{
    partial class Form2
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.GrVarListCheckBox = new System.Windows.Forms.CheckedListBox();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.StrtBtn = new System.Windows.Forms.Button();
            this.GrUpdTimer1 = new System.Windows.Forms.Timer(this.components);
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1363, 462);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(251, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // GrVarListCheckBox
            // 
            this.GrVarListCheckBox.FormattingEnabled = true;
            this.GrVarListCheckBox.Location = new System.Drawing.Point(1363, 6);
            this.GrVarListCheckBox.Margin = new System.Windows.Forms.Padding(4);
            this.GrVarListCheckBox.Name = "GrVarListCheckBox";
            this.GrVarListCheckBox.Size = new System.Drawing.Size(249, 446);
            this.GrVarListCheckBox.TabIndex = 2;
            this.GrVarListCheckBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.GrVarListCheckBox_ItemCheck);
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(1490, 523);
            this.CloseBtn.Margin = new System.Windows.Forms.Padding(4);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(124, 28);
            this.CloseBtn.TabIndex = 3;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // StrtBtn
            // 
            this.StrtBtn.Location = new System.Drawing.Point(1363, 523);
            this.StrtBtn.Margin = new System.Windows.Forms.Padding(4);
            this.StrtBtn.Name = "StrtBtn";
            this.StrtBtn.Size = new System.Drawing.Size(119, 28);
            this.StrtBtn.TabIndex = 4;
            this.StrtBtn.Text = "Start";
            this.StrtBtn.UseVisualStyleBackColor = true;
            // 
            // GrUpdTimer1
            // 
            this.GrUpdTimer1.Tick += new System.EventHandler(this.GrUpdTimer1_Tick);
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(4, 6);
            this.zedGraphControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(1351, 545);
            this.zedGraphControl1.TabIndex = 5;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1621, 557);
            this.ControlBox = false;
            this.Controls.Add(this.zedGraphControl1);
            this.Controls.Add(this.StrtBtn);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.GrVarListCheckBox);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form2";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "GraphView";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckedListBox GrVarListCheckBox;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.Button StrtBtn;
        public System.Windows.Forms.Timer GrUpdTimer1;
        private ZedGraph.ZedGraphControl zedGraphControl1;
    }
}