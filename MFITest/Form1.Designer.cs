namespace MFITest
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
            this.components = new System.ComponentModel.Container();
            this.btnStartBackTest = new System.Windows.Forms.Button();
            this.btnCheckLoopPerformance = new System.Windows.Forms.Button();
            this.txtErr = new System.Windows.Forms.TextBox();
            this.timerTestPerformance = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // btnStartBackTest
            // 
            this.btnStartBackTest.Location = new System.Drawing.Point(12, 12);
            this.btnStartBackTest.Name = "btnStartBackTest";
            this.btnStartBackTest.Size = new System.Drawing.Size(102, 23);
            this.btnStartBackTest.TabIndex = 1;
            this.btnStartBackTest.Text = "Start Back Test";
            this.btnStartBackTest.UseVisualStyleBackColor = true;
            this.btnStartBackTest.Click += new System.EventHandler(this.btnStartBackTest_Click);
            // 
            // btnCheckLoopPerformance
            // 
            this.btnCheckLoopPerformance.Location = new System.Drawing.Point(155, 12);
            this.btnCheckLoopPerformance.Name = "btnCheckLoopPerformance";
            this.btnCheckLoopPerformance.Size = new System.Drawing.Size(215, 23);
            this.btnCheckLoopPerformance.TabIndex = 2;
            this.btnCheckLoopPerformance.Text = "Check Loop Performance";
            this.btnCheckLoopPerformance.UseVisualStyleBackColor = true;
            this.btnCheckLoopPerformance.Click += new System.EventHandler(this.btnCheckLoopPerformance_Click);
            // 
            // txtErr
            // 
            this.txtErr.Location = new System.Drawing.Point(-3, 41);
            this.txtErr.Multiline = true;
            this.txtErr.Name = "txtErr";
            this.txtErr.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtErr.Size = new System.Drawing.Size(1278, 549);
            this.txtErr.TabIndex = 36;
            this.txtErr.TextChanged += new System.EventHandler(this.txtErr_TextChanged);
            // 
            // timerTestPerformance
            // 
            this.timerTestPerformance.Interval = 5000;
            this.timerTestPerformance.Tick += new System.EventHandler(this.timerTestPerformance_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1303, 627);
            this.Controls.Add(this.txtErr);
            this.Controls.Add(this.btnCheckLoopPerformance);
            this.Controls.Add(this.btnStartBackTest);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.form_load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartBackTest;
        private System.Windows.Forms.Button btnCheckLoopPerformance;
        private System.Windows.Forms.TextBox txtErr;
        private System.Windows.Forms.Timer timerTestPerformance;
    }
}

