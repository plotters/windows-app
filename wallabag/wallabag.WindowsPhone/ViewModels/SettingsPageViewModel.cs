using MyToolkit.Command;
using MyToolkit.Mvvm;
using MyToolkit.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace wallabag.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private string _wallabagUrl;
        public string wallabagUrl
        {
            get { return _wallabagUrl; }
            set
            {
                if (value != _wallabagUrl)
                {
                    _wallabagUrl = value;
                    RaisePropertyChanged(() => wallabagUrl);
                }
            }
        }

        private int _userId = 1;
        public int userId
        {
            get { return _userId; }
            set
            {
                if (value != _userId)
                {
                    _userId = value;
                    RaisePropertyChanged(() => userId);
                }
            }
        }

        private string _Token;
        public string Token
        {
            get { return _Token; }
            set
            {
                if (value != _Token)
                {
                    _Token = value;
                    RaisePropertyChanged(() => Token);
                }
            }
        }

        private bool _refreshOnStartup;
        public bool refreshOnStartup
        {
            get { return _refreshOnStartup; }
            set
            {
                if (value != _refreshOnStartup)
                {
                    _refreshOnStartup = value;
                    RaisePropertyChanged(() => refreshOnStartup);
                }
            }
        }

        private bool _enableAddLink;
        public bool enableAddLink
        {
            get { return _enableAddLink; }
            set
            {
                if (value != _enableAddLink)
                {
                    _enableAddLink = value;
                    RaisePropertyChanged(() => enableAddLink);
                }
            }
        }
        
        public RelayCommand saveCommand { get; private set; }
        private void save()
        {
            //TODO: Check for framabag!
            string wlbgUrl = "http://" + wallabagUrl;
            if (wallabagUrl.StartsWith("http://") || wallabagUrl.StartsWith("https://")) wlbgUrl = wallabagUrl;
            ApplicationSettings.SetSetting<string>("wallabagUrl", wlbgUrl, true);
            ApplicationSettings.SetSetting<int>("userId", userId, true);
            ApplicationSettings.SetSetting<string>("Token", Token, true);
            ApplicationSettings.SetSetting<bool>("refreshOnStartup", refreshOnStartup, true);
            ApplicationSettings.SetSetting<bool>("enableAddLink", enableAddLink);

            ((Frame)Window.Current.Content).GoBack();
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
            saveCommand = new RelayCommand(() => save());
        }
    }
}
