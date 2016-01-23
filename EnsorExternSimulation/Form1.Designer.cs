namespace EnsorExternSimulation
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
            this.btnStartServer = new System.Windows.Forms.Button();
            this.tbxServerIp = new System.Windows.Forms.TextBox();
            this.tbxServerPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lstbDigOutputs = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbDigOutputsFilter = new System.Windows.Forms.TextBox();
            this.lstbDigInputs = new System.Windows.Forms.ListBox();
            this.txtbDigInputsFilter = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlNumOutputs = new System.Windows.Forms.Panel();
            this.pnlNumInputs = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(161, 26);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(93, 20);
            this.btnStartServer.TabIndex = 0;
            this.btnStartServer.Text = "Connect Server";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // tbxServerIp
            // 
            this.tbxServerIp.Location = new System.Drawing.Point(12, 26);
            this.tbxServerIp.Name = "tbxServerIp";
            this.tbxServerIp.Size = new System.Drawing.Size(102, 20);
            this.tbxServerIp.TabIndex = 1;
            this.tbxServerIp.Text = "127.0.0.1";
            // 
            // tbxServerPort
            // 
            this.tbxServerPort.Location = new System.Drawing.Point(120, 26);
            this.tbxServerPort.Name = "tbxServerPort";
            this.tbxServerPort.Size = new System.Drawing.Size(35, 20);
            this.tbxServerPort.TabIndex = 2;
            this.tbxServerPort.Text = "5555";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Port:";
            // 
            // lstbDigOutputs
            // 
            this.lstbDigOutputs.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstbDigOutputs.FormattingEnabled = true;
            this.lstbDigOutputs.Items.AddRange(new object[] {
            "Output 1"});
            this.lstbDigOutputs.Location = new System.Drawing.Point(12, 100);
            this.lstbDigOutputs.Name = "lstbDigOutputs";
            this.lstbDigOutputs.Size = new System.Drawing.Size(291, 290);
            this.lstbDigOutputs.TabIndex = 5;
            this.lstbDigOutputs.Tag = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Outputs";
            // 
            // txtbDigOutputsFilter
            // 
            this.txtbDigOutputsFilter.Location = new System.Drawing.Point(12, 74);
            this.txtbDigOutputsFilter.Name = "txtbDigOutputsFilter";
            this.txtbDigOutputsFilter.Size = new System.Drawing.Size(137, 20);
            this.txtbDigOutputsFilter.TabIndex = 7;
            // 
            // lstbDigInputs
            // 
            this.lstbDigInputs.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lstbDigInputs.FormattingEnabled = true;
            this.lstbDigInputs.Items.AddRange(new object[] {
            "Output 1"});
            this.lstbDigInputs.Location = new System.Drawing.Point(325, 100);
            this.lstbDigInputs.Name = "lstbDigInputs";
            this.lstbDigInputs.Size = new System.Drawing.Size(288, 290);
            this.lstbDigInputs.TabIndex = 8;
            this.lstbDigInputs.Tag = "";
            // 
            // txtbDigInputsFilter
            // 
            this.txtbDigInputsFilter.Location = new System.Drawing.Point(325, 74);
            this.txtbDigInputsFilter.Name = "txtbDigInputsFilter";
            this.txtbDigInputsFilter.Size = new System.Drawing.Size(137, 20);
            this.txtbDigInputsFilter.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(322, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Inputs";
            // 
            // pnlNumOutputs
            // 
            this.pnlNumOutputs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.pnlNumOutputs.AutoScroll = true;
            this.pnlNumOutputs.Location = new System.Drawing.Point(15, 409);
            this.pnlNumOutputs.Name = "pnlNumOutputs";
            this.pnlNumOutputs.Size = new System.Drawing.Size(288, 277);
            this.pnlNumOutputs.TabIndex = 11;
            // 
            // pnlNumInputs
            // 
            this.pnlNumInputs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.pnlNumInputs.AutoScroll = true;
            this.pnlNumInputs.Location = new System.Drawing.Point(325, 409);
            this.pnlNumInputs.Name = "pnlNumInputs";
            this.pnlNumInputs.Size = new System.Drawing.Size(288, 277);
            this.pnlNumInputs.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 698);
            this.Controls.Add(this.pnlNumInputs);
            this.Controls.Add(this.pnlNumOutputs);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtbDigInputsFilter);
            this.Controls.Add(this.lstbDigInputs);
            this.Controls.Add(this.txtbDigOutputsFilter);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lstbDigOutputs);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxServerPort);
            this.Controls.Add(this.tbxServerIp);
            this.Controls.Add(this.btnStartServer);
            this.Name = "Form1";
            this.Text = "Ensor Extern Simulation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.TextBox tbxServerIp;
        private System.Windows.Forms.TextBox tbxServerPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstbDigOutputs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtbDigOutputsFilter;
        private System.Windows.Forms.ListBox lstbDigInputs;
        private System.Windows.Forms.TextBox txtbDigInputsFilter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlNumOutputs;
        private System.Windows.Forms.Panel pnlNumInputs;
    }
}

