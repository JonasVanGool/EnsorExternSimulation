using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace EnsorExternSimulation
{
    public partial class EnsorExternSimulation : Form
    {
        SocketClient socketClient;
        EnsorIOController ensorIOController;
        EnsorIOSocketAdaptor ensorIOSockeAdaptor;
        Thread updateGUI;
        int guiUpdateFreq;
        bool allowUpdateGUI;
        Color brushTrue = Color.LightGreen;
        Color brushFalse = Color.LightPink;
        Color brushTrueSelected = Color.Green;
        Color brushFalseSelected = Color.Red;
        Color brushText = Color.Black;

        const int yOffsetGrouboxes = 80;
        const int yOffsetTextboxes = 15;
        
        const int trackBarRescaler = 10;
        private LinkedList<GroupBox> grpbNumOutputs;
        private LinkedList<GroupBox> grpbNumInputs;
        private bool connected = false;

        delegate void UpdateGraphicsCallback();

        public EnsorExternSimulation()
        {
            InitializeComponent();
            this.FormClosing += Form1_Closing;

            txtbDigOutputsFilter.TextChanged += txtbDigOutputs_TextChanged;
            txtbDigInputsFilter.TextChanged += txtbDigInputs_TextChanged;

            pnlDigOutputs.MouseEnter += pnlDigOutputs_MouseEnter;
            pnlDigInputs.MouseEnter += pnlDigInputs_MouseEnter;

            grpbNumOutputs = new LinkedList<GroupBox>();
            grpbNumInputs = new LinkedList<GroupBox>();


            guiUpdateFreq = 10; //Hz
        }

        void pnlDigInputs_MouseEnter(object sender, EventArgs e)
        {
            ((Panel)sender).Focus();
        }

        void pnlDigOutputs_MouseEnter(object sender, EventArgs e)
        {
            ((Panel)sender).Focus();
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ensorIOSockeAdaptor.StopService();
            socketClient.CloseConnection();
            allowUpdateGUI = false;
            if (updateGUI != null)
                updateGUI.Abort();
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if(ensorIOController==null){
                return;
            }

            if (!connected)
            {   
                socketClient = new SocketClient(tbxServerIp.Text, int.Parse(tbxServerPort.Text));
                socketClient.Connect();
                ensorIOSockeAdaptor = new EnsorIOSocketAdaptor(ensorIOController, socketClient);
                allowUpdateGUI = true;
                updateGUI = new Thread(new ThreadStart(this.UpdateGui));
                updateGUI.Start();
                connected = !connected;
                return;
            }
            if (connected)
            {
                ensorIOSockeAdaptor.StopService();
                socketClient.CloseConnection();
                connected = !connected;
                return;
            }

        }


        private void UpdateGui()
        {
            while (allowUpdateGUI)
            {
                Thread.Sleep((int)((double)1 / (double)guiUpdateFreq * 1000));
                // Update gui
                updateGraphics();         
            }
        }

        private void updateGraphics()
        {
            if (this.InvokeRequired)
            {
                UpdateGraphicsCallback n = new UpdateGraphicsCallback(updateGraphics);
                this.Invoke(n, new object[] { });
            }
            else
            {
                if (ensorIOController.GetSocketUpdate())
                {
                    updateDigOutputs();
                    updateDigInputs();
                    updateNumOutputs();
                    updateNumInputs();
                    ensorIOController.ResetSocketUpdate();
                 }
                btnStartServer.BackColor = !socketClient.GetConnected()? Color.Red : Color.Green;
            }
            
        }

        private void updateDigOutputs()
        {
            foreach (Control control in pnlDigOutputs.Controls)
            {
                //find DigOutput
                foreach (DigOutput digOutput in ensorIOController.digOutputs)
                {
                    if (digOutput.Symbol.Equals(control.Name))
                    {
                        Label tempLabel = (Label)control;
                        // Check if selected
                        if(tempLabel.BackColor == brushTrueSelected || tempLabel.BackColor == brushFalseSelected){
                            if (digOutput.CurrentVal && tempLabel.BackColor == brushFalseSelected)
                            {
                                tempLabel.BackColor = brushTrueSelected;
                            }
                            else if (!digOutput.CurrentVal && tempLabel.BackColor == brushTrueSelected)
                            {
                                tempLabel.BackColor = brushFalseSelected;
                            }
                        }else{
                            if (digOutput.CurrentVal && tempLabel.BackColor == brushFalse)
                            {
                                tempLabel.BackColor = brushTrue;
                            }
                            else if (!digOutput.CurrentVal && tempLabel.BackColor == brushTrue)
                            {
                                tempLabel.BackColor = brushFalse;
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void updateDigInputs()
        {
            foreach (Control control in pnlDigInputs.Controls)
            {
                //find DigInput
                foreach (DigInput digInput in ensorIOController.digInputs)
                {
                    if (digInput.Symbol.Equals(control.Name))
                    {
                        Label tempLabel = (Label)control;
                        // Check if selected
                        if (tempLabel.BackColor == brushTrueSelected || tempLabel.BackColor == brushFalseSelected)
                        {
                            if (digInput.CurrentVal && tempLabel.BackColor == brushFalseSelected)
                            {
                                tempLabel.BackColor = brushTrueSelected;
                            }
                            else if (!digInput.CurrentVal && tempLabel.BackColor == brushTrueSelected)
                            {
                                tempLabel.BackColor = brushFalseSelected;
                            }
                        }
                        else
                        {
                            if (digInput.CurrentVal && tempLabel.BackColor == brushFalse)
                            {
                                tempLabel.BackColor = brushTrue;
                            }
                            else if (!digInput.CurrentVal && tempLabel.BackColor == brushTrue)
                            {
                                tempLabel.BackColor = brushFalse;
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void updateNumOutputs()
        {
            foreach (Control control in pnlNumOutputs.Controls)
            {
                //find numOutput
                foreach (NumOutput numOutput in ensorIOController.numOutputs)
                {
                    if (numOutput.Symbol.Equals(control.Name))
                    {
                        TextBox tempTextbox = (TextBox) control.Controls[0];
                        TrackBar tempTrackbar = (TrackBar)control.Controls[1];
                        tempTextbox.Text = numOutput.CurrentVal.ToString("0.###");
                        int tempOutput = (int)numOutput.CurrentVal * trackBarRescaler;
                        if (tempOutput < tempTrackbar.Minimum)
                        {
                            tempTrackbar.Value = tempTrackbar.Minimum;
                        }
                        else if (tempOutput > tempTrackbar.Maximum)
                        {
                            tempTrackbar.Value = tempTrackbar.Maximum;
                        }
                        else
                        {
                            tempTrackbar.Value = tempOutput;
                        }
                        break;
                    }
                }
            }
        }

        private void updateNumInputs()
        {
            foreach (Control control in pnlNumInputs.Controls)
            {
                //find numInput
                foreach (NumInput numInput in ensorIOController.numInputs)
                {
                    if (numInput.Symbol.Equals(control.Name))
                    {
                        TextBox tempTextbox = (TextBox)control.Controls[0];
                        TrackBar tempTrackbar = (TrackBar)control.Controls[1];
                        tempTextbox.Text = numInput.CurrentVal.ToString("0.###");
                        int tempInput = (int)numInput.CurrentVal * trackBarRescaler;
                        if(tempInput<tempTrackbar.Minimum){
                            tempTrackbar.Value = tempTrackbar.Minimum;
                        }
                        else if (tempInput > tempTrackbar.Maximum)
                        {
                            tempTrackbar.Value = tempTrackbar.Maximum;
                        }
                        else
                        {
                            tempTrackbar.Value = tempInput;
                        }
                        break;
                    }
                }
            }
        }

        private void txtbDigOutputs_TextChanged(object sender, EventArgs e)
        {
            int counter = 0;
            pnlDigOutputs.Controls.Clear();
            foreach (DigOutput digOutput in ensorIOController.digOutputs)
            {
                if (txtbDigOutputsFilter.Text == "" || digOutput.Symbol.Contains(txtbDigOutputsFilter.Text)){
                    Label tempLabel = new Label();
                    tempLabel.Name = digOutput.Symbol;
                    tempLabel.Text = digOutput.Symbol;
                    tempLabel.AutoSize = false;
                    tempLabel.Location = new System.Drawing.Point(9, 4 + counter * yOffsetTextboxes);
                    tempLabel.Size = new System.Drawing.Size(pnlDigInputs.Width - 60, 14);
                    tempLabel.DoubleClick += tempLabelOutput_DoubleClick;
                    tempLabel.Click += tempLabelOutput_Click;
                    tempLabel.BackColor = brushFalse;
                    pnlDigOutputs.Controls.Add(tempLabel);
                    counter++;
                }
            }
        }

        private void txtbDigInputs_TextChanged(object sender, EventArgs e)
        {
            int counter = 0;
            pnlDigInputs.Controls.Clear();
            foreach (DigInput digInput in ensorIOController.digInputs)
            {
                if (txtbDigInputsFilter.Text == "" || digInput.Symbol.Contains(txtbDigInputsFilter.Text))
                {
                    Label tempLabel = new Label();
                    tempLabel.Name = digInput.Symbol;
                    tempLabel.Text = digInput.Symbol;
                    tempLabel.AutoSize = false;
                    tempLabel.Location = new System.Drawing.Point(9, 4 + counter * yOffsetTextboxes);
                    tempLabel.Size = new System.Drawing.Size(pnlDigInputs.Width - 60, 14);
                    tempLabel.DoubleClick += tempLabelInput_DoubleClick;
                    tempLabel.Click += tempLabelInput_Click;
                    tempLabel.BackColor = brushFalse;
                    pnlDigInputs.Controls.Add(tempLabel);
                    counter++;
                }
            }
        }


        private void btnBrowse_Click(object sender, EventArgs e)
        {       
            int counter = 0;   
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "E'nsor Config files (*.EnsConfLst)|*.EnsConfLst";
            openFileDialog1.DefaultExt = "EnsConfLst"; 
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtbConfigFile.Text = openFileDialog1.FileName;
                ensorIOController = new EnsorIOController(openFileDialog1.FileName);

                // Create panel digital outputs
                pnlDigOutputs.Controls.Clear();
                counter = 0;
                foreach (DigOutput digOutput in ensorIOController.digOutputs)
                {
                    Label tempLabel = new Label();
                    tempLabel.Name = digOutput.Symbol;
                    tempLabel.Text = digOutput.Symbol;
                    tempLabel.AutoSize = false;
                    tempLabel.Location = new System.Drawing.Point(9, 4 + counter * yOffsetTextboxes);
                    tempLabel.Size = new System.Drawing.Size(pnlDigInputs.Width-60, 14);
                    tempLabel.DoubleClick += tempLabelOutput_DoubleClick;
                    tempLabel.Click += tempLabelOutput_Click;
                    tempLabel.BackColor = brushFalse;
                    pnlDigOutputs.Controls.Add(tempLabel);
                    counter++;
                }

                // Create panel digital inputs
                pnlDigInputs.Controls.Clear();
                counter = 0;
                foreach (DigInput digInput in ensorIOController.digInputs)
                {
                    Label tempLabel = new Label();
                    tempLabel.Name = digInput.Symbol;
                    tempLabel.Text = digInput.Symbol;
                    tempLabel.AutoSize = false;
                    tempLabel.Location = new System.Drawing.Point(9, 4 + counter * yOffsetTextboxes);
                    tempLabel.Size = new System.Drawing.Size(pnlDigInputs.Width - 60, 14);
                    tempLabel.DoubleClick += tempLabelInput_DoubleClick;
                    tempLabel.Click += tempLabelInput_Click;
                    tempLabel.BackColor = brushFalse;
                    pnlDigInputs.Controls.Add(tempLabel);
                    counter++;
                }

                // clear all group boxes num outputs
                foreach (GroupBox groupBox in grpbNumOutputs)
                    groupBox.Controls.Clear();
                counter = 0;
                foreach (NumOutput numOutput in ensorIOController.numOutputs)
                {
                    // create groupbox
                    GroupBox tempGroupbox = new GroupBox();
                    tempGroupbox.Location = new System.Drawing.Point(9, 4 + counter * yOffsetGrouboxes);
                    tempGroupbox.Name = numOutput.Symbol;
                    tempGroupbox.Text = numOutput.Symbol;
                    tempGroupbox.Size = new System.Drawing.Size(258, 69);
                    tempGroupbox.TabIndex = 2;
                    tempGroupbox.TabStop = false;
                    grpbNumOutputs.AddLast(tempGroupbox);


                    // create textbox
                    TextBox tempTextbox = new TextBox();
                    tempTextbox.Location = new System.Drawing.Point(7, 20);
                    tempTextbox.Name = numOutput.Symbol; ;
                    tempTextbox.Size = new System.Drawing.Size(56, 20);
                    tempTextbox.TabIndex = 1;
                    tempTextbox.Text = numOutput.DefVal.ToString("0.###");
                    tempTextbox.KeyPress += tempTextbox_KeyPress;
                    // create slider
                    TrackBar tempTrackbar = new TrackBar();
                    tempTrackbar.Location = new System.Drawing.Point(69, 19);
                    tempTrackbar.Name = numOutput.Symbol;
                    tempTrackbar.Size = new System.Drawing.Size(183, 45);
                    tempTrackbar.TabIndex = 0;
                    tempTrackbar.Minimum = Math.Min((int)(numOutput.MaxVal * trackBarRescaler), (int)(numOutput.MinVal * trackBarRescaler));
                    tempTrackbar.Maximum = Math.Max((int)(numOutput.MaxVal * trackBarRescaler), (int)(numOutput.MinVal * trackBarRescaler));
                    tempTrackbar.SmallChange = (int)(tempTrackbar.Maximum - tempTrackbar.Minimum) / 100;
                    tempTrackbar.LargeChange = (int)(tempTrackbar.Maximum - tempTrackbar.Minimum) / 10;
                    tempTrackbar.TickFrequency = (int)(tempTrackbar.Maximum - tempTrackbar.Minimum) / 10;
                    grpbNumOutputs.Last().Controls.Add(tempTextbox);
                    grpbNumOutputs.Last().Controls.Add(tempTrackbar);

                    counter++;
                }
                foreach (GroupBox groupBox in grpbNumOutputs)
                    pnlNumOutputs.Controls.Add(groupBox);

                // clear all group boxes num outputs
                foreach (GroupBox groupBox in grpbNumInputs)
                    groupBox.Controls.Clear();
                counter = 0;
                foreach (NumInput numInput in ensorIOController.numInputs)
                {
                    // create groupbox
                    GroupBox tempGroupbox = new GroupBox();
                    tempGroupbox.Location = new System.Drawing.Point(9, 4 + counter * yOffsetGrouboxes);
                    tempGroupbox.Name = numInput.Symbol;
                    tempGroupbox.Text = numInput.Symbol;
                    tempGroupbox.Size = new System.Drawing.Size(258, 69);
                    tempGroupbox.TabIndex = 2;
                    tempGroupbox.TabStop = false;
                    grpbNumInputs.AddLast(tempGroupbox);

                    // create textbox
                    TextBox tempTextbox = new TextBox();
                    tempTextbox.Location = new System.Drawing.Point(7, 20);
                    tempTextbox.Name = numInput.Symbol; ;
                    tempTextbox.Size = new System.Drawing.Size(56, 20);
                    tempTextbox.TabIndex = 1;
                    tempTextbox.Text = numInput.CurrentVal.ToString("0.###");
                    tempTextbox.KeyPress += tempTextbox_KeyPress;
                    // create slider
                    TrackBar tempTrackbar = new TrackBar();
                    tempTrackbar.Location = new System.Drawing.Point(69, 19);
                    tempTrackbar.Name = numInput.Symbol;
                    tempTrackbar.Size = new System.Drawing.Size(183, 45);
                    tempTrackbar.TabIndex = 0;
                    tempTrackbar.Minimum = Math.Min((int)(numInput.MaxVal * trackBarRescaler), (int)(numInput.MinVal * trackBarRescaler));
                    tempTrackbar.Maximum = Math.Max((int)(numInput.MaxVal * trackBarRescaler), (int)(numInput.MinVal * trackBarRescaler)); ;
                    tempTrackbar.SmallChange = (int)(tempTrackbar.Maximum - tempTrackbar.Minimum) / 100;
                    tempTrackbar.LargeChange = (int)(tempTrackbar.Maximum - tempTrackbar.Minimum) / 10;
                    tempTrackbar.TickFrequency = (int)(tempTrackbar.Maximum - tempTrackbar.Minimum) / 10;
                    grpbNumInputs.Last().Controls.Add(tempTextbox);
                    grpbNumInputs.Last().Controls.Add(tempTrackbar);

                    counter++;
                }
                foreach (GroupBox groupBox in grpbNumInputs)
                    pnlNumInputs.Controls.Add(groupBox);
            }
        }

        void tempLabelOutput_Click(object sender, EventArgs e)
        {
            foreach (Label label in pnlDigOutputs.Controls)
            {
                if(label.BackColor == brushFalseSelected || label.BackColor == brushTrueSelected)
                    label.BackColor = ((Label)sender).BackColor == brushTrueSelected ? brushTrue : brushFalse;
            }

            ((Label)sender).BackColor = ((Label)sender).BackColor == brushTrue ? brushTrueSelected : brushFalseSelected;
        }

        void tempLabelInput_Click(object sender, EventArgs e)
        {
            foreach (Label label in pnlDigInputs.Controls)
            {
                if (label.BackColor == brushFalseSelected || label.BackColor == brushTrueSelected)
                    label.BackColor = ((Label)sender).BackColor == brushTrueSelected ? brushTrue : brushFalse;
            }

            ((Label)sender).BackColor = ((Label)sender).BackColor == brushTrue ? brushTrueSelected : brushFalseSelected;
        }

        void tempLabelOutput_DoubleClick(object sender, EventArgs e)
        {
            if (((Label)sender).BackColor == brushTrue || ((Label)sender).BackColor == brushTrueSelected)
                ensorIOController.SetDigOutputByGUI(((Label)sender).Text, false);
            else
                ensorIOController.SetDigOutputByGUI(((Label)sender).Text, true);
            ensorIOSockeAdaptor.SendIOUpdate();
        }

        void tempLabelInput_DoubleClick(object sender, EventArgs e)
        {
            if (((Label)sender).BackColor == brushTrue || ((Label)sender).BackColor == brushTrueSelected)
                ensorIOController.SetDigInputByGUI(((Label)sender).Text, false);
            else
                ensorIOController.SetDigInputByGUI(((Label)sender).Text, true);
            ensorIOSockeAdaptor.SendIOUpdate();
        }

        void tempTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

            // only allow '-' when at first position
            if ((e.KeyChar == '-') && ((sender as TextBox).SelectionStart != 0))
            {
                e.Handled = true;
            }
        }
    }
}
