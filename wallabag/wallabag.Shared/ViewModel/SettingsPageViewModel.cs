using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using wallabag.Common;
using Windows.UI;
using Windows.UI.Xaml.Media;

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

        private int _userId;
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

        private double _fontSize;
        public double fontSize
        {
            get { return _fontSize; }
            set { Set(() => fontSize, ref _fontSize, value); }
        }

        private double _lineHeight;
        public double lineHeight
        {
            get { return _lineHeight; }
            set { Set(() => lineHeight, ref _lineHeight, value); }
        }

        private bool _isLightMode;
        public bool isLightMode
        {
            get { return _isLightMode; }
            set { Set(() => isLightMode, ref _isLightMode, value); }
        }

        public SolidColorBrush textColor
        {
            get {
                if (isLightMode)            
                    return new SolidColorBrush(ColorHelper.FromArgb(255, 0, 0, 0)); // #000000
                else
                    return new SolidColorBrush(ColorHelper.FromArgb(255, 189, 189, 189)); // #bdbdbd
            }
        }

        public SolidColorBrush Background
        {
            get {
                if (isLightMode)
                    return new SolidColorBrush(ColorHelper.FromArgb(255, 250, 247, 238)); // #faf7ee
                else
                    return new SolidColorBrush(ColorHelper.FromArgb(255, 0, 0, 0)); // #000000
            }
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
            ApplicationSettings.SetSetting<double>("fontSize", fontSize);
            ApplicationSettings.SetSetting<double>("lineHeight", lineHeight);
            ApplicationSettings.SetSetting<bool>("isLightMode", isLightMode);
        }

        public RelayCommand resetCommand { get; private set; }
        private void resetSettings()
        {
            ApplicationSettings.ClearSettings();
            loadSettings();
        }

        private void loadSettings()
        {
            wallabagUrl = ApplicationSettings.GetSetting<string>("wallabagUrl", "");
            userId = ApplicationSettings.GetSetting<int>("userId", 1);
            Token = ApplicationSettings.GetSetting<string>("Token", "");
            refreshOnStartup = ApplicationSettings.GetSetting<bool>("refreshOnStartup", false);
            enableAddLink = ApplicationSettings.GetSetting<bool>("enableAddLink", false, false);
            fontSize = ApplicationSettings.GetSetting<double>("fontSize", 16);
            lineHeight = ApplicationSettings.GetSetting<double>("lineHeight", 1.5);
            isLightMode = ApplicationSettings.GetSetting<bool>("IsLightMode", false);
        }

        public SettingsPageViewModel()
        {
            loadSettings();
            saveCommand = new RelayCommand(() => saveSettings());
            resetCommand = new RelayCommand(() => resetSettings());
        }
    }
}
