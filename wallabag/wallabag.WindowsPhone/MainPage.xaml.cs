using MyToolkit.Storage;
using MyToolkit.Command;
using System;
using wallabag.Common;
using wallabag.ViewModels;
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
        private NavigationHelper navigationHelper;

        private ObservableDictionary _defaultViewModel = new ObservableDictionary();
        public ObservableDictionary defaultViewModel
        {
            get { return this._defaultViewModel; }
        }
        
        public MainPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            if (ApplicationSettings.GetSetting<bool>("refreshOnStartup", false, true) && defaultViewModel.ContainsKey("refreshCommand"))
                ((AsyncRelayCommand)defaultViewModel["refreshCommand"]).TryExecute();
        }

        /// <summary>
        /// Ruft den <see cref="NavigationHelper"/> ab, der mit dieser <see cref="Page"/> verknüpft ist.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        /// <summary>
        /// Füllt die Seite mit Inhalt auf, der bei der Navigation übergeben wird.  Gespeicherte Zustände werden ebenfalls
        /// bereitgestellt, wenn eine Seite aus einer vorherigen Sitzung neu erstellt wird.
        /// </summary>
        /// <param name="sender">
        /// Die Quelle des Ereignisses, normalerweise <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Ereignisdaten, die die Navigationsparameter bereitstellen, die an
        /// <see cref="Frame.Navigate(Type, Object)"/> als diese Seite ursprünglich angefordert wurde und
        /// ein Wörterbuch des Zustands, der von dieser Seite während einer früheren
        /// beibehalten wurde.  Der Zustand ist beim ersten Aufrufen einer Seite NULL.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.PageState != null && e.PageState.ContainsKey("VM"))
            {
                _defaultViewModel["VM"] = (MainViewModel)e.PageState["VM"];

                if (e.PageState.ContainsKey("selectedPivotItem"))
                    mainPivot.SelectedIndex = Convert.ToInt32(e.PageState["selectedPivotItem"]);
            }
            else
            {
                _defaultViewModel["VM"] = new MainViewModel();
            }

            _defaultViewModel["unreadItems"] = ((MainViewModel)defaultViewModel["VM"]).unreadItems;
            _defaultViewModel["favouriteItems"] = ((MainViewModel)defaultViewModel["VM"]).favouriteItems;
            _defaultViewModel["archivedItems"] = ((MainViewModel)defaultViewModel["VM"]).archivedItems;
            _defaultViewModel["refreshCommand"] = ((MainViewModel)defaultViewModel["VM"]).refreshCommand;
            _defaultViewModel["addLinkCommand"] = ((MainViewModel)defaultViewModel["VM"]).addLinkCommand;
            _defaultViewModel["openSettingsCommand"] = ((MainViewModel)defaultViewModel["VM"]).openSettingsCommand;
            _defaultViewModel["AddLinkButtonVisibility"] = ((MainViewModel)defaultViewModel["VM"]).AddLinkButtonVisibility;
        }

        /// <summary>
        /// Behält den dieser Seite zugeordneten Zustand bei, wenn die Anwendung angehalten oder
        /// die Seite im Navigationscache verworfen wird.  Die Werte müssen den Serialisierungsanforderungen
        /// von <see cref="SuspensionManager.SessionState"/> entsprechen.
        /// </summary>
        /// <param name="sender">Die Quelle des Ereignisses, normalerweise <see cref="NavigationHelper"/></param>
        /// <param name="e">Ereignisdaten, die ein leeres Wörterbuch zum Auffüllen bereitstellen
        /// serialisierbarer Zustand.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState["VM"] = defaultViewModel["VM"];
            e.PageState["selectedPivotItem"] = mainPivot.SelectedIndex;
        }

        #region NavigationHelper-Registrierung

        /// <summary>
        /// Die in diesem Abschnitt bereitgestellten Methoden werden einfach verwendet, um
        /// damit NavigationHelper auf die Navigationsmethoden der Seite reagieren kann.
        /// <para>
        /// Platzieren Sie seitenspezifische Logik in Ereignishandlern für  
        /// <see cref="NavigationHelper.LoadState"/>
        /// und <see cref="NavigationHelper.SaveState"/>.
        /// Der Navigationsparameter ist in der LoadState-Methode verfügbar 
        /// zusätzlich zum Seitenzustand, der während einer früheren Sitzung beibehalten wurde.
        /// </para>
        /// </summary>
        /// <param name="e">Stellt Daten für Navigationsmethoden und -ereignisse bereit.
        /// Handler, bei denen die Navigationsanforderung nicht abgebrochen werden kann.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

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
