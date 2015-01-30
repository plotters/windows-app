using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using wallabag.Common;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace wallabag.ViewModel
{
    public class MainViewModel : viewModelBase
    {
        private ObservableCollection<ItemViewModel> _Items = new ObservableCollection<ItemViewModel>();
        public ObservableCollection<ItemViewModel> Items { get { return _Items; } }

        private async void Refresh()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetAsync(new Uri("http://wallabag-v2.jlnostr.de/api/entries"));
            if (response.IsSuccessStatusCode)
            {
                foreach (Models.Item item in JsonConvert.DeserializeObject<ObservableCollection<Models.Item>>(await response.Content.ReadAsStringAsync()))
                {
                    Items.Add(new ItemViewModel(item));
                }
            }
        }

        public MainViewModel()
        {
            Refresh();
        }
    }
}