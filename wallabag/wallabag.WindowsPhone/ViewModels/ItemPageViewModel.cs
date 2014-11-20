using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToolkit.Mvvm;
using MyToolkit.Command;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;

namespace wallabag.ViewModels
{
    public class ItemPageViewModel : ViewModelBase
    {
        private ArticleViewModel _Article;
        public ArticleViewModel Article
        {
            get { return _Article; }
            set
            {
                if (value != _Article)
                {
                    _Article = value;
                    RaisePropertyChanged(() => Article);
                }
            }
        }

        public RelayCommand shareCommand { get; private set; }
        private void share()
        {
            DataTransferManager.ShowShareUI();
        }

        public ItemPageViewModel()
        {
            _Article = new ArticleViewModel();
            shareCommand = new RelayCommand(() => share());
        }
        public ItemPageViewModel(ArticleViewModel item)
        {
            Article = item;
            shareCommand = new RelayCommand(() => share());
        }
    }
}
