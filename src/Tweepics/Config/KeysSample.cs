using System;
using System.Configuration;

namespace Tweepics.Config
{
    public class KeysSample
    {
        public static string key1 = ConfigurationManager.AppSettings["appSettingsKey1"];
        public static string key2 = ConfigurationManager.AppSettings["appSettingsKey2"];
        public static string key3 = ConfigurationManager.AppSettings["appSettingsKey3"];
    }
}
