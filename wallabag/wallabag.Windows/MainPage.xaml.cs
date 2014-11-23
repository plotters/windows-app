using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using wallabag.Common;
using wallabag.ViewModels;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace wallabag
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Frames navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableDictionary _defaultViewModel = new ObservableDictionary();
        public ObservableDictionary DefaultViewModel
        {
            get { return this._defaultViewModel; }
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.PageState != null && e.PageState.ContainsKey("VM"))
            {
                DefaultViewModel["VM"] = (MainViewModel)e.PageState["VM"];

                if (e.PageState.ContainsKey("selectedPivotItem"))
                    mainPivot.SelectedIndex = Convert.ToInt32(e.PageState["selectedPivotItem"]);
            }
            else
            {
                DefaultViewModel["VM"] = new MainViewModel();
            }

            DefaultViewModel["unreadItems"] = ((MainViewModel)DefaultViewModel["VM"]).unreadItems;
            DefaultViewModel["favouriteItems"] = ((MainViewModel)DefaultViewModel["VM"]).favouriteItems;
            DefaultViewModel["archivedItems"] = ((MainViewModel)DefaultViewModel["VM"]).archivedItems;
            DefaultViewModel["refreshCommand"] = ((MainViewModel)DefaultViewModel["VM"]).refreshCommand;
            DefaultViewModel["addLinkCommand"] = ((MainViewModel)DefaultViewModel["VM"]).addLinkCommand;
            DefaultViewModel["openSettingsCommand"] = ((MainViewModel)DefaultViewModel["VM"]).openSettingsCommand;
            DefaultViewModel["AddLinkButtonVisibility"] = ((MainViewModel)DefaultViewModel["VM"]).AddLinkButtonVisibility;
            DefaultViewModel["currentItems"] = ((MainViewModel)DefaultViewModel["VM"]).unreadItems;
        }
    }
}
