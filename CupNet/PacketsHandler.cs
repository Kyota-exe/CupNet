using System;

namespace CupNet
{
    public static class PacketsHandler
    {
        private enum ServerPacketsId
        {
            Welcome = 0
        }

        private enum ClientPacketsId
        {
            WelcomeReceived = 0
        }
        
        #region Received Packet Handlers
        public static void ReceivedPacketHandler(Packet packet)
        {
            // packet argument should not have the packet size as it's first 4 bytes (int).
            // That part has already been read by the ClientManager. The next 4 bytes (int) are the packet ID.
            int packetId = packet.ReadInt();
        
            // Error if invalid packetId read from packet
            if (!Enum.IsDefined(typeof(ClientPacketsId), packetId))
            {
                Console.WriteLine("Invalid received packet ID.");
                return;
            }

            ClientPacketsId packetIdAsEnum = (ClientPacketsId)packetId;
            switch (packetIdAsEnum)
            {
                case ClientPacketsId.WelcomeReceived:
                    WelcomeReceivedHandler(packet);
                    break;
                default:
                    Console.WriteLine($"Could not find received packet handler for packet ID {packetId}");
                    break;
            }
        }
        
        private static void WelcomeReceivedHandler(Packet packet)
        {
            int clientId = packet.ReadInt();
            string username = packet.ReadString();
            
            Console.WriteLine($"Player \"{username}\" ({Server.clients[clientId].clientSocket.Client.RemoteEndPoint}) connected successfully with client ID {clientId}");
            
            // Send player to game
        }
        #endregion
        
        #region Packet Sending Methods
        public static void SendWelcome(int clientId, string message)
        {
            Packet packet = new Packet(0); // 0 is the temporary packetId for the welcome packet
            packet.Write(message);
            packet.Write(clientId);
            Server.clients[clientId].SendTCPData(packet);
        }
        #endregion
    }
}