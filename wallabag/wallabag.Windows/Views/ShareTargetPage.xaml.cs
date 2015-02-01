using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using wallabag.Common;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace wallabag.Views
{
    public sealed partial class ShareTargetPage : Page
    {
        private Windows.ApplicationModel.DataTransfer.ShareTarget.ShareOperation _shareOperation;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public ObservableDictionary DefaultViewModel { get { return this.defaultViewModel; } }

        public ShareTargetPage()
        {
            this.InitializeComponent();
        }

        public async void Activate(ShareTargetActivatedEventArgs e)
        {
            this._shareOperation = e.ShareOperation;

            var shareProperties = this._shareOperation.Data.Properties;
            var thumbnailImage = new BitmapImage();
            this.DefaultViewModel["Title"] = shareProperties.Title;
            this.DefaultViewModel["Description"] = shareProperties.Description;
            this.DefaultViewModel["Url"] = await this._shareOperation.Data.GetWebLinkAsync();
            this.DefaultViewModel["Sharing"] = false;
            Window.Current.Content = this;
            Window.Current.Activate();
        }

        private async void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            this.DefaultViewModel["Sharing"] = true;
            this._shareOperation.ReportStarted();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.UserAgent.Add(new HttpProductInfoHeaderValue("wallabag for WinRT"));

            var content = new HttpStringContent(JsonConvert.SerializeObject(new Dictionary<string, object>() {
                 {"url", this.DefaultViewModel["Url"]},
                 {"title", this.DefaultViewModel["Title"]}
                }), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
            var response = await client.PostAsync(new Uri("http://wallabag-v2.jlnostr.de/api/entries"), content);
            if (response.IsSuccessStatusCode)
            {
                this._shareOperation.ReportCompleted();
            }
        }
    }
}
