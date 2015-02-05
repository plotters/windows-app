using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using wallabag.Common;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace wallabag.ViewModel
{
    public class MainViewModel : viewModelBase
    {
        private INavigationService navigationService;
        private HttpClient client;

        private ObservableCollection<ItemViewModel> _Items = new ObservableCollection<ItemViewModel>();
        private ObservableCollection<ItemViewModel> _unreadItems = new ObservableCollection<ItemViewModel>();
        private ObservableCollection<ItemViewModel> _favouriteItems = new ObservableCollection<ItemViewModel>();
        private ObservableCollection<ItemViewModel> _archivedItems = new ObservableCollection<ItemViewModel>();
        private ObservableCollection<ItemViewModel> _deletedItems = new ObservableCollection<ItemViewModel>();
        private ObservableCollection<string> _Tags = new ObservableCollection<string>();

        public ObservableCollection<ItemViewModel> Items
        {
            get { return _Items; }
            set { Set(() => Items, ref _Items, value); }
        }
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
        public ObservableCollection<ItemViewModel> deletedItems
        {
            get { return _deletedItems; }
            set { Set(() => deletedItems, ref _deletedItems, value); }
        }

        public ObservableCollection<string> Tags
        {
            get { return _Tags; }
            set { Set(() => Tags, ref _Tags, value); }
        }

        public RelayCommand RefreshCommand { get; private set; }
        private async Task Refresh()
        {
            await LoadAllItems();
            await LoadUnreadItems();
            await LoadFavouriteItems();
            await LoadArchivedItems();
            await LoadDeletedItems();
            await LoadTags();
        }

        private async Task LoadAllItems()
        {
            var response = await client.GetAsync(new Uri("http://v2.wallabag.org/api/entries.json"));
            if (response.IsSuccessStatusCode)
            {
                Items.Clear();
                foreach (Models.Item item in JsonConvert.DeserializeObject<ObservableCollection<Models.Item>>(await response.Content.ReadAsStringAsync()))
                {
                    Items.Add(new ItemViewModel(item));
                }
            }
        } // for search only, I think
        private async Task LoadUnreadItems()
        {
            var response = await client.GetAsync(new Uri("http://v2.wallabag.org/api/entries.json?archive=false"));
            if (response.IsSuccessStatusCode)
            {
                unreadItems.Clear();
                foreach (Models.Item item in JsonConvert.DeserializeObject<ObservableCollection<Models.Item>>(await response.Content.ReadAsStringAsync()))
                {
                    unreadItems.Add(new ItemViewModel(item));
                }
            }
        }
        private async Task LoadFavouriteItems()
        {
            var response = await client.GetAsync(new Uri("http://v2.wallabag.org/api/entries.json?star=true"));
            if (response.IsSuccessStatusCode)
            {
                favouriteItems.Clear();
                foreach (Models.Item item in JsonConvert.DeserializeObject<ObservableCollection<Models.Item>>(await response.Content.ReadAsStringAsync()))
                {
                    favouriteItems.Add(new ItemViewModel(item));
                }
            }
        }
        private async Task LoadArchivedItems()
        {
            var response = await client.GetAsync(new Uri("http://v2.wallabag.org/api/entries.json?archive=false"));
            if (response.IsSuccessStatusCode)
            {
                archivedItems.Clear();
                foreach (Models.Item item in JsonConvert.DeserializeObject<ObservableCollection<Models.Item>>(await response.Content.ReadAsStringAsync()))
                {
                    archivedItems.Add(new ItemViewModel(item));
                }
            }
        }
        private async Task LoadDeletedItems()
        {
            var response = await client.GetAsync(new Uri("http://v2.wallabag.org/api/entries.json?delete=true"));
            if (response.IsSuccessStatusCode)
            {
                deletedItems.Clear();
                foreach (Models.Item item in JsonConvert.DeserializeObject<ObservableCollection<Models.Item>>(await response.Content.ReadAsStringAsync()))
                {
                    deletedItems.Add(new ItemViewModel(item));
                }
            }
        }
        private async Task LoadTags()
        {
            var response = await client.GetAsync(new Uri("http://v2.wallabag.org/api/tags.json"));
            if (response.IsSuccessStatusCode)
            {
                Tags.Clear();
                foreach (string tag in JsonConvert.DeserializeObject<ObservableCollection<string>>(await response.Content.ReadAsStringAsync()))
                {
                    Tags.Add(tag);
                }
            }
        }

        public RelayCommand<string> AddLinkCommand { get; private set; }
        private async Task AddLink(string Link)
        {
#if WINDOWS_APP
            var content = new HttpStringContent(JsonConvert.SerializeObject(new Dictionary<string, object>() {
                 {"url", Link}
                }), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            var response = await client.PostAsync(new Uri("http://v2.wallabag.org/api/entries.json"), content);
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<Models.Item>(await response.Content.ReadAsStringAsync());
                Items.Add(new ItemViewModel(result));
            }
#else
            await new Views.AddLinkDialog().ShowAsync();
#endif
        }

        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("wallabag for WinRT"));

            RefreshCommand = new RelayCommand(async () => await Refresh());
            AddLinkCommand = new RelayCommand<string>(async t => await AddLink(t));

            if (IsInDesignMode)
            {
                for (int i = 0; i < 100; i++)
                {
                    var model = new Models.Item();
                    model.Title = "Article No. " + i;
                    model.Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec a diam lectus. Sed sit amet ipsum mauris. Maecenas congue ligula ac quam viverra nec consectetur ante hendrerit. Donec et mollis dolor. Praesent et diam eget libero egestas mattis sit amet vitae augue. Nam tincidunt congue enim, ut porta lorem lacinia consectetur. Donec ut libero sed arcu vehicula ultricies a non tortor. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean ut gravida lorem. Ut turpis felis, pulvinar a semper sed, adipiscing id dolor. Pellentesque auctor nisi id magna consequat sagittis. Curabitur dapibus enim sit amet elit pharetra tincidunt feugiat nisl imperdiet. Ut convallis libero in urna ultrices accumsan. Donec sed odio eros. Donec viverra mi quis quam pulvinar at malesuada arcu rhoncus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. In rutrum accumsan ultricies. Mauris vitae nisi at sem facilisis semper ac in est.";
                    Items.Add(new ItemViewModel(model));
                }
            }
        }
    }
}