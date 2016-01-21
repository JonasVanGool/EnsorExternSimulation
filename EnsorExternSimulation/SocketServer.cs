using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace EnsorExternSimulation
{
    class SocketServer
    {
        private TcpListener tcpServer;
        private Thread waitForConnection;
        private TcpClient tcpClient;
        private NetworkStream streamClient;
        private bool serverStarted;

        public SocketServer(string _IP, int _Port)
        {
            IPAddress localAddr = IPAddress.Parse(_IP);
            tcpServer = new TcpListener(localAddr, _Port);
            waitForConnection = new Thread(new ThreadStart(this.WaitForConnection));
            serverStarted = false;
        }

        public void StopServer(){
            waitForConnection.Abort();
            tcpClient.Close();
            tcpServer.Stop();
        }

        public void StartServer()
        {       
            tcpServer.Start();
            waitForConnection.Start();
            serverStarted = true;
        }

        public bool GetServerActive()
        {
            return serverStarted;
        }

        private void WaitForConnection()
        {
            Console.WriteLine("Waiting for client ...");
            tcpClient = tcpServer.AcceptTcpClient();
            Console.WriteLine("Connected ...");
            streamClient = tcpClient.GetStream();
        }

        public bool SendData(Byte[] data, int dataLength)
        {
            if (streamClient == null)
                return false;
            try{
                streamClient.Write(data, 0, dataLength);
            }
            catch (Exception e1)
            {
                // Conecction lost, wait for reconnect
                if (waitForConnection.ThreadState != ThreadState.Running)
                {
                    waitForConnection = new Thread(new ThreadStart(this.WaitForConnection));
                    waitForConnection.Start();
                }
                Console.WriteLine("ERROR SEND DATA: " + e1.Message);
                return false;
            }
            Console.WriteLine("Data send");
            return true;
        }

        public bool DataAvailable()
        {
            if (streamClient == null)
                return false;
            return streamClient.DataAvailable;
        }

        public int ReadData(Byte[] buffer, int size)
        {
            if (streamClient == null)
                return 0;
            return streamClient.Read(buffer, 0, size);
        }
    }
}
