using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using wallabag.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace wallabag.Views
{
    public sealed partial class AddLinkDialog : ContentDialog
    {
        public AddLinkDialog()
        {
            this.InitializeComponent();
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // TODO: Replace by ViewModel.
            HttpClient client = new HttpClient();
            var content = new HttpStringContent(JsonConvert.SerializeObject(new Dictionary<string, object>() {
                 {"url", UrlTextBox.Text},
                 {"tags", TagTextBox.Text}
                }), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            var response = await client.PostAsync(new Uri("http://v2.wallabag.org/api/entries.json"), content);
            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<Models.Item>(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
