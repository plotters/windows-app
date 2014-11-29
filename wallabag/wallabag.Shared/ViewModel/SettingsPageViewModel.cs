using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using wallabag.Common;
using System.Collections.ObjectModel;

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

        private ObservableCollection<string> _Fonts;
        public ObservableCollection<string> Fonts
        {
            get { return _Fonts; }
            set { Set(() => Fonts, ref _Fonts, value); }
        }

        private int _fontSize = 20;
        public int fontSize
        {
            get { return _fontSize; }
            set { Set(() => fontSize, ref _fontSize, value); }
        }

        private double _lineHeight = 1.5;
        public double lineHeight
        {
            get { return _lineHeight; }
            set { Set(() => lineHeight, ref _lineHeight, value); }
        }

        public RelayCommand saveCommand { get; private set; }
        private void saveSettings()
        {
            //TODO: Check for framabag!
            string wlbgUrl = "http://" + wallabagUrl;
            if (wallabagUrl.StartsWith("http://") || wallabagUrl.StartsWith("https://")) wlbgUrl = wallabagUrl;
            ApplicationSettings.SetSetting<string>("wallabagUrl", wlbgUrl);
            ApplicationSettings.SetSetting<int>("userId", userId);
            ApplicationSettings.SetSetting<string>("Token", Token);
            ApplicationSettings.SetSetting<bool>("refreshOnStartup", refreshOnStartup);
            ApplicationSettings.SetSetting<bool>("enableAddLink", enableAddLink, false);
            ApplicationSettings.SetSetting<int>("fontSize", fontSize);
            ApplicationSettings.SetSetting<double>("lineHeight", lineHeight);
        }

        private void loadSettings()
        {
            wallabagUrl = ApplicationSettings.GetSetting<string>("wallabagUrl", "");
            userId = ApplicationSettings.GetSetting<int>("userId", 1);
            Token = ApplicationSettings.GetSetting<string>("Token", "");
            refreshOnStartup = ApplicationSettings.GetSetting<bool>("refreshOnStartup", false);
            enableAddLink = ApplicationSettings.GetSetting<bool>("enableAddLink", false, false);
            fontSize = ApplicationSettings.GetSetting<int>("fontSize", 20);
            lineHeight = ApplicationSettings.GetSetting<double>("lineHeight", 1.5);
        }

        public SettingsPageViewModel()
        {
            loadSettings();
            saveCommand = new RelayCommand(() => saveSettings());
        }
    }
}
