namespace WindowsFormsApplication1
{
    partial class Main
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
            this.button1 = new System.Windows.Forms.Button();
            this.url = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.res = new System.Windows.Forms.RichTextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.iex = new System.Windows.Forms.CheckBox();
            this.gc = new System.Windows.Forms.CheckBox();
            this.fir = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(468, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Scan";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // url
            // 
            this.url.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.url.Location = new System.Drawing.Point(61, 18);
            this.url.Name = "url";
            this.url.Size = new System.Drawing.Size(401, 21);
            this.url.TabIndex = 1;
            this.url.Text = "http://www.thesaurus.com/";
            this.url.TextChanged += new System.EventHandler(this.url_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(549, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "URL:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.res);
            this.groupBox1.Location = new System.Drawing.Point(26, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(598, 262);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scan Result";
            // 
            // res
            // 
            this.res.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.res.Location = new System.Drawing.Point(9, 16);
            this.res.Name = "res";
            this.res.Size = new System.Drawing.Size(580, 246);
            this.res.TabIndex = 0;
            this.res.Text = "";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(372, 327);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(126, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Generate Report";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(504, 327);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(120, 23);
            this.button4.TabIndex = 7;
            this.button4.Text = "Save Result";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // iex
            // 
            this.iex.AutoSize = true;
            this.iex.Location = new System.Drawing.Point(390, 45);
            this.iex.Name = "iex";
            this.iex.Size = new System.Drawing.Size(103, 17);
            this.iex.TabIndex = 8;
            this.iex.Text = "Internet Explorer";
            this.iex.UseVisualStyleBackColor = true;
            // 
            // gc
            // 
            this.gc.AutoSize = true;
            this.gc.Location = new System.Drawing.Point(499, 45);
            this.gc.Name = "gc";
            this.gc.Size = new System.Drawing.Size(62, 17);
            this.gc.TabIndex = 9;
            this.gc.Text = "Chrome";
            this.gc.UseVisualStyleBackColor = true;
            // 
            // fir
            // 
            this.fir.AutoSize = true;
            this.fir.Checked = true;
            this.fir.CheckState = System.Windows.Forms.CheckState.Checked;
            this.fir.Location = new System.Drawing.Point(567, 45);
            this.fir.Name = "fir";
            this.fir.Size = new System.Drawing.Size(57, 17);
            this.fir.TabIndex = 10;
            this.fir.Text = "Firefox";
            this.fir.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(298, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Select Browsers:";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 358);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.fir);
            this.Controls.Add(this.gc);
            this.Controls.Add(this.iex);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.url);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Name = "Main";
            this.Text = "Xenotix Browser Based Fuzzer";
            this.Load += new System.EventHandler(this.Main_Load);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox url;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox iex;
        private System.Windows.Forms.CheckBox gc;
        private System.Windows.Forms.CheckBox fir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox res;
    }
}

