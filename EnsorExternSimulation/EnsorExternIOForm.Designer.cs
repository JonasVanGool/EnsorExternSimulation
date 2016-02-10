namespace EnsorExternSimulation
{
    partial class EnsorExternIO
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnsorExternIO));
            this.btnStartServer = new System.Windows.Forms.Button();
            this.tbxServerIp = new System.Windows.Forms.TextBox();
            this.tbxServerPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbDigOutputsFilter = new System.Windows.Forms.TextBox();
            this.txtbDigInputsFilter = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.pnlNumOutputs = new System.Windows.Forms.Panel();
            this.pnlNumInputs = new System.Windows.Forms.Panel();
            this.txtbConfigFile = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.pnlDigOutputs = new System.Windows.Forms.Panel();
            this.pnlDigInputs = new System.Windows.Forms.Panel();
            this.lblConnectionSpeed = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Control;
            this.label3.Location = new System.Drawing.Point(12, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Outputs";
            // 
            // txtbDigOutputsFilter
            // 
            this.txtbDigOutputsFilter.BackColor = System.Drawing.SystemColors.Window;
            this.txtbDigOutputsFilter.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtbDigOutputsFilter.Enabled = false;
            this.txtbDigOutputsFilter.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtbDigOutputsFilter.Location = new System.Drawing.Point(12, 74);
            this.txtbDigOutputsFilter.Name = "txtbDigOutputsFilter";
            this.txtbDigOutputsFilter.Size = new System.Drawing.Size(137, 20);
            this.txtbDigOutputsFilter.TabIndex = 7;
            // 
            // txtbDigInputsFilter
            // 
            this.txtbDigInputsFilter.Enabled = false;
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
            // txtbConfigFile
            // 
            this.txtbConfigFile.Location = new System.Drawing.Point(325, 26);
            this.txtbConfigFile.Name = "txtbConfigFile";
            this.txtbConfigFile.ReadOnly = true;
            this.txtbConfigFile.Size = new System.Drawing.Size(223, 20);
            this.txtbConfigFile.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(325, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Config file:";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(554, 26);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(58, 23);
            this.btnBrowse.TabIndex = 15;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // pnlDigOutputs
            // 
            this.pnlDigOutputs.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pnlDigOutputs.AutoScroll = true;
            this.pnlDigOutputs.Location = new System.Drawing.Point(15, 100);
            this.pnlDigOutputs.Name = "pnlDigOutputs";
            this.pnlDigOutputs.Size = new System.Drawing.Size(288, 303);
            this.pnlDigOutputs.TabIndex = 12;
            // 
            // pnlDigInputs
            // 
            this.pnlDigInputs.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pnlDigInputs.AutoScroll = true;
            this.pnlDigInputs.Location = new System.Drawing.Point(325, 100);
            this.pnlDigInputs.Name = "pnlDigInputs";
            this.pnlDigInputs.Size = new System.Drawing.Size(288, 303);
            this.pnlDigInputs.TabIndex = 13;
            // 
            // lblConnectionSpeed
            // 
            this.lblConnectionSpeed.AutoSize = true;
            this.lblConnectionSpeed.Location = new System.Drawing.Point(261, 32);
            this.lblConnectionSpeed.Name = "lblConnectionSpeed";
            this.lblConnectionSpeed.Size = new System.Drawing.Size(0, 13);
            this.lblConnectionSpeed.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(264, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Delay:";
            // 
            // EnsorExternIO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(625, 698);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblConnectionSpeed);
            this.Controls.Add(this.pnlDigInputs);
            this.Controls.Add(this.pnlDigOutputs);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtbConfigFile);
            this.Controls.Add(this.pnlNumInputs);
            this.Controls.Add(this.pnlNumOutputs);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtbDigInputsFilter);
            this.Controls.Add(this.txtbDigOutputsFilter);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxServerPort);
            this.Controls.Add(this.tbxServerIp);
            this.Controls.Add(this.btnStartServer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EnsorExternIO";
            this.Text = "Ensor Extern IO";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.TextBox tbxServerIp;
        private System.Windows.Forms.TextBox tbxServerPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtbDigInputsFilter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel pnlNumOutputs;
        private System.Windows.Forms.Panel pnlNumInputs;
        private System.Windows.Forms.TextBox txtbConfigFile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBrowse;
        public System.Windows.Forms.TextBox txtbDigOutputsFilter;
        private System.Windows.Forms.Panel pnlDigOutputs;
        private System.Windows.Forms.Panel pnlDigInputs;
        private System.Windows.Forms.Label lblConnectionSpeed;
        private System.Windows.Forms.Label label6;
    }
}

