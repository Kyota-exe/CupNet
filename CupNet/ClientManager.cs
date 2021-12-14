using System;
using System.Net.Sockets;

namespace CupNet
{
    public class ClientManager
    {
        public TcpClient clientSocket;
        
        private readonly int id;
        private NetworkStream stream;
        private byte[] receiveBuffer;
        private Packet receivedData;

        public ClientManager(int _clientId)
        {
            id = _clientId;
        }

        public void Connect(TcpClient _clientSocket)
        {
            clientSocket = _clientSocket;

            stream = clientSocket.GetStream();
            receiveBuffer = new byte[Server.DataBufferSize];
            receivedData = new Packet();
            
            // If data is added to the stream, read data into receiveBuffer and call OnStreamDataReceived
            stream.BeginRead(receiveBuffer, 0, Server.DataBufferSize, OnStreamDataReceived, null);
            
            PacketsHandler.SendWelcome(id, "Welcome to the server.");
        }

        private void OnStreamDataReceived(IAsyncResult asyncResult)
        {
            try
            {
                int byteLength = stream.EndRead(asyncResult);
                if (byteLength <= 0)
                {
                    // Disconnect client, should not be receiving 0 (or less than 0?) data
                    return;
                }
                
                // Transfer data from receiveBuffer to data variable for handling
                byte[] data = new byte[byteLength];
                Array.Copy(receiveBuffer, data, byteLength);
                if (HandleData(data))
                {
                    receivedData = null;
                    receivedData = new Packet();
                }

                stream.BeginRead(receiveBuffer, 0, Server.DataBufferSize, OnStreamDataReceived, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error receiving TCP data: {e.Message}");
                // Disconnect client
            }
        }
        
        private bool HandleData(byte[] data)
        {
            receivedData.Write(data);
        
            int packetSize = 0;
        
            // New packet beginning
            if (receivedData.UnreadSize() >= sizeof(int))
            {
                packetSize = receivedData.ReadInt();
            
                // Invalid packet? Not sure why or if we need this.
                // Maybe it's for when the client is sending a packet rather than receiving one?
                /* if (packetSize <= 0) return true; */
            }

            // As long as this while loop is running, the packet has not completed reading.
            while (packetSize > 0 && packetSize <= receivedData.UnreadSize())
            {
                // If bugs are encountered, it could be because this following code block
                // is not running on the main thread only, thus causing concurrency issues.
                {
                    byte[] packetBytes = receivedData.ReadBytes(packetSize);
                    Packet packet = new Packet(packetBytes);
                    PacketsHandler.ReceivedPacketHandler(packet);
                }

                packetSize = 0;
                if (receivedData.UnreadSize() >= sizeof(int))
                {
                    packetSize = receivedData.ReadInt();
                    // Packet is not ready yet
                    if (packetSize <= 0) return true;
                }
            }

            return packetSize <= 1;
        }

        public void SendTCPData(Packet packet)
        {
            packet.WriteLength();
            try
            {
                stream.BeginWrite(packet.ToArray(), 0, packet.Size(), null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error sending packet to client {id} via TCP: {e.Message}");
            }
        }
    }
}