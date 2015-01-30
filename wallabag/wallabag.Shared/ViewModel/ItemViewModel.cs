using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

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
            get { return Model.Title; }
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

        private ObservableCollection<string> _Tags;
        public ObservableCollection<string> Tags
        {
            get { return _Tags; }
            set { Set(() => Tags, ref _Tags, value); }
        }

        private async Task<bool> Fetch()
        {
            using (client)
            {
                var response = await client.GetAsync(new Uri("http://wallabag-v2.jlnostr.de/api/entries/" + Id));
                if (response.IsSuccessStatusCode)
                {
                    _Model = JsonConvert.DeserializeObject<Models.Item>(await response.Content.ReadAsStringAsync());
                    return true;
                }
                else return false;
            }
        }
        private async Task<bool> Delete()
        {
            using (client)
            {
                var response = await client.DeleteAsync(new Uri("http://wallabag-v2.jlnostr.de/api/entries/" + Id));
                return response.IsSuccessStatusCode;
            }
        }
        private async Task Update()
        {
            // TODO: PATCH /api/entries/{entry}
        }

        private async Task LoadTags() { }
        private async Task AddTag(string tag) { }
        private async Task RemoveTag(string tag) { }

        public ItemViewModel() { _Model = new Models.Item(); }
        public ItemViewModel(Models.Item Model) { _Model = Model; }

        public ItemViewModel()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
