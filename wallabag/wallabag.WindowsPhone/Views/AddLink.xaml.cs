using System;
using wallabag.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace wallabag.Views
{
    public sealed partial class AddLink : ContentDialog
    {
        public ApplicationSettings AppSettings { get { return ApplicationSettings.Instance; } }

        public AddLink()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }
    }
}
