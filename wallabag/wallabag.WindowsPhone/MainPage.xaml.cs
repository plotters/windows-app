using GalaSoft.MvvmLight.Command;
using System;
using wallabag.Common;
using wallabag.ViewModel;
using wallabag.Views;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Standardseite" ist unter "http://go.microsoft.com/fwlink/?LinkID=390556" dokumentiert.

namespace wallabag
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Frames navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox l = (ListBox)sender;

            if (l.SelectedIndex > -1)
            {
                ArticleViewModel a = (ArticleViewModel)e.AddedItems[0];
                this.Frame.Navigate(typeof(ItemPage), a);
            }
        }

    }
}
