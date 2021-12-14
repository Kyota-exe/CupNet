using System;
using System.Configuration;

namespace CupNet
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "CupNet Server";

            GetServerConfigSettings(out int maxPlayers, out int port, out int dataBufferSize);
            Server.Start(maxPlayers, port, dataBufferSize);

            Console.ReadKey();
        }

        private static void GetServerConfigSettings(out int maxPlayers, out int port, out int dataBufferSize)
        {
            /*
            maxPlayers = ConfigurationManager.AppSettings.Get("maxPlayers");
            port = ConfigManager.GetValue<int>("port");
            dataBufferSize = ConfigManager.GetValue<int>("dataBufferSize");
            */

            // Use values here instead of App.config for now.
            maxPlayers = 4;
            port = 9812;
            dataBufferSize = 4096;
        }
    }
}