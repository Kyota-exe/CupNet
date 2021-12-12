using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace CupNet
{
    public static class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }
        // clients could just be a list.
        public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
        private static TcpListener tcpListener;

        public static void Start(int _maxPlayers, int _port)
        {
            MaxPlayers = _maxPlayers;
            Port = _port;

            Console.WriteLine("Starting server...");
            
            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();

            Console.WriteLine($"Server started on {Port}.");
            Console.WriteLine($"maxPlayers: {MaxPlayers}");
            
            tcpListener.BeginAcceptTcpClient(OnTcpClientConnect, null);
        }

        private static void OnTcpClientConnect(IAsyncResult asyncResult)
        {
            TcpClient client = tcpListener.EndAcceptTcpClient(asyncResult);
            tcpListener.BeginAcceptTcpClient(OnTcpClientConnect, null);
            
            Console.WriteLine($"Incoming connection from {client.Client.RemoteEndPoint}...");

            // Go through clients dict and find empty slot to place client in
            for (int i = 0; i < MaxPlayers; i++)
            {
                if (clients[i].tcp.socket == null)
                {
                    clients[i].tcp.Connect(client);
                    return;
                }
            }
            
            Console.WriteLine($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
        }

        private static void InitializeServerData()
        {
            // Why must we initialize all of the client objects at the start,
            // instead of adding them as the clients themselves are connecting?
            for (int i = 0; i < MaxPlayers; i++)
            {
                clients.Add(i, new Client(i));
            }
        }
    }
}