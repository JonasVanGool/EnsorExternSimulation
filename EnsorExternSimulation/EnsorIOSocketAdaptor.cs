using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EnsorExternSimulation
{
    class EnsorIOSocketAdaptor
    {
        private EnsorIOController ensorIOController;
        private SocketClient socketServer;
        private Thread waitForReceive;
        private Thread pollData;
        private ExternSimulationPackage receivePackage;
        private ExternSimulationPackage sendPackage;
        private int pollFreq;
        private int checkReceiveFreq;
        private bool allowReceive;
        private bool allowPoll;

        public EnsorIOSocketAdaptor(EnsorIOController ioController, SocketClient server)
        {
            ensorIOController = ioController;
            socketServer = server;
            waitForReceive = new Thread(new ThreadStart(this.WaitForReceive));
            allowReceive= true;
            waitForReceive.Start();
            pollData = new Thread(new ThreadStart(this.PollData));
            allowPoll = true;
            pollData.Start();
            pollFreq = 1; //Hz
            checkReceiveFreq = 10; //Hz
        }

        public void StopService()
        {
            allowPoll = false;
            pollData.Abort();
            allowReceive = false;
            waitForReceive.Abort();
        }
        
        public void SendIOUpdate()
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

            socketServer.SendData(sendPackage.ToByteArray(), sendPackage.ToByteArray().Length);
            ensorIOController.ResetGUIBusyWriting();
        }

        private void PollData()
        {
            while (allowPoll)
            {
                Byte[] tempArray = new Byte[1];
                tempArray[0] = 0x70; //'p'
                socketServer.SendData(tempArray, tempArray.Length);
                Thread.Sleep((int) ((double)1 /(double)pollFreq * 1000));
            }
        }

        private void WaitForReceive(){
            while (allowReceive)
            {
                Thread.Sleep((int)((double)1 / (double)checkReceiveFreq * 1000));    
                if (socketServer.DataAvailable())
                {
                    // Read bytes
                    Byte[] tempBuffer = new Byte[576];
                    socketServer.ReadData(tempBuffer,576);
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

                }
            }
        }
    }
}
