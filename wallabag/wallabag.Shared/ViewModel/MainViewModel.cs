using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using wallabag.Common;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using GalaSoft.MvvmLight.Command;
using System.Threading.Tasks;
using System.Linq;

namespace wallabag.ViewModel
{
    public class MainViewModel : viewModelBase
    {
        private HttpClient client;

        private ObservableCollection<ItemViewModel> _Items = new ObservableCollection<ItemViewModel>();
        public ObservableCollection<ItemViewModel> Items { get { return _Items; } }

        private ObservableCollection<string> _Tags;
        public ObservableCollection<string> Tags
        {
            get { return _Tags; }
            set { Set(() => Tags, ref _Tags, value); }
        }

        public RelayCommand RefreshCommand { get; private set; }
        private async Task Refresh()
        {
            using (client)
            {
                var response = await client.GetAsync(new Uri("http://wallabag-v2.jlnostr.de/api/entries"));
                if (response.IsSuccessStatusCode)
                {
                    foreach (Models.Item item in JsonConvert.DeserializeObject<ObservableCollection<Models.Item>>(await response.Content.ReadAsStringAsync()))
                    {
                        Items.Add(new ItemViewModel(item));
                    }
                }

                // TODO: When the Tag API is working, enable this.
                //var tagResponse = await client.GetAsync(new Uri("http://wallabag-v2.jlnostr.de/api/tags"));
                //if (tagResponse.IsSuccessStatusCode)
                //{
                //    foreach (string tag in JsonConvert.DeserializeObject<ObservableCollection<string>>(await response.Content.ReadAsStringAsync()))
                //    {
                //        Tags.Add(tag);
                //    }
                //}
            }
        }

        public MainViewModel()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
            RefreshCommand = new RelayCommand(async () => await Refresh());

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