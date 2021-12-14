using System;
using System.Net.Sockets;

namespace CupNet
{
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
            socket.ReceiveBufferSize = Server.DataBufferSize;
            socket.SendBufferSize = Server.DataBufferSize;

            stream = socket.GetStream();
            receiveBuffer = new byte[Server.DataBufferSize];

            // If data is added to the stream, read data into receiveBuffer and call ReceiveCallback
            stream.BeginRead(receiveBuffer, 0, Server.DataBufferSize, OnStreamDataReceived, null);

            ServerSend.Welcome(id, "Welcome to the server.");
        }

        public void SendData(Packet packet)
        {
            try
            {
                stream.BeginWrite(packet.ToArray(), 0, packet.Length(), null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending packet to client {id} via TCP: {e.Message}");
            }
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

                stream.BeginRead(receiveBuffer, 0, Server.DataBufferSize, OnStreamDataReceived, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error receiving TCP data: {e.Message}");
                // Disconnect client
            }
        }
    }
}