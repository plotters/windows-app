using wallabag.Common;
using wallabag.Views;
using Windows.UI.Xaml.Controls;

namespace wallabag
{
    public sealed partial class MainPage : basicPage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddLink));
        }

        private void AppBarButton_Click_1(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(ItemPage), e.ClickedItem);
        }

    }
}
