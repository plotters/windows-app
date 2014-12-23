using Windows.Storage;
using System;
using System.ComponentModel;

namespace wallabag.Common
{
    public class ApplicationSettings : INotifyPropertyChanged
    {
        private static ApplicationSettings _instance;
        private ApplicationSettings() { }
        public static ApplicationSettings Instance { get { return _instance ?? (_instance = new ApplicationSettings()); } }

        private ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;

        private ObservableDictionary _settings;
        public ObservableDictionary Settings
        {
            get
            {
                if (_settings == null || _settings.Count == 0)
                {
                    _settings = new ObservableDictionary();
                    foreach (var s in ApplicationData.Current.RoamingSettings.Values)
                    {
                        _settings.Add(s.Key, s.Value);
                    }
                }
                return _settings;
            }
        }

        public dynamic this[string key, object defaultValue = default(object)]
        {
            get
            {
                if (Settings.ContainsKey(key))
                {
                    return Settings[key];
                }
                return defaultValue;
            }
            set
            {
                Settings[key] = value;
                RaisePropertyChanged(key);
                roamingSettings.Values[key] = value;
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
                System.Diagnostics.Debug.WriteLine("Set property: " + propertyName );
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
