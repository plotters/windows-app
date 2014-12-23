using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Media;
using wallabag.Common;

namespace wallabag.ViewModel
{
    public class ItemPageViewModel : viewModelBase
    {
        private ItemViewModel _Item;
        public ItemViewModel Item
        {
            get { return _Item; }
            set { Set(() => Item, ref _Item, value); }
        }
        
        public RelayCommand shareCommand { get; private set; }

        [PreferredConstructor]
        public ItemPageViewModel()
        {
            shareCommand = new RelayCommand(() => DataTransferManager.ShowShareUI());
        }
        public ItemPageViewModel(ItemViewModel item)
        {
            shareCommand = new RelayCommand(() => DataTransferManager.ShowShareUI());
            Item = item;
        }

    }
}
