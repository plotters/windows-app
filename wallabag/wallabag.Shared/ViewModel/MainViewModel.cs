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
        }
    }
}