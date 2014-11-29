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
            if (roaming)
            {
                if (!ApplicationData.Current.RoamingSettings.Values.ContainsKey(key))
                    ApplicationData.Current.RoamingSettings.Values[key] = value;
            }
            else
            {
                if (!ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
                    ApplicationData.Current.LocalSettings.Values[key] = value;
            }
        }

        public static T GetSetting<T>(string key)
        {
            return GetSetting(key, default(T));
        }

        public static T GetSetting<T>(string key, T defaultValue, bool roaming = true)
        {
            if (roaming)
            {
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(key))
                    return (T)ApplicationData.Current.RoamingSettings.Values[key];
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
                    return (T)ApplicationData.Current.LocalSettings.Values[key];
            }
            return (T)defaultValue;
        }
    }
}
