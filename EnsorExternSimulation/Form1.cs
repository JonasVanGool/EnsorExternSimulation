﻿using System;
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
    public partial class Form1 : Form
    {
        SocketClient socketServer;
        EnsorIOController ensorIOController;
        EnsorIOSocketAdaptor ensorIOSockeAdaptor;
        Thread updateGUI;
        int guiUpdateFreq;
        bool allowUpdateGUI;
        Brush brushTrue = Brushes.LightGreen;
        Brush brushFalse = Brushes.LightPink;
        Brush brushTrueSelected = Brushes.Green;
        Brush brushFalseSelected = Brushes.Red;
        Brush brushText = Brushes.Black;

        const int yOffsetGrouboxes = 80;
        const int trackBarRescaler = 100;
        private LinkedList<GroupBox> grpbNumOutputs;
        private LinkedList<GroupBox> grpbNumInputs;

        delegate void UpdateGraphicsCallback();

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_Closing;

            lstbDigOutputs.DrawItem += lstbOutputs_DrawItem;
            lstbDigOutputs.MouseDoubleClick += lstbOutputs_MouseDoubleClick;
            txtbDigOutputsFilter.TextChanged += txtbDigOutputs_TextChanged;
            lstbDigOutputs.FormattingEnabled = false;

            lstbDigInputs.DrawItem += lstbInputs_DrawItem;
            lstbDigInputs.MouseDoubleClick += lstbInputs_MouseDoubleClick;
            txtbDigInputsFilter.TextChanged += txtbDigInputs_TextChanged;
            lstbDigInputs.FormattingEnabled = false;

            grpbNumOutputs = new LinkedList<GroupBox>();
            grpbNumInputs = new LinkedList<GroupBox>();

            socketServer = new SocketClient(tbxServerIp.Text,int.Parse(tbxServerPort.Text));
            guiUpdateFreq = 5; //Hz
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (socketServer.GetServerActive())
            {
                ensorIOSockeAdaptor.StopService();
                socketServer.StopServer();

            }
            allowUpdateGUI = false;
            if (updateGUI != null)
            updateGUI.Abort();
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if (!socketServer.GetServerActive()){
                ensorIOController = new EnsorIOController(@"C:\Team Systems\Ensor - Projects\Standard_FLV-JonasTestProject\EnsConf\Bld\Ensor_Standard_FLV.EnsConfLst");
                
                lstbDigOutputs.Items.Clear();
                foreach(DigOutput digOutput in ensorIOController.digOutputs)
                    lstbDigOutputs.Items.Add(digOutput.Symbol);

                lstbDigInputs.Items.Clear();
                foreach (DigInput digInput in ensorIOController.digInputs)
                    lstbDigInputs.Items.Add(digInput.Symbol);

                // clear all group boxes num outputs
                foreach (GroupBox groupBox in grpbNumOutputs)
                    groupBox.Controls.Clear();
                int counter = 0;
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
                    tempTextbox.TextChanged += tempTextboxOutputs_TextChanged;
                    // create slider
                    TrackBar tempTrackbar = new TrackBar();
                    tempTrackbar.Location = new System.Drawing.Point(69, 19);
                    tempTrackbar.Name = numOutput.Symbol;
                    tempTrackbar.Size = new System.Drawing.Size(183, 45);
                    tempTrackbar.TabIndex = 0;
                    tempTrackbar.Minimum = Math.Min((int)(numOutput.MaxVal * trackBarRescaler), (int)(numOutput.MinVal * trackBarRescaler));
                    tempTrackbar.Maximum = Math.Max( (int)(numOutput.MaxVal * trackBarRescaler),(int)(numOutput.MinVal * trackBarRescaler));
                    tempTrackbar.SmallChange = (int)(tempTrackbar.Maximum - tempTrackbar.Minimum)  / 100;
                    tempTrackbar.LargeChange = (int)(tempTrackbar.Maximum - tempTrackbar.Minimum)/ 10;
                    tempTrackbar.TickFrequency = (int)(tempTrackbar.Maximum - tempTrackbar.Minimum) / 10;
                    tempTrackbar.MouseUp += tempTrackbarOutputs_MouseUp;
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
                    tempTextbox.TextChanged += tempTextboxInputs_TextChanged;
                    // create slider
                    TrackBar tempTrackbar = new TrackBar();
                    tempTrackbar.Location = new System.Drawing.Point(69, 19);
                    tempTrackbar.Name = numInput.Symbol;
                    tempTrackbar.Size = new System.Drawing.Size(183, 45);
                    tempTrackbar.TabIndex = 0;
                    tempTrackbar.Minimum = Math.Min((int)(numInput.MaxVal * trackBarRescaler), (int)(numInput.MinVal * trackBarRescaler));
                    tempTrackbar.Maximum = Math.Max((int)(numInput.MaxVal * trackBarRescaler), (int)(numInput.MinVal * trackBarRescaler)); ;
                    tempTrackbar.SmallChange = (int)(tempTrackbar.Maximum - tempTrackbar.Minimum)/ 100;
                    tempTrackbar.LargeChange = (int)(tempTrackbar.Maximum - tempTrackbar.Minimum) / 10;
                    tempTrackbar.TickFrequency = (int)(tempTrackbar.Maximum - tempTrackbar.Minimum) / 10;
                    tempTrackbar.MouseUp += tempTrackbarInputs_MouseUp;
                    grpbNumInputs.Last().Controls.Add(tempTextbox);
                    grpbNumInputs.Last().Controls.Add(tempTrackbar);

                    counter++;
                }
                foreach (GroupBox groupBox in grpbNumInputs)
                    pnlNumInputs.Controls.Add(groupBox);

                socketServer.StartServer();
                ensorIOSockeAdaptor = new EnsorIOSocketAdaptor(ensorIOController, socketServer);
                allowUpdateGUI = true;
                updateGUI = new Thread(new ThreadStart(this.UpdateGui));

                updateGUI.Start();
            }
        }

        void tempTrackbarOutputs_MouseUp(object sender, MouseEventArgs e)
        {                     
            TrackBar parent = (TrackBar)sender;
            ensorIOController.SetNumOutputByGUI(parent.Name, parent.Value / trackBarRescaler);
            ensorIOSockeAdaptor.SendIOUpdate();
        }

        void tempTextboxOutputs_TextChanged(object sender, EventArgs e)
        {
            TextBox parent = (TextBox)sender;
            ensorIOController.SetNumOutputByGUI(parent.Name, double.Parse(parent.Text));     
            ensorIOSockeAdaptor.SendIOUpdate();
        }

        void tempTrackbarInputs_MouseUp(object sender, MouseEventArgs e)
        {
            TrackBar parent = (TrackBar)sender;
            ensorIOController.SetNumInputByGUI(parent.Name, parent.Value / trackBarRescaler);
            ensorIOSockeAdaptor.SendIOUpdate();
        }

        void tempTextboxInputs_TextChanged(object sender, EventArgs e)
        {
            TextBox parent = (TextBox)sender;
            ensorIOController.SetNumInputByGUI(parent.Name, double.Parse(parent.Text));
            ensorIOSockeAdaptor.SendIOUpdate();
        }


        private void UpdateGui()
        {
            while (allowUpdateGUI)
            {
                Thread.Sleep((int)((double)1 / (double)guiUpdateFreq * 1000));
                // Update gui
                if (ensorIOController.GetSocketUpdate())
                {
                    updateGraphics();
                    ensorIOController.ResetSocketUpdate();
                }
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
                lstbDigOutputs.Refresh();
                lstbDigInputs.Refresh();
                updateNumOutputs();
                updateNumInputs();
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
                        tempTrackbar.Value = (int)numOutput.CurrentVal * trackBarRescaler;
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
                        tempTrackbar.Value = (int)numInput.CurrentVal * trackBarRescaler;
                        break;
                    }
                }
            }
        }
        private void lstbOutputs_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (ensorIOController == null)
                return;

            e.DrawBackground();
            bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            int index = e.Index;
            if (index >= 0 && index < lstbDigOutputs.Items.Count)
            {
                DigOutput tempDigOutput = null;
                foreach (DigOutput digOutput in ensorIOController.digOutputs)
                {
                    if (digOutput.Symbol.Equals(lstbDigOutputs.Items[index]))
                    {
                        tempDigOutput = digOutput;
                        break;
                    }
                }
                string text = tempDigOutput.Symbol;
                Graphics g = e.Graphics;

                //background:
                Brush backgroundBrush;
                if (selected){
                    backgroundBrush = tempDigOutput.CurrentVal ? brushTrueSelected : brushFalseSelected;
                }else{
                    backgroundBrush = tempDigOutput.CurrentVal ? brushTrue : brushFalse;
                }
                g.FillRectangle(backgroundBrush, e.Bounds);

                //text:
                Brush foregroundBrush = (selected) ? Brushes.Red : Brushes.Bisque;
                g.DrawString(text, e.Font, brushText, lstbDigOutputs.GetItemRectangle(index).Location);
            }

            e.DrawFocusRectangle();
        }

        private void txtbDigOutputs_TextChanged(object sender, EventArgs e)
        {
            if (txtbDigOutputsFilter.Text != "")
            {
                lstbDigOutputs.Items.Clear();
                foreach (DigOutput digOutput in ensorIOController.digOutputs)
                {
                    if (digOutput.Symbol.Contains(txtbDigOutputsFilter.Text)){
                        lstbDigOutputs.Items.Add(digOutput.Symbol);
                    }
                }
            }
            else
            {
                foreach (DigOutput digOutput in ensorIOController.digOutputs)
                    lstbDigOutputs.Items.Add(digOutput.Symbol);
            }
        }

        void lstbOutputs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (DigOutput digOutput in ensorIOController.digOutputs)
            {
                if (digOutput.Symbol.Equals(lstbDigOutputs.SelectedItem))
                {
                    ensorIOController.SetDigOutputByGUI(lstbDigOutputs.SelectedItem.ToString(), !digOutput.CurrentVal);
                    ensorIOSockeAdaptor.SendIOUpdate();
                    break;
                }
            }
        }


        private void lstbInputs_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (ensorIOController == null)
                return;

            e.DrawBackground();
            bool selected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);

            int index = e.Index;
            if (index >= 0 && index < lstbDigInputs.Items.Count)
            {
                DigInput tempDigInput = null;
                foreach (DigInput digInput in ensorIOController.digInputs)
                {
                    if (digInput.Symbol.Equals(lstbDigInputs.Items[index]))
                    {
                        tempDigInput = digInput;
                        break;
                    }
                }
                string text = tempDigInput.Symbol;
                Graphics g = e.Graphics;

                //background:
                Brush backgroundBrush;
                if (selected)
                {
                    backgroundBrush = tempDigInput.CurrentVal ? brushTrueSelected : brushFalseSelected;
                }
                else
                {
                    backgroundBrush = tempDigInput.CurrentVal ? brushTrue : brushFalse;
                }
                g.FillRectangle(backgroundBrush, e.Bounds);

                //text:
                Brush foregroundBrush = (selected) ? Brushes.Red : Brushes.Bisque;
                g.DrawString(text, e.Font, brushText, lstbDigInputs.GetItemRectangle(index).Location);
            }

            e.DrawFocusRectangle();
        }

        private void txtbDigInputs_TextChanged(object sender, EventArgs e)
        {
            if (txtbDigInputsFilter.Text != "")
            {
                lstbDigInputs.Items.Clear();
                foreach (DigInput digInput in ensorIOController.digInputs)
                {
                    if (digInput.Symbol.Contains(txtbDigInputsFilter.Text))
                    {
                        lstbDigInputs.Items.Add(digInput.Symbol);
                    }
                }
            }
            else
            {
                foreach (DigInput digInput in ensorIOController.digInputs)
                    lstbDigInputs.Items.Add(digInput.Symbol);
            }
        }

        void lstbInputs_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            foreach (DigInput digInput in ensorIOController.digInputs)
            {
                if (digInput.Symbol.Equals(lstbDigInputs.SelectedItem))
                {
                    ensorIOController.SetDigInputByGUI(lstbDigInputs.SelectedItem.ToString(), !digInput.CurrentVal);
                    ensorIOSockeAdaptor.SendIOUpdate();
                    break;
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }
    }
}
