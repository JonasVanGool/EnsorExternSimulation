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
        private TcpClient tcpClient;
        private NetworkStream streamClient;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private string ip;
        private int port;

        public SocketClient(string _IP, int _Port)
        {
            ip= _IP;
            port = _Port; 
        }

        public void CloseConnection()
        {
            if(streamClient != null)
                streamClient.Close();
            if (tcpClient != null)
                tcpClient.Close();
            streamClient = null;
            tcpClient = null;
        }

        public bool Connect(){
            try
            {
                if (tcpClient == null)
                    tcpClient = new TcpClient(ip, port);
                else
                    tcpClient.Connect(ip, port);

                if (tcpClient.Connected)
                {
                    streamClient = tcpClient.GetStream();
                }
                return tcpClient.Connected;
            }
            catch (Exception e)
            {
                return false;
            }
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
