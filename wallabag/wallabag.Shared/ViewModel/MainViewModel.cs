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
using SQLite;

namespace wallabag.ViewModel
{
    public class MainViewModel : viewModelBase
    {        
        private ObservableCollection<ItemViewModel> _unreadItems = new ObservableCollection<ItemViewModel>();
        private ObservableCollection<ItemViewModel> _favouriteItems = new ObservableCollection<ItemViewModel>();
        private ObservableCollection<ItemViewModel> _archivedItems = new ObservableCollection<ItemViewModel>();

        public ObservableCollection<ItemViewModel> unreadItems { get { return _unreadItems; } }
        public ObservableCollection<ItemViewModel> favouriteItems { get { return _favouriteItems; } }
        public ObservableCollection<ItemViewModel> archivedItems { get { return _archivedItems; } }

        private bool everythingOkay
        {
            get
            {
                string wallabagUrl = AppSettings["wallabagUrl", string.Empty];
                int userId = AppSettings["userId", 1];
                string token = AppSettings["Token", string.Empty];

                ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
                bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;

                return wallabagUrl != string.Empty && userId != 0 && token != string.Empty && internet;
            }
        }
        private string buildUrl(string parameter)
        {
            string wallabagUrl = AppSettings["wallabagUrl", string.Empty];
            int userId = AppSettings["userId", 1];
            string token = AppSettings["Token", string.Empty];

            if (everythingOkay)
                return string.Format("{0}?feed&type={1}&user_id={2}&token={3}", wallabagUrl, parameter, userId, token);

            return string.Empty;
        }

        public RelayCommand refreshCommand { get; private set; }
        private async Task RefreshItems()
        {
            if (everythingOkay)
            {
                StatusText = Helpers.LocalizedString("UpdatingText");
                IsActive = true;

                unreadItems.Clear();
                favouriteItems.Clear();
                archivedItems.Clear();

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
                                        unreadItems.Add(new ItemViewModel(tmpItem));
                                        break;
                                    case "fav":
                                        favouriteItems.Add(new ItemViewModel(tmpItem));
                                        break;
                                    case "archive":
                                        archivedItems.Add(new ItemViewModel(tmpItem));
                                        break;
                                }
                            }
                        }
                        IsActive = false;
                    }
                    catch
                    {
                        IsActive = false;
                    }
                }
            }
        }

        private async Task SaveItemsInDatabase()
        {
            SQLiteAsyncConnection conn = new SQLiteAsyncConnection("wallabag.db");
            await conn.CreateTableAsync<Models.Item>();
            
        }
        
        public MainViewModel()
        {
            refreshCommand = new RelayCommand(async () => await RefreshItems());

            if (AppSettings["refreshOnStartup", false])
                refreshCommand.Execute(0);
        }
    }
}