using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnsorExternSimulation
{
    class EnsorIOSocketAdaptor
    {
        private EnsorIOController ensorIOController;
        private SocketClient socketClient;
        private Thread waitForReceive;
        private Thread pollData;
        private ExternSimulationPackage receivePackage;
        private ExternSimulationPackage sendPackage;
        private int pollFreq;
        private int checkReceiveFreq;
        private bool allowReceive;
        private Stopwatch stopWatch;
        private long lastTimeSend;
        private long lastTimeReceived;
        private long deltaTime;
        private bool connectionAlive = false;

        public EnsorIOSocketAdaptor(ref EnsorIOController ioController)
        {
            stopWatch = new Stopwatch();
            stopWatch.Start();
            ensorIOController = ioController;
            checkReceiveFreq = 50; //Hz
        }

        public bool MakeConnection(string _ip, int _port)
        {
            try
            {
                // Make socket connection
                socketClient = new SocketClient(_ip, _port);
                socketClient.Connect();
                try
                {
                    // Set up data thread
                    allowReceive = true;
                    waitForReceive = new Thread(new ThreadStart(this.WaitForReceive));
                    waitForReceive.Start();
                    // Send first poll message to start ping pong systen
                    this.PollData();
                    return true;
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Unable to start data thread!\n" + exp.Message);
                    return false;
                }
               
            }
            catch (Exception exp)
            {
                // Display error message
                MessageBox.Show("Unable to connect!\n" + exp.Message);
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                // Stop data thread
                allowReceive = false;
                // Wait for thread to finish
                long startTerminateThreadTime = stopWatch.ElapsedMilliseconds;
                while (waitForReceive.IsAlive)
                {
                    //Check elapsed time
                    if (stopWatch.ElapsedMilliseconds - startTerminateThreadTime > 100)
                    {
                        // Display error message
                        MessageBox.Show("Unable to stop data thread!");
                    }
                }
                try
                {
                    // Close socket connection
                    socketClient.CloseConnection();
                    return true;
                }
                catch (Exception exp)
                {
                    // Display error message
                    MessageBox.Show("Unable to close socket connection!\n" + exp.Message);
                    return false;
                }
            }
            catch (Exception exp)
            {
                // Display error message
                MessageBox.Show("Unable to close connection!\n" + exp.Message);
                return false;
            }
        }

        public long GetTimeToLastReceive()
        {
            return stopWatch.ElapsedMilliseconds - lastTimeReceived;
        }

        public bool SendIOUpdate()
        {
            // convert EnsorIOcontroller in to Extern simulation packege
            sendPackage = new ExternSimulationPackage();

            // convert dig inputs
            foreach (DigInput digInput in ensorIOController.digInputs)
            {
                int byteNr = (int)Math.Floor((double)digInput.IdxNr / 8.0);
                int bitNr = digInput.IdxNr % 8;
                byte mask = (byte)((digInput.CurrentVal?1:0) << bitNr);
                sendPackage.digInputs[byteNr] |= mask;
            }

            // convert dig outputs
            foreach (DigOutput digOutput in ensorIOController.digOutputs)
            {
                int byteNr = (int)Math.Floor((double)digOutput.IdxNr / 8.0);
                int bitNr = digOutput.IdxNr % 8;
                sendPackage.digOutputs[byteNr] |= (byte)((digOutput.CurrentVal?1:0) << bitNr);
            }

            // convert num inputs
            foreach (NumInput numInput in ensorIOController.numInputs)
            {
                sendPackage.numInputs[numInput.IdxNr] = numInput.CurrentVal;
            }

            // convert num outputs
            foreach (NumOutput numOutput in ensorIOController.numOutputs)
            {
                sendPackage.numOutputs[numOutput.IdxNr] = numOutput.CurrentVal;
            }

            if (socketClient.SendData(sendPackage.ToByteArray(), sendPackage.ToByteArray().Length))
            {
                ensorIOController.ResetGUIBusyWriting();
                return true;
            }
            else
            {
                return false;
            }          
        }

        private bool PollData()
        {
            Byte[] tempArray = new Byte[1];
            tempArray[0] = 0x70; //'p'
            if (socketClient.SendData(tempArray, tempArray.Length))
            {
                lastTimeSend = stopWatch.ElapsedMilliseconds;
                return true;
            }
            return false;         
        }

        private void WaitForReceive(){
            while (allowReceive)
            {
                int sleep = (int)((1.0 / (double)checkReceiveFreq) * 1000);
                Thread.Sleep(sleep);    
                if (socketClient.DataAvailable())
                {
                    // Update receive time
                    lastTimeReceived = stopWatch.ElapsedMilliseconds;

                    // Read bytes
                    ExternSimulationPackage tempP = new ExternSimulationPackage();
                    Byte[] tempBuffer = new Byte[tempP.GetSize()];
                    socketClient.ReadData(tempBuffer, tempP.GetSize());
                    receivePackage = new ExternSimulationPackage(tempBuffer);

                    Console.WriteLine("Recevied" + receivePackage.numInputs[3].ToString());
                    // read dig inputs
                    // loop bytes
                    for (int i = 0; i < receivePackage.digInputs.Length; i++)
                    {
                        //loop bits
                        for(int j = 0; j<8 ; j++){
                            ensorIOController.SetDigInputBySocket(i*8+j,(receivePackage.digInputs[i] & (1 << j)) != 0);
                        } 
                    }

                    // read dig outputs
                    // loop bytes
                    for (int i = 0; i < receivePackage.digOutputs.Length; i++)
                    {
                        //loop bits
                        for (int j = 0; j < 8; j++)
                        {
                            ensorIOController.SetDigOutputBySocket(i*8 + j, (receivePackage.digOutputs[i] & (1 << j)) != 0);
                        }
                    }

                    // read num outputs
                    for (int i = 0; i < receivePackage.numOutputs.Length; i++)
                    {
                        ensorIOController.SetNumOutputBySocket(i, receivePackage.numOutputs[i]);
                    }

                    // read num inputs
                    for (int i = 0; i < receivePackage.numInputs.Length; i++)
                    {
                        ensorIOController.SetNumInputBySocket(i, receivePackage.numInputs[i]);
                    }

                    // Resend poll message
                    if (!ensorIOController.GetGUIBusyWriting())
                    {
                       connectionAlive = this.PollData();
                       
                    }
                    else
                    {
                       connectionAlive = SendIOUpdate();
                    }

                    if (!connectionAlive)
                    {
                        // Stop sending data untin new connection is made
                        allowReceive = false;
                    }
                    else
                    {
                        // Last time valid send
                        lastTimeSend = stopWatch.ElapsedMilliseconds;
                    }
                        
                }
            }
        }

        public bool ConnectionAlive()
        {
            return connectionAlive && GetTimeToLastReceive() < (((1.0 / (double)checkReceiveFreq) * 1000) * 2);
        }
    }
}
