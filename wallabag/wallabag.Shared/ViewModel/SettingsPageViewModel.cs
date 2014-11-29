using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using wallabag.Common;

namespace wallabag.ViewModel
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private string _wallabagUrl;
        public string wallabagUrl
        {
            get { return _wallabagUrl; }
            set { Set(() => wallabagUrl, ref _wallabagUrl, value); }
        }

        private int _userId = 1;
        public int userId
        {
            get { return _userId; }
            set { Set(() => userId, ref _userId, value); }
        }

        private string _Token;
        public string Token
        {
            get { return _Token; }
            set { Set(() => Token, ref _Token, value); }
        }

        private bool _refreshOnStartup;
        public bool refreshOnStartup
        {
            get { return _refreshOnStartup; }
            set { Set(() => refreshOnStartup, ref _refreshOnStartup, value); }
        }

        private bool _enableAddLink;
        public bool enableAddLink
        {
            get { return _enableAddLink; }
            set { Set(() => enableAddLink, ref _enableAddLink, value); }
        }
               
        public RelayCommand saveCommand { get; private set; }
        private void saveSettings()
        {
            //TODO: Check for framabag!
            string wlbgUrl = "http://" + wallabagUrl;
            if (wallabagUrl.StartsWith("http://") || wallabagUrl.StartsWith("https://")) wlbgUrl = wallabagUrl;
            ApplicationSettings.SetSetting<string>("wallabagUrl", wlbgUrl, true);
            ApplicationSettings.SetSetting<int>("userId", userId, true);
            ApplicationSettings.SetSetting<string>("Token", Token, true);
            ApplicationSettings.SetSetting<bool>("refreshOnStartup", refreshOnStartup, true);
            ApplicationSettings.SetSetting<bool>("enableAddLink", enableAddLink);
        }

        private void loadSettings()
        {
            wallabagUrl = ApplicationSettings.GetSetting<string>("wallabagUrl", "", true);
            userId = ApplicationSettings.GetSetting<int>("userId", 1, true);
            Token = ApplicationSettings.GetSetting<string>("Token", "", true);
            refreshOnStartup = ApplicationSettings.GetSetting<bool>("refreshOnStartup", false, true);
            enableAddLink = ApplicationSettings.GetSetting<bool>("enableAddLink", false);
        }

        public SettingsPageViewModel()
        {
            loadSettings();
            saveCommand = new RelayCommand(() => saveSettings());
        }
    }
}
