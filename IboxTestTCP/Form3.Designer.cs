namespace IboxTestTCP
{
    partial class StatusForm
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.ClearLogButt = new System.Windows.Forms.Button();
            this.SaveLogButt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(1, -2);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(1148, 196);
            this.listBox1.TabIndex = 0;
            // 
            // ClearLogButt
            // 
            this.ClearLogButt.Location = new System.Drawing.Point(12, 197);
            this.ClearLogButt.Name = "ClearLogButt";
            this.ClearLogButt.Size = new System.Drawing.Size(206, 30);
            this.ClearLogButt.TabIndex = 1;
            this.ClearLogButt.Text = "Clear Log";
            this.ClearLogButt.UseVisualStyleBackColor = true;
            this.ClearLogButt.Click += new System.EventHandler(this.ClearLogButt_Click);
            // 
            // SaveLogButt
            // 
            this.SaveLogButt.Location = new System.Drawing.Point(931, 197);
            this.SaveLogButt.Name = "SaveLogButt";
            this.SaveLogButt.Size = new System.Drawing.Size(206, 30);
            this.SaveLogButt.TabIndex = 2;
            this.SaveLogButt.Text = "Save Log";
            this.SaveLogButt.UseVisualStyleBackColor = true;
            // 
            // StatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 230);
            this.ControlBox = false;
            this.Controls.Add(this.SaveLogButt);
            this.Controls.Add(this.ClearLogButt);
            this.Controls.Add(this.listBox1);
            this.Name = "StatusForm";
            this.Text = "Status Log";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button ClearLogButt;
        private System.Windows.Forms.Button SaveLogButt;
    }
}