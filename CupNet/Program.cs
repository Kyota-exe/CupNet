using System;
using System.Configuration;

namespace CupNet
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "CupNet Server";

            GetServerConfigSettings(out int maxPlayers, out int port);
            Server.Start(maxPlayers, port);
            
            Console.ReadKey();
        }

        private static void GetServerConfigSettings(out int maxPlayers, out int port)
        {
            if (!Int32.TryParse(ConfigurationManager.AppSettings.Get("maxPlayers"), out maxPlayers) || maxPlayers < 0)
            {
                throw new Exception("Could not find \"maxPlayers\" in App.config that is a positive integer.");
            }

            if (!Int32.TryParse(ConfigurationManager.AppSettings.Get("port"), out port) || port < 0)
            {
                throw new Exception("Could not find \"port\" in App.config that is a positive integer.");
            }
        }
    }
}