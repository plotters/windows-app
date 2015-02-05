using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace wallabag.ViewModel
{
    public class AddLinkDialogViewModel : Common.viewModelBase
    {
        private string _Url;
        public string Url
        {
            get { return _Url; }
            set { Set(() => Url, ref _Url, value); }
        }

        private string _Tags;
        public string Tags
        {
            get { return _Tags; }
            set { Set(() => Tags, ref _Tags, value); }
        }

        public RelayCommand AddLinkCommand { get; private set; }
        private async Task AddLink()
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new HttpStringContent(JsonConvert.SerializeObject(new Dictionary<string, object>() {
                 {"url", Url},
                 {"tags", Tags}
                }), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
                var response = await client.PostAsync(new Uri("http://v2.wallabag.org/api/entries.json"), content);
                if (response.IsSuccessStatusCode)
                {
                    // TODO: Handle this.
                }
            }
        }

        public AddLinkDialogViewModel()
        {
            AddLinkCommand = new RelayCommand(async () => await AddLink());
        }
    }
}
