using System;
using wallabag.Common;
using Windows.UI.Xaml.Controls;

namespace wallabag.Views
{
    public sealed partial class MainPage : basicPage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(ItemPage));
        }

    }
}
