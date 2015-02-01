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

// Die Elementvorlage "Freigabezielvertrag" ist unter http://go.microsoft.com/fwlink/?LinkId=234241 dokumentiert.

namespace wallabag.Views
{
    /// <summary>
    /// Über diese Seite können andere Anwendungen Inhalte durch diese Anwendung freigeben.
    /// </summary>
    public sealed partial class ShareTargetPage : Page
    {
        /// <summary>
        /// Stellt einen Kanal zum Kommunizieren mit Windows über den Freigabevorgang bereit.
        /// </summary>
        private Windows.ApplicationModel.DataTransfer.ShareTarget.ShareOperation _shareOperation;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// Dies kann in ein stark typisiertes Anzeigemodell geändert werden.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public ShareTargetPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Wird aufgerufen, wenn eine andere Anwendung Inhalte durch diese Anwendung freigeben möchte.
        /// </summary>
        /// <param name="e">Aktivierungsdaten zum Koordinieren des Prozesses mit Windows.</param>
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

        /// <summary>
        /// Wird aufgerufen, wenn der Benutzer auf die Schaltfläche "Gemeinsam verwenden" klickt.
        /// </summary>
        /// <param name="sender">Instanz der Schaltfläche, die zum Initiieren der Freigabe verwendet wird.</param>
        /// <param name="e">Ereignisdaten, die beschreiben, wie auf die Schaltfläche geklickt wurde.</param>
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
