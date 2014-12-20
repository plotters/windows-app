using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using wallabag.Common;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace wallabag.ViewModel
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public string wallabagUrl
        {
            get { return ApplicationSettings.GetSetting<string>("wallabagUrl", ""); }
            set
            {
                string tmp = "http://" + value;
                if (value.StartsWith("http://") || value.StartsWith("https://")) tmp = value;
                ApplicationSettings.SetSetting<string>("wallabagUrl", tmp);
                RaisePropertyChanged(() => wallabagUrl);
            }
        }
        public int userId
        {
            get { return ApplicationSettings.GetSetting<int>("userId", 1); }
            set
            {
                ApplicationSettings.SetSetting<int>("userId", value);
                RaisePropertyChanged(() => userId);
            }
        }
        public string Token
        {
            get { return ApplicationSettings.GetSetting<string>("Token", ""); }
            set
            {
                ApplicationSettings.SetSetting<string>("Token", value);
                RaisePropertyChanged(() => Token);
            }
        }
        public bool refreshOnStartup
        {
            get { return ApplicationSettings.GetSetting<bool>("refreshOnStartup", false); }
            set
            {
                ApplicationSettings.SetSetting<bool>("refreshOnStartup", value);
                RaisePropertyChanged(() => refreshOnStartup);
            }
        }
        public bool enableAddLink
        {
            get { return ApplicationSettings.GetSetting<bool>("enableAddLink", false); }
            set
            {
                ApplicationSettings.SetSetting<bool>("enableAddLink", value);
                RaisePropertyChanged(() => enableAddLink);
            }
        }
        public double fontSize
        {
            get { return ApplicationSettings.GetSetting<double>("fontSize", 16); }
            set
            {
                ApplicationSettings.SetSetting<double>("fontSize", value);
                RaisePropertyChanged(() => fontSize);
            }
        }
        public double lineHeight
        {
            get { return ApplicationSettings.GetSetting<double>("lineHeight", 1.5); }
            set
            {
                ApplicationSettings.SetSetting<double>("lineHeight", value);
                RaisePropertyChanged(() => lineHeight);
            }
        }
        public bool isLightMode
        {
            get { return ApplicationSettings.GetSetting<bool>("isLightMode", false); }
            set
            {
                ApplicationSettings.SetSetting<bool>("isLightMode", value);
                RaisePropertyChanged(() => isLightMode);
            }
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
#if WINDOWS_PHONE_APP
                if (isLightMode)
                    return new SolidColorBrush(ColorHelper.FromArgb(255, 250, 247, 238)); // #faf7ee
                else
                    return new SolidColorBrush(ColorHelper.FromArgb(255, 0, 0, 0)); // #000000
#else
                if (isLightMode)
                    return new SolidColorBrush(ColorHelper.FromArgb(255, 250, 247, 238)); // #faf7ee
                else
                    return new SolidColorBrush(ColorHelper.FromArgb(255, 29, 29, 29)); // #1D1D1D
#endif
            }
        }
        
        public RelayCommand resetCommand { get; private set; }

        public SettingsPageViewModel()
        {
            resetCommand = new RelayCommand(() => ApplicationSettings.ClearSettings());
        }
    }
}
