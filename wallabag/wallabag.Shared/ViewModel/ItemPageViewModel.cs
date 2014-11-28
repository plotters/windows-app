using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Windows.ApplicationModel.DataTransfer;

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

        public string Title
        {
            get { return Item.Title; }
        }

        public RelayCommand shareCommand { get; private set; }
        private void share()
        {
            DataTransferManager.ShowShareUI();
        }

        public ItemPageViewModel()
        {
            shareCommand = new RelayCommand(() => share());
        }

    }
}
