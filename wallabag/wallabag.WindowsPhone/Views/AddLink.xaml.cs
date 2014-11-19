using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using MyToolkit.Storage;
using System.Diagnostics;

// Die Elementvorlage "Inhaltsdialog" ist unter "http://go.microsoft.com/fwlink/?LinkID=390556" dokumentiert.

namespace wallabag.Views
{
    public sealed partial class AddLink : ContentDialog
    {
        public AddLink()
        {
            this.InitializeComponent();
        }
        
        private string finalUrl()
        {
            string wallabagUrl = ApplicationSettings.GetSetting<string>("wallabagUrl", "", true);
            string encodedUrl = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(link.Text));
            Debug.WriteLine("finalUrl: " + string.Format("{0}?action=add&url={1}", wallabagUrl, encodedUrl));
            return string.Format("{0}?action=add&url={1}", wallabagUrl, encodedUrl);
        }
        
        private void ContentDialog_PrimaryButtonClick(object sender, RoutedEventArgs e)
        {
            webView.Visibility = Windows.UI.Xaml.Visibility.Visible;
            webView.Navigate(new Uri(finalUrl()));
        }
    }
}
