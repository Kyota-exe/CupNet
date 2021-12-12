using System;

namespace CupNet
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "CupNet Server";
            Server server = new Server();
            server.Start(0, 0);
            Console.ReadKey();
        }
    }
}