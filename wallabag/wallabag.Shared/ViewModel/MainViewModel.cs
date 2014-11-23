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
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
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
                addLinkCommand.RaiseCanExecuteChanged();
                openSettingsCommand.RaiseCanExecuteChanged();
            }
        }
        
        public Visibility addLinkButtonVisibility
        {
            get
            {
                if (ApplicationSettings.GetSetting<bool>("enableAddLink", false)) { return Visibility.Visible; } else { return Visibility.Collapsed; }
            }
        }

#if WINDOWS
        public ObservableCollection<ArticleViewModel> currentItems { get; set; }                       
#endif
        public ObservableCollection<ArticleViewModel> unreadItems { get; set; }
        public ObservableCollection<ArticleViewModel> favouriteItems { get; set; }
        public ObservableCollection<ArticleViewModel> archivedItems { get; set; }

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

        public RelayCommand addLinkCommand { get; private set; }
        private void addLink()
        {
#if WINDOWS_PHONE
            this.navigationService.NavigateTo("addLinkWP");
#else
            this.navigationService.NavigateTo("addLink");
#endif
        }

        public RelayCommand openSettingsCommand { get; private set; }
        private void openSettings()
        {
#if WINDOWS_PHONE
            this.navigationService.NavigateTo("settingsWP");
#else
            this.navigationService.NavigateTo("settings");
#endif
        }

        #region Windows Phone related
        private int _selectedPivotItem;
        public int selectedPivotItem
        {
            get { return _selectedPivotItem; }
            set { Set(() => selectedPivotItem, ref _selectedPivotItem, value); }
        }
        #endregion

        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;

            unreadItems = new ObservableCollection<ArticleViewModel>();
            favouriteItems = new ObservableCollection<ArticleViewModel>();
            archivedItems = new ObservableCollection<ArticleViewModel>();
#if WINDOWS
            currentItems = new ObservableCollection<ArticleViewModel>();
#endif
            refreshCommand = new RelayCommand(async () => await refresh(), () => IsRunning);
            addLinkCommand = new RelayCommand(() => addLink(), () => IsRunning);
            openSettingsCommand = new RelayCommand(() => openSettings(), () => IsRunning);

            if (IsInDesignMode)
            {
                for (int i = 0; i < 10; i++)
                {
                    Article a = new Article();
                    a.Content = "<p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec a diam lectus. Sed sit amet ipsum mauris. Maecenas congue ligula ac quam viverra nec consectetur ante hendrerit. Donec et mollis dolor. Praesent et diam eget libero egestas mattis sit amet vitae augue. Nam tincidunt congue enim, ut porta lorem lacinia consectetur. Donec ut libero sed arcu vehicula ultricies a non tortor. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean ut gravida lorem. Ut turpis felis, pulvinar a semper sed, adipiscing id dolor. Pellentesque auctor nisi id magna consequat sagittis. Curabitur dapibus enim sit amet elit pharetra tincidunt feugiat nisl imperdiet. Ut convallis libero in urna ultrices accumsan. Donec sed odio eros. Donec viverra mi quis quam pulvinar at malesuada arcu rhoncus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. In rutrum accumsan ultricies. Mauris vitae nisi at sem facilisis semper ac in est.</p>";
                    a.Title = "Article No. " + i.ToString();
                    a.Url = new Uri("http://wallabag.org/");

                    unreadItems.Add(new ArticleViewModel(a));
                    favouriteItems.Add(new ArticleViewModel(a));
                    archivedItems.Add(new ArticleViewModel(a));
                }
            }
        }
    }
}