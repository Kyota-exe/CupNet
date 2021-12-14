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
        public static int DataBufferSize { get; private set; }
        // clients could just be a list?
        public static Dictionary<int, ClientManager> clients = new Dictionary<int, ClientManager>();
        private static TcpListener tcpListener;

        public static void Start(int _maxPlayers, int _port, int _dataBufferSize)
        {
            MaxPlayers = _maxPlayers;
            Port = _port;
            DataBufferSize = _dataBufferSize;
            // Why must we initialize all of the client objects at the start,
            // instead of adding them as the clients themselves are connecting?
            // Populate clients dictionary
            for (int i = 0; i < MaxPlayers; i++)
            {
                clients.Add(i, new ClientManager(i));
            }

            Console.WriteLine("Starting server...");
            
            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();

            Console.WriteLine($"Server started on {Port}.");
            Console.WriteLine($"maxPlayers: {MaxPlayers}");
            
            tcpListener.BeginAcceptTcpClient(OnTcpClientConnect, null);
        }

        private static void OnTcpClientConnect(IAsyncResult asyncResult)
        { 
            TcpClient clientSocket = tcpListener.EndAcceptTcpClient(asyncResult);
            
            // Call BeginAcceptTcpClient to keep accepting clients (if this wasn't here, I'd only accept the first)
            tcpListener.BeginAcceptTcpClient(OnTcpClientConnect, null);
            
            Console.WriteLine($"Incoming connection from {clientSocket.Client.RemoteEndPoint}...");

            // Go through clients dict and find empty slot to place client in
            for (int i = 0; i < MaxPlayers; i++)
            {
                if (clients[i].clientSocket == null)
                {
                    // Prepare client sockets receive/send buffers
                    clientSocket.ReceiveBufferSize = DataBufferSize;
                    clientSocket.SendBufferSize = DataBufferSize;
                    
                    clients[i].Connect(clientSocket);
                    return;
                }
            }
            
            Console.WriteLine($"{clientSocket.Client.RemoteEndPoint} failed to connect: Server full!");
        }
    }
}