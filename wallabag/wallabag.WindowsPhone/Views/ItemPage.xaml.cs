using System;
using wallabag.Common;
using wallabag.ViewModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Standardseite" ist unter "http://go.microsoft.com/fwlink/?LinkID=390556" dokumentiert.

namespace wallabag.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Frames navigiert werden kann.
    /// </summary>
    public sealed partial class ItemPage : basicPage
    {
        public ItemPage()
        {
            this.InitializeComponent();

            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += dataTransferManager_DataRequested;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
                this.DataContext = new ItemPageViewModel(e.Parameter as ItemViewModel);
            
            base.OnNavigatedTo(e);

            var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            await statusBar.HideAsync();
        }

        void dataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.Properties.Title = ((this.DataContext as ItemPageViewModel).Item as ItemViewModel).Title;
            request.Data.SetWebLink(((this.DataContext as ItemPageViewModel).Item as ItemViewModel).Url);
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
