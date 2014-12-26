using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;

namespace wallabag.Common
{
    public class basicPage : Page
    {
        public NavigationHelper navigationHelper;
        private const string ViewModelPageKey = "ViewModel";

        public basicPage()
        {
            this.Loaded += basicPage_Loaded;
            this.Unloaded += basicPage_Unloaded;
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        void basicPage_Loaded(object sender, RoutedEventArgs e) { Window.Current.SizeChanged += Window_SizeChanged; }
        void basicPage_Unloaded(object sender, RoutedEventArgs e) { Window.Current.SizeChanged -= Window_SizeChanged; }

        void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            ChangedSize(e);
        }

        protected virtual void ChangedSize(WindowSizeChangedEventArgs e) { }

        void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            e.PageState.Add(ViewModelPageKey, this.DataContext);
            SaveState(e);
        }
        void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.PageState != null && e.PageState.ContainsKey(ViewModelPageKey))
            {
                this.DataContext = e.PageState[ViewModelPageKey];
            }
            LoadState(e);
        }

        protected virtual void LoadState(LoadStateEventArgs e) { }
        protected virtual void SaveState(SaveStateEventArgs e) { }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }
    }
}
