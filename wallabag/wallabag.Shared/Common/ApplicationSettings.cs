using System;
using System.Collections.Generic;
using System.Text;
using Windows.Storage;

namespace wallabag.Common
{
    public static class ApplicationSettings
    {
        public static void SetSetting<T>(string key, T value, bool roaming = true)
        {
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            if (roaming) settings = ApplicationData.Current.RoamingSettings;

            settings.Values[key] = value;
        }

        public static T GetSetting<T>(string key)
        {
            return GetSetting(key, default(T));
        }

        public static T GetSetting<T>(string key, T defaultValue, bool roaming = true)
        {
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            if (roaming) settings = ApplicationData.Current.RoamingSettings;

            if (settings.Values.ContainsKey(key))
                return (T)settings.Values[key];

            return (T)defaultValue;
        }

        public static void RemoveSetting(string key, bool roaming = true)
        {
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            if (roaming) settings = ApplicationData.Current.RoamingSettings;

            if (!settings.Values.ContainsKey(key))
                settings.Values.Remove(key);
        }

        public static void ClearSettings(bool roaming = true)
        {
            ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;
            if (roaming) settings = ApplicationData.Current.RoamingSettings;
            settings.Values.Clear();
        }
    }
}
