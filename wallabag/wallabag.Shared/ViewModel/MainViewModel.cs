using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using wallabag.Common;
using wallabag.Models;
using Windows.ApplicationModel.Resources;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.Web.Syndication;

namespace wallabag.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool _IsRunning;
        public bool IsRunning
        {
            get { return _IsRunning; }
            set { 
                Set(() => IsRunning, ref _IsRunning, value);
                refreshCommand.RaiseCanExecuteChanged();

#if WINDOWS_PHONE_APP    
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                if (value)
                {
                    var resourceLoader = ResourceLoader.GetForCurrentView();
                    statusBar.ProgressIndicator.Text = resourceLoader.GetString("UpdatingText");
                    statusBar.ProgressIndicator.ShowAsync();
                }
                else { statusBar.ProgressIndicator.HideAsync(); }
#endif
            }
        }
        
        public Visibility addLinkButtonVisibility
        {
            get
            {
                if (ApplicationSettings.GetSetting<bool>("enableAddLink", false)) { return Visibility.Visible; } else { return Visibility.Collapsed; }
            }
        }

        private ObservableCollection<ItemViewModel> _unreadItems;
        private ObservableCollection<ItemViewModel> _favouriteItems;
        private ObservableCollection<ItemViewModel> _archivedItems;

        public ObservableCollection<ItemViewModel> unreadItems
        {
            get { return _unreadItems; }
            set { Set(() => unreadItems, ref _unreadItems, value); }
        }
        public ObservableCollection<ItemViewModel> favouriteItems
        {
            get { return _favouriteItems; }
            set { Set(() => favouriteItems, ref _favouriteItems, value); }
        }
        public ObservableCollection<ItemViewModel> archivedItems
        {
            get { return _archivedItems; }
            set { Set(() => archivedItems, ref _archivedItems, value); }
        }

        private bool everythingOkay
        {
            get
            {
                string wallabagUrl = ApplicationSettings.GetSetting<string>("wallabagUrl", "");
                int userId = ApplicationSettings.GetSetting<int>("userId", 1);
                string token = ApplicationSettings.GetSetting<string>("Token", "");

                ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
                bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;

                return wallabagUrl != string.Empty && userId != 0 && token != string.Empty && internet;
            }
        }

        private string buildUrl(string parameter)
        {
            string wallabagUrl = ApplicationSettings.GetSetting<string>("wallabagUrl", "");
            int userId = ApplicationSettings.GetSetting<int>("userId", 1);
            string token = ApplicationSettings.GetSetting<string>("Token", "");

            if (everythingOkay)
                return string.Format("{0}?feed&type={1}&user_id={2}&token={3}", wallabagUrl, parameter, userId, token);

            return string.Empty;
        }

        public RelayCommand refreshCommand { get; private set; }
        private async Task refresh()
        {
            if (everythingOkay)
            {
                IsRunning = true;

                var tmpUnread = new ObservableCollection<ItemViewModel>();
                var tmpFavourites = new ObservableCollection<ItemViewModel>();
                var tmpArchive = new ObservableCollection<ItemViewModel>();

                Windows.Web.Syndication.SyndicationClient client = new SyndicationClient();
                string[] parameters = new string[] { "home", "fav", "archive" };

                foreach (string param in parameters)
                {
                    Uri feedUri = new Uri(buildUrl(param));
                    try
                    {
                        SyndicationFeed feed = await client.RetrieveFeedAsync(feedUri);

                        if (feed.Items != null && feed.Items.Count > 0)
                        {
                            foreach (SyndicationItem item in feed.Items)
                            {
                                Item tmpItem = new Item();
                                if (item.Title != null && item.Title.Text != null)
                                {
                                    tmpItem.Title = item.Title.Text;
                                }
                                if (item.Summary != null && item.Summary.Text != null)
                                {
                                    tmpItem.Content = item.Summary.Text;
                                }
                                if (item.Links != null && item.Links.Count > 0)
                                {
                                    tmpItem.Url = item.Links[0].Uri;
                                }
                                switch (param)
                                {
                                    case "home":
                                        tmpUnread.Add(new ItemViewModel(tmpItem));
                                        break;
                                    case "fav":
                                        tmpFavourites.Add(new ItemViewModel(tmpItem));
                                        break;
                                    case "archive":
                                        tmpArchive.Add(new ItemViewModel(tmpItem));
                                        break;
                                }
                            }
                        }
                        unreadItems = tmpUnread;
                        favouriteItems = tmpFavourites;
                        archivedItems = tmpArchive;
                        
                        IsRunning = false;
                    }
                    catch (Exception e)
                    {
                        IsRunning = false;
#if DEBUG
                        throw (e);
#endif
                    }
                }
            }
        }
        
        public MainViewModel()
        {
            unreadItems = new ObservableCollection<ItemViewModel>();
            favouriteItems = new ObservableCollection<ItemViewModel>();
            archivedItems = new ObservableCollection<ItemViewModel>();

            refreshCommand = new RelayCommand(async () => await refresh(), () => IsRunning ? false : true);

            if (ApplicationSettings.GetSetting<bool>("refreshOnStartup", false))
                refreshCommand.Execute(0);
        }
    }
}