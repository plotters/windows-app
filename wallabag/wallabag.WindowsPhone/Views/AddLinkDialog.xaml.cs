using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using wallabag.ViewModel;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;

namespace wallabag.Views
{
    public sealed partial class AddLinkDialog : ContentDialog
    {
        public AddLinkDialog()
        {
            this.InitializeComponent();
        }
    }
}
