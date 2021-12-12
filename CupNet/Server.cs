using System;
using System.Net;
using System.Net.Sockets;

namespace CupNet
{
    public class Server
    {
        public static int MaxPlayers { get; private set; }
        public static int Port { get; private set; }

        private static TcpListener tcpListener;

        public void Start(int _maxPlayers, int _port)
        {
            MaxPlayers = _maxPlayers;
            Port = _port;

            Console.WriteLine("Starting server...");
            
            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();

            Console.WriteLine($"Server started on {Port}.");
            
            tcpListener.BeginAcceptTcpClient(OnTcpClientConnect, null);
        }

        private static void OnTcpClientConnect(IAsyncResult asyncResult)
        {
            TcpClient client = tcpListener.EndAcceptTcpClient(asyncResult);
            tcpListener.BeginAcceptTcpClient(OnTcpClientConnect, null);
        }
    }
}