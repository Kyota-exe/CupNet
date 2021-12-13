using System;
using System.Net.Sockets;

namespace CupNet
{
    public class Client
    {
        // Should dataBufferSize be a const? Could be put in App.config?
        public static int dataBufferSize = 4096;
        public int id;
        public TCP tcp;

        public Client(int _clientId)
        {
            id = _clientId;
            tcp = new TCP(id);
        }

        public class TCP
        {
            public TcpClient socket;
            
            private readonly int id;
            private NetworkStream stream;
            private byte[] receiveBuffer;

            public TCP(int _id)
            {
                id = _id;
            }
            
            public void Connect(TcpClient _socket)
            { 
                socket = _socket;
                socket.ReceiveBufferSize = dataBufferSize;
                socket.SendBufferSize = dataBufferSize;

                stream = socket.GetStream();
                receiveBuffer = new byte[dataBufferSize];

                // If data is added to the stream, read data into receiveBuffer and call ReceiveCallback
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, OnStreamDataReceived, null);
            }

            private void OnStreamDataReceived(IAsyncResult asyncResult)
            {
                try
                {
                    int byteLength = stream.EndRead(asyncResult);
                    if (byteLength <= 0)
                    {
                        // Disconnect client
                        return;
                    }

                    // Transfer data from receiveBuffer to data variable for handling
                    byte[] data = new byte[byteLength];
                    Array.Copy(receiveBuffer, data, byteLength);
                    // Handle data

                    stream.BeginRead(receiveBuffer, 0, dataBufferSize, OnStreamDataReceived, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error receiving TCP data: {e.Message}");
                    // Disconnect client
                }
            }
        }
    }
}