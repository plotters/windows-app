using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.ApplicationModel.DataTransfer;
using GalaSoft.MvvmLight.Ioc;

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
