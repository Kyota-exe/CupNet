using System.Linq;

namespace CupNet
{
    public static class ServerSend
    {
        private static void SendTCPData(int clientId, Packet packet, bool writeLength = true)
        {
            if (writeLength) packet.WriteLength();
            Server.clients[clientId].tcp.SendData(packet);
        }

        private static void SendTCPDataAllClients(Packet packet, int[] exceptClients = null)
        {
            packet.WriteLength();
            for (int i = 0; i < Server.MaxPlayers; i++)
            {
                if (exceptClients == null || !exceptClients.Contains(i))
                {
                    SendTCPData(i, packet, false);
                }
            }
        }
        
        public static void Welcome(int clientId, string message)
        {
            Packet packet = new Packet(0); // 0 is the temporary packetId for the welcome packet
            packet.Write(message);
            packet.Write(clientId);
            SendTCPData(clientId, packet);
        }
    }
}