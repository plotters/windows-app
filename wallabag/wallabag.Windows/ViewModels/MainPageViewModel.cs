using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using MyToolkit.Command;
using MyToolkit.Mvvm;
using MyToolkit.Storage;
using wallabag.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Syndication;

namespace wallabag.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public Visibility AddLinkButtonVisibility
        {
            get 
            {
                if (ApplicationSettings.GetSetting<bool>("enableAddLink", false)) { return Visibility.Visible; } else { return Visibility.Collapsed; }    
            }
        }
                                
        public ObservableCollection<ArticleViewModel> unreadItems { get; set; }
        public ObservableCollection<ArticleViewModel> favouriteItems { get; set; }
        public ObservableCollection<ArticleViewModel> archivedItems { get; set; }

        private string buildUrl(string parameter)
        {
            string wallabagUrl = ApplicationSettings.GetSetting<string>("wallabagUrl", "", true);
            int userId = ApplicationSettings.GetSetting<int>("userId", 1, true);
            string token = ApplicationSettings.GetSetting<string>("Token", "", true);

            if (wallabagUrl != string.Empty || userId != 0 || token != string.Empty)
            {
                Debug.WriteLine("buildUrl: " + string.Format("{0}?feed&type={1}&user_id={2}&token={3}", wallabagUrl, parameter, userId, token));
                return string.Format("{0}?feed&type={1}&user_id={2}&token={3}", wallabagUrl, parameter, userId, token);
            }
            else
            {
                Debug.WriteLine("buildUrl returns an empty string!");
                return string.Empty;
            }
        }

        public AsyncRelayCommand refreshCommand { get; private set; }
        private async Task refresh()
        {
            if (Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile() != null)
            {
                IsLoading = true;

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
                                Article article = new Article();
                                if (item.Title != null && item.Title.Text != null)
                                {
                                    article.Title = item.Title.Text;
                                }
                                if (item.PublishedDate != null)
                                {
                                    article.PublishedDate = item.PublishedDate.DateTime;
                                }
                                if (item.Summary != null && item.Summary.Text != null)
                                {
                                    article.Content = item.Summary.Text;
                                }
                                if (item.Links != null && item.Links.Count > 0)
                                {
                                    article.Url = item.Links[0].Uri;
                                }
                                switch (param)
                                {
                                    case "home":
                                        unreadItems.Add(new ArticleViewModel(article));
                                        break;
                                    case "fav":
                                        favouriteItems.Add(new ArticleViewModel(article));
                                        break;
                                    case "archive":
                                        archivedItems.Add(new ArticleViewModel(article));
                                        break;
                                }
                            }
                        }
                        IsLoading = false;
                    }
                    catch (Exception e)
                    {
                        IsLoading = false;
                        throw e;
                    }
                }
            }
        }

        public AsyncRelayCommand addLinkCommand { get; private set; }
        private async Task addLink()
        {
            //if (Windows.Networking.Connectivity.NetworkInformation.GetInternetConnectionProfile() != null)
            //    await new wallabag.Views.AddLink().ShowAsync();
        }

        public RelayCommand openSettingsCommand { get; private set; }
        private void openSettings()
        {
            //((Frame)Window.Current.Content).Navigate(typeof(wallabag.Views.SettingsPage));
        }
        
        public MainPageViewModel()
        {
            unreadItems = new ObservableCollection<ArticleViewModel>();
            favouriteItems = new ObservableCollection<ArticleViewModel>();
            archivedItems = new ObservableCollection<ArticleViewModel>();
            
            refreshCommand = new AsyncRelayCommand(() => refresh());
            addLinkCommand = new AsyncRelayCommand(() => addLink());
            openSettingsCommand = new RelayCommand(() => openSettings());

        }

    }
}
