using System;
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

        protected override void SaveState(SaveStateEventArgs e)
        {
            e.PageState.Add("SelectedPivotItem", mainPivot.SelectedIndex);
            base.SaveState(e);
        }

        protected override void LoadState(LoadStateEventArgs e)
        {
            if (e.PageState != null)
            {
                if (e.PageState.ContainsKey("SelectedPivotItem"))
                    mainPivot.SelectedIndex = (int)e.PageState["SelectedPivotItem"];
            }
            base.LoadState(e);
        }

        private async void AppBarButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await new AddLink().ShowAsync();
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
