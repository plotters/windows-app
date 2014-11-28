using System;
using wallabag.Common;
using wallabag.ViewModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Command;

// Die Elementvorlage "Standardseite" ist unter "http://go.microsoft.com/fwlink/?LinkID=390556" dokumentiert.

namespace wallabag.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Frames navigiert werden kann.
    /// </summary>
    public sealed partial class ItemPage : Page
    {
        public ItemPage()
        {
            this.InitializeComponent();
        }

        void dataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            //request.Data.Properties.Title = this.defaultViewModel["Title"].ToString();
            //request.Data.SetWebLink(((ArticleViewModel)this.defaultViewModel["Item"]).Url);
        }

        private async void webView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            // Opens links in the Internet Explorer and not in the webView.
            if (args.Uri != null && args.Uri.AbsoluteUri.StartsWith("http"))
            {
                args.Cancel = true;
                await Launcher.LaunchUriAsync(new Uri(args.Uri.AbsoluteUri));
            }
        }

        

    }
}
