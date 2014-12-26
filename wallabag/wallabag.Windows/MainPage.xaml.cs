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
