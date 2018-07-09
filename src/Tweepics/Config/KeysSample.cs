using System;
using System.Configuration;

namespace Tweepics.Config
{
    public class KeysSample
    {
        public readonly static string key1 = ConfigurationManager.AppSettings["appSettingsKey1"];
        public readonly static string key2 = ConfigurationManager.AppSettings["appSettingsKey2"];
        public readonly static string key3 = ConfigurationManager.AppSettings["appSettingsKey3"];
    }
}