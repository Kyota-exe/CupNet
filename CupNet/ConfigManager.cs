using System.ComponentModel;
using System.Configuration;

namespace CupNet
{
    public static class ConfigManager
    { 
        public static T GetValue<T>(string key)
        {
            string stringValue = ConfigurationManager.AppSettings.Get(key);
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(stringValue);
            
            /*if (!Int32.TryParse(ConfigurationManager.AppSettings.Get("maxPlayers"), out maxPlayers) || maxPlayers < 0)
            {
                throw new Exception("Could not find \"maxPlayers\" in App.config that is a positive integer.");
            }

            if (!Int32.TryParse(ConfigurationManager.AppSettings.Get("port"), out port) || port < 0)
            {
                throw new Exception("Could not find \"port\" in App.config that is a positive integer.");
            }*/
        }
    }
}