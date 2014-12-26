using wallabag.Common;
using wallabag.Views;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Controls;

namespace wallabag
{
    public sealed partial class MainPage : basicPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            SettingsPane.GetForCurrentView().CommandsRequested += MainPage_CommandsRequested;
        }

        protected override void SaveState(SaveStateEventArgs e)
        {
            e.PageState.Add("unreadItemsActive", unreadItemsMenuButton.IsChecked);
            e.PageState.Add("favouriteItemsActive", favouriteItemsMenuButton.IsChecked);
            e.PageState.Add("archivedItemsActive", archivedItemsMenuButton.IsChecked);
            base.SaveState(e);
        }

        protected override void LoadState(LoadStateEventArgs e)
        {
            if (e.PageState != null)
            {
                if (e.PageState.ContainsKey("unreadItemsActive"))
                    unreadItemsMenuButton.IsChecked = (bool)e.PageState["unreadItemsActive"];
                if (e.PageState.ContainsKey("favouriteItemsActive"))
                    favouriteItemsMenuButton.IsChecked = (bool)e.PageState["favouriteItemsActive"];
                if (e.PageState.ContainsKey("archivedItemsActive"))
                    archivedItemsMenuButton.IsChecked = (bool)e.PageState["archivedItemsActive"];
            }
            base.LoadState(e);
        }

        void MainPage_CommandsRequested(Windows.UI.ApplicationSettings.SettingsPane sender, Windows.UI.ApplicationSettings.SettingsPaneCommandsRequestedEventArgs args)
        {
            SettingsCommand generalSettings = new SettingsCommand("generalSettings", "General", (handler) =>
            {
                generalSettingsFlyout settingsFlyout = new generalSettingsFlyout();
                settingsFlyout.Show();
            });

            SettingsCommand readingSettings = new SettingsCommand("readingSettings", "Reading", (handler) =>
            {
                readingSettingsFlyout readingFlyout = new readingSettingsFlyout();
                readingFlyout.Show();
            });

            if (args.Request.ApplicationCommands.Count == 0)
            {
                args.Request.ApplicationCommands.Add(generalSettings);
                args.Request.ApplicationCommands.Add(readingSettings);
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(ItemPage), e.ClickedItem);
        }
    }
}
