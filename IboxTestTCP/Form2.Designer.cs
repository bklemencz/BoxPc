namespace IboxTestTCP
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
            this.GrVarListCheckBox = new System.Windows.Forms.CheckedListBox();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.StrtBtn = new System.Windows.Forms.Button();
            this.GrUpdTimer1 = new System.Windows.Forms.Timer(this.components);
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.label1 = new System.Windows.Forms.Label();
            this.XAxisUpdown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.XAxisUpdown)).BeginInit();
            this.SuspendLayout();
            // 
            // GrVarListCheckBox
            // 
            this.GrVarListCheckBox.FormattingEnabled = true;
            this.GrVarListCheckBox.Location = new System.Drawing.Point(1022, 5);
            this.GrVarListCheckBox.Name = "GrVarListCheckBox";
            this.GrVarListCheckBox.Size = new System.Drawing.Size(188, 349);
            this.GrVarListCheckBox.TabIndex = 2;
            this.GrVarListCheckBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.GrVarListCheckBox_ItemCheck);
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(1118, 425);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(93, 23);
            this.CloseBtn.TabIndex = 3;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // StrtBtn
            // 
            this.StrtBtn.Location = new System.Drawing.Point(1022, 425);
            this.StrtBtn.Name = "StrtBtn";
            this.StrtBtn.Size = new System.Drawing.Size(89, 23);
            this.StrtBtn.TabIndex = 4;
            this.StrtBtn.Text = "Start";
            this.StrtBtn.UseVisualStyleBackColor = true;
            this.StrtBtn.Click += new System.EventHandler(this.StrtBtn_Click);
            // 
            // GrUpdTimer1
            // 
            this.GrUpdTimer1.Tick += new System.EventHandler(this.GrUpdTimer1_Tick);
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.Location = new System.Drawing.Point(3, 5);
            this.zedGraphControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(1013, 443);
            this.zedGraphControl1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1022, 357);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Time Range:";
            // 
            // XAxisUpdown
            // 
            this.XAxisUpdown.Location = new System.Drawing.Point(1094, 355);
            this.XAxisUpdown.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.XAxisUpdown.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.XAxisUpdown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.XAxisUpdown.Name = "XAxisUpdown";
            this.XAxisUpdown.Size = new System.Drawing.Size(58, 20);
            this.XAxisUpdown.TabIndex = 7;
            this.XAxisUpdown.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.XAxisUpdown.ValueChanged += new System.EventHandler(this.XAxisUpdown_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1156, 357);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "s";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1215, 453);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.XAxisUpdown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.zedGraphControl1);
            this.Controls.Add(this.StrtBtn);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.GrVarListCheckBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form2";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "GraphView";
            ((System.ComponentModel.ISupportInitialize)(this.XAxisUpdown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox GrVarListCheckBox;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.Button StrtBtn;
        public System.Windows.Forms.Timer GrUpdTimer1;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown XAxisUpdown;
        private System.Windows.Forms.Label label2;
    }
}