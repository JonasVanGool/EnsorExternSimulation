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
    public partial class Form1 : Form
    {
        SocketServer socketServer;
        EnsorIOController ensorIOController;
        EnsorIOSocketAdaptor ensorIOSockeAdaptor;
        Thread updateGUI;
        int guiUpdateFreq;

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_Closing;
            socketServer = new SocketServer(tbxServerIp.Text,int.Parse(tbxServerPort.Text));
            guiUpdateFreq = 5; //Hz
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ensorIOSockeAdaptor.StopService();
            socketServer.StopServer();
        }

        private void btnStartServer_Click(object sender, EventArgs e)
        {
            if (!socketServer.GetServerActive()){
                ensorIOController = new EnsorIOController(@"C:\Team Systems\Ensor - Projects\Standard_FLV-JonasTestProject\EnsConf\Bld\Ensor_Standard_FLV.EnsConfLst");
                socketServer.StartServer();
                ensorIOSockeAdaptor = new EnsorIOSocketAdaptor(ensorIOController, socketServer);
                updateGUI = new Thread(new ThreadStart(this.UpdateGui));
                updateGUI.Start();
            }
        }

        private void UpdateGui()
        {
            while (true)
            {
                Thread.Sleep((int)((double)1 / (double)guiUpdateFreq * 1000));
                // Update gui
                if (ensorIOController.GetSocketUpdate())
                {
                    TextBox.CheckForIllegalCrossThreadCalls = false;
                    foreach (DigInput digInput in ensorIOController.digInputs)
                        if (digInput.Symbol.Equals(textBox1.Text))
                            SetBackGroundColor(digInput.CurrentVal ? Color.Green : Color.Red);
                    ensorIOController.ResetSocketUpdate();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ensorIOController.SetDigInputByGUI(textBox1.Text, checkBox1.Checked);
            ensorIOSockeAdaptor.SendIOUpdate();
        }

        delegate void SetBackGroundCallback(Color color);

        private void SetBackGroundColor(Color color)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SetBackGroundCallback d = new SetBackGroundCallback(SetBackGroundColor);
                this.Invoke(d, new object[] { color });
            }
            else
            {
                this.textBox1.BackColor = color;
            }
        }
    }
}
