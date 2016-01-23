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
    class SocketClient
    {
        private TcpListener tcpListener;
        private Thread mainWaitingThread;
        private TcpClient tcpClient;
        private NetworkStream streamClient;
        private bool serverStarted;
        private bool waitingForConnection;
        private CancellationToken _ct;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public SocketClient(string _IP, int _Port)
        {
            IPAddress localAddr = IPAddress.Parse(_IP);
            tcpListener = new TcpListener(localAddr, _Port);
            serverStarted = false;
        }



        public void StartServer()
        {       
            tcpListener.Start();
            _ct = _cts.Token;
            Console.WriteLine("Waiting for client ...");
            tcpListener.BeginAcceptTcpClient(ProcessRequest, tcpListener);
            serverStarted = true;
        }

        public void StopServer()
        {
            // If listening has been cancelled, simply go out from method.
            if (_ct.IsCancellationRequested)
            {
                return;
            }

            // Cancels listening.
            _cts.Cancel();

            // Waits a little, to guarantee 
            // that all operation receive information about cancellation.
            Thread.Sleep(100);
            tcpListener.Stop();
        }

        private void ProcessRequest(IAsyncResult ar)
        {
            //Stop if operation was cancelled.
            if (_ct.IsCancellationRequested)
            {
                return;
            }

            var listener = ar.AsyncState as TcpListener;
            if (listener == null)
            {
                return;
            }

            // Check cancellation again. Stop if operation was cancelled.
            if (_ct.IsCancellationRequested)
            {
                return;
            }

            // Starts waiting for the next request.
            // listener.BeginAcceptTcpClient(ProcessRequest, listener);

            // Gets client and starts processing received request.

            Console.WriteLine("Connected ...");
            tcpClient = listener.EndAcceptTcpClient(ar);
            streamClient = tcpClient.GetStream();
        }

        public bool GetServerActive()
        {
            return serverStarted;
        }

        public static void AccptClnt(ref TcpClient client, TcpListener listener)
        {
            if (client == null)
                client = listener.AcceptTcpClient();
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
                Console.WriteLine("ERROR SEND DATA: " + e1.Message);
                // Conecction lost, wait for reconnect
                streamClient = null;
                Console.WriteLine("Waiting for client ...");
                tcpListener.BeginAcceptTcpClient(ProcessRequest, tcpListener);              
                return false;
            }
            Console.WriteLine("Data send");
            return true;
        }

        public bool DataAvailable()
        {
            if (tcpListener.Pending() || streamClient == null)
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
