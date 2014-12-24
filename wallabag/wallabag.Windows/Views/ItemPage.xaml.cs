using System;
using wallabag.Common;
using wallabag.ViewModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace wallabag.Views
{
    public sealed partial class ItemPage : basicPage
    {
        public ItemPage()
        {
            this.InitializeComponent();
            backButton.Command = this.navigationHelper.GoBackCommand;

            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += dataTransferManager_DataRequested;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
                this.DataContext = new ItemPageViewModel(e.Parameter as ItemViewModel);

            base.OnNavigatedTo(e);
        }

        void dataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            ItemViewModel item = (this.DataContext as ItemPageViewModel).Item as ItemViewModel;
            request.Data.Properties.Title = item.Title;
            request.Data.SetWebLink(item.Url);
            request.Data.SetHtmlFormat(item.Content);
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
