using Windows.UI.Xaml.Controls;
using wallabag.Common;
using wallabag.Views;

namespace wallabag
{
    public sealed partial class MainPage : basicPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            Windows.UI.ApplicationSettings.SettingsPane.GetForCurrentView().CommandsRequested += MainPage_CommandsRequested;
        }

        void MainPage_CommandsRequested(Windows.UI.ApplicationSettings.SettingsPane sender, Windows.UI.ApplicationSettings.SettingsPaneCommandsRequestedEventArgs args)
        {
            Windows.UI.ApplicationSettings.SettingsCommand generalSettings =
                new Windows.UI.ApplicationSettings.SettingsCommand("generalSettings", "General", (handler) =>
            {
                generalSettingsFlyout settingsFlyout = new generalSettingsFlyout();
                settingsFlyout.Show();
            });

            Windows.UI.ApplicationSettings.SettingsCommand readingSettings =
                new Windows.UI.ApplicationSettings.SettingsCommand("readingSettings", "Reading", (handler) =>
            {
                readingSettingsFlyout readingFlyout = new readingSettingsFlyout();
                readingFlyout.Show();
            });

            args.Request.ApplicationCommands.Add(generalSettings);
            args.Request.ApplicationCommands.Add(readingSettings);
        }
    }
}
