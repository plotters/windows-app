using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Media;

namespace wallabag.ViewModel
{
    public class ItemPageViewModel : ViewModelBase
    {
        private ItemViewModel _Item;
        public ItemViewModel Item
        {
            get { return _Item; }
            set { Set(() => Item, ref _Item, value); }
        }

        public SolidColorBrush Background
        {
            get { return new SettingsViewModel().Background; }
        }

        public SolidColorBrush textColor
        {
            get { return new SettingsViewModel().textColor; }
        }
        
        public RelayCommand shareCommand { get; private set; }
        private void share()
        {
            DataTransferManager.ShowShareUI();
        }

        [PreferredConstructor]
        public ItemPageViewModel()
        {
            shareCommand = new RelayCommand(() => share());
        }
        public ItemPageViewModel(ItemViewModel item)
        {
            shareCommand = new RelayCommand(() => share());
            Item = item;
        }

    }
}
