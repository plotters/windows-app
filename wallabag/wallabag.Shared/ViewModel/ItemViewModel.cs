using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using wallabag.Common;

namespace wallabag.ViewModel
{
    public class ItemViewModel : Common.viewModelBase
    {
        private HttpClient client;

        private Models.Item _Model;
        public Models.Item Model
        {
            get { return _Model; }
        }

        public int Id
        {
            get { return Model.Id; }
            set
            {
                if (Model.Id != value)
                {
                    Model.Id = value;
                    RaisePropertyChanged("Id");
                }
            }
        }
        public string Title
        {
            get
            {
                // RegExp to remove multiple whitespaces (including newline etc.) in title.
                Regex r = new Regex("\\s+");
                return r.Replace(Model.Title, " ");
            }
            set
            {
                if (Model.Title != value)
                {
                    Model.Title = value;
                    RaisePropertyChanged("Title");
                }
            }
        }
        public Uri Url
        {
            get { return new Uri(Model.Url); }
            set
            {
                if (Model.Url != value.ToString() && value != null)
                {
                    Model.Url = value.ToString();
                    RaisePropertyChanged("Url");
                }
            }
        }
        public bool IsRead
        {
            get { return Model.IsRead == 1; }
            set
            {
                if ((Model.IsRead == 1) != value)
                {
                    int tmp = 0; if (value) tmp = 1;
                    Model.IsRead = tmp;
                    RaisePropertyChanged("IsRead");
                }
            }
        }
        public bool IsFavourite
        {
            get { return Model.IsFavourite == 1; }
            set
            {
                if ((Model.IsFavourite == 1) != value)
                {
                    int tmp = 0; if (value) tmp = 1;
                    Model.IsFavourite = tmp;
                    RaisePropertyChanged("IsFavourite");
                }
            }
        }
        public string Content
        {
            get { return Model.Content; }
            set
            {
                if (Model.Content != value && value != null)
                {
                    Model.Content = value;
                    RaisePropertyChanged("Content");
                }
            }
        }
        public string ContentWithCSS
        {
            get
            {
                //TODO: Add user-specific CSS styles.
                return "<html><head><link rel=\"stylesheet\" href=\"ms-appx-web:///Assets/css/wallabag.css\" type=\"text/css\" media=\"screen\" /></head>" + Model.Content + "</html>";
            }
        }
        public int UserId
        {
            get { return Model.UserId; }
            set
            {
                if (Model.UserId != value)
                {
                    Model.UserId = value;
                    RaisePropertyChanged("UserId");
                }
            }
        }

        private ObservableCollection<string> _Tags = new ObservableCollection<string>();
        public ObservableCollection<string> Tags
        {
            get { return _Tags; }
            set { Set(() => Tags, ref _Tags, value); }
        }

        private async Task<bool> Fetch()
        {
            var response = await client.GetAsync(new Uri(string.Format("http://wallabag-v2.jlnostr.de/api/entries/{0}.json", Id)));
            if (response.IsSuccessStatusCode)
            {
                _Model = JsonConvert.DeserializeObject<Models.Item>(await response.Content.ReadAsStringAsync());
                return true;
            }
            else return false;
        }
        private async Task<bool> Delete()
        {
            var response = await client.DeleteAsync(new Uri(string.Format("http://wallabag-v2.jlnostr.de/api/entries/{0}.json", Id)));
            return response.IsSuccessStatusCode;
        }
        private async Task Update()
        {
            var content = new HttpStringContent(JsonConvert.SerializeObject(new Dictionary<string, object>() {
                 {"title", Title},
                 {"tags", Tags.ToCommaSeperatedString()},
                 {"star", IsFavourite.ToInteger()},
                 {"archive", IsRead.ToInteger()},
                 //{"delete", false} //TODO
                }), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            var response = await client.PatchAsync(new Uri(string.Format("http://wallabag-v2.jlnostr.de/api/entries/{0}.json", Id)), content);
            if (response.IsSuccessStatusCode)
            {
                // TODO: Parse response.
            }
        }

        private async Task LoadTags()
        {
            var response = await client.GetAsync(new Uri(string.Format("http://wallabag-v2.jlnostr.de/api/entries/{0}/tags.json", Id)));
            // TODO: Parse response!
        }
        private async Task AddTag(string tag) { }
        private async Task RemoveTag(string tag) { }

        public ItemViewModel()
        {
            _Model = new Models.Item();
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("wallabag for WinRT"));
        }
        public ItemViewModel(Models.Item Model)
        {
            _Model = Model;
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("wallabag for WinRT"));
        }
    }
}
