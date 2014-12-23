using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using wallabag.Common;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace wallabag.ViewModel
{
    public class SettingsViewModel : viewModelBase
    {
        public string wallabagUrl
        {
            get { return AppSettings["wallabagUrl", string.Empty]; }
            set
            {
                string tmp = "http://" + value;
                if (value.StartsWith("http://") || value.StartsWith("https://")) tmp = value;
                AppSettings["wallabagUrl"] = tmp;
                RaisePropertyChanged(() => wallabagUrl);
            }
        }
        public int userId
        {
            get { return AppSettings["userId", 1]; }
            set
            {
                AppSettings["userId"] = value;
                RaisePropertyChanged(() => userId);
            }
        }
        public string Token
        {
            get { return AppSettings["Token", string.Empty]; }
            set
            {
                AppSettings["Token"] = value;
                RaisePropertyChanged(() => Token);
            }
        }
        public bool refreshOnStartup
        {
            get { return AppSettings["refreshOnStartup", false]; }
            set
            {
                AppSettings["refreshOnStartup"] = value;
                RaisePropertyChanged(() => refreshOnStartup);
            }
        }
        public bool enableAddLink
        {
            get { return AppSettings["enableAddLink", false]; }
            set
            {
                AppSettings["enableAddLink"] = value;
                RaisePropertyChanged(() => enableAddLink);
            }
        }
        public double fontSize
        {
            get { return AppSettings["fontSize", 18]; }
            set
            {
                AppSettings["fontSize"] = value;
                RaisePropertyChanged(() => fontSize);
            }
        }
        public double lineHeight
        {
            get { return AppSettings["lineHeight", 1.5]; }
            set
            {
                AppSettings["lineHeight"] = value;
                RaisePropertyChanged(() => lineHeight);
            }
        }
        public bool isLightMode
        {
            get { return AppSettings["isLightMode", false]; }
            set
            {
                AppSettings["isLightMode"] = value;
                RaisePropertyChanged(() => isLightMode);
            }
        }
        
        public RelayCommand resetCommand { get; private set; }

        public SettingsViewModel()
        {
            resetCommand = new RelayCommand(() => AppSettings.Settings.Clear());
        }
    }
}
