using System;
using System.Collections.Generic;
using System.Text;

namespace wallabag.Common
{
    public static class ApplicationSettings
    {
        public static void SetSetting<T>(string key, T value, bool roaming = false, bool save = false)
        {
        }

        public static T GetSetting<T>(string key)
        {
            return GetSetting(key, default(T));
        }

        public static T GetSetting<T>(string key, T defaultValue, bool roaming = false)
        {
            return default(T);
        }
    }
}
