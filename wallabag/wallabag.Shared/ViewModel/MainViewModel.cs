using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using wallabag.Common;
using wallabag.Models;
using Windows.UI.Xaml;
using Windows.Web.Syndication;

namespace wallabag.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private INavigationService navigationService;

        private bool _IsRunning = true;
        public bool IsRunning
        {
            get { return _IsRunning; }
            set
            {
                Set(() => IsRunning, ref _IsRunning, value ? false : true);
                refreshCommand.RaiseCanExecuteChanged();
            }
        }
        
        public Visibility addLinkButtonVisibility
        {
            get
            {
                if (ApplicationSettings.GetSetting<bool>("enableAddLink", false)) { return Visibility.Visible; } else { return Visibility.Collapsed; }
            }
        }

        public ObservableCollection<ItemViewModel> unreadItems { get; set; }
        public ObservableCollection<ItemViewModel> favouriteItems { get; set; }
        public ObservableCollection<ItemViewModel> archivedItems { get; set; }

        private bool everythingOkay
        {
            get
            {
                string wallabagUrl = ApplicationSettings.GetSetting<string>("wallabagUrl", "", true);
                int userId = ApplicationSettings.GetSetting<int>("userId", 1, true);
                string token = ApplicationSettings.GetSetting<string>("Token", "", true);

                return wallabagUrl != string.Empty && userId != 0 && token != string.Empty && Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile() != null;
            }
        }

        private string buildUrl(string parameter)
        {
            string wallabagUrl = ApplicationSettings.GetSetting<string>("wallabagUrl", "", true);
            int userId = ApplicationSettings.GetSetting<int>("userId", 1, true);
            string token = ApplicationSettings.GetSetting<string>("Token", "", true);

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
                        IsRunning = false;
                    }
                    catch (Exception e)
                    {
                        IsRunning = false;
                        throw e;
                    }
                }
            }
        }

        public RelayCommand openItemCommand { get; private set; }
        private void openItem(ItemViewModel item)
        {
            this.navigationService.NavigateTo("singleItem", item);
        }

        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            unreadItems = new ObservableCollection<ItemViewModel>();
            favouriteItems = new ObservableCollection<ItemViewModel>();
            archivedItems = new ObservableCollection<ItemViewModel>();

            refreshCommand = new RelayCommand(async () => await refresh(), () => IsRunning);

            if (ApplicationSettings.GetSetting<bool>("refreshOnStartup", false, true))
                refreshCommand.Execute(0);
        }
    }
}