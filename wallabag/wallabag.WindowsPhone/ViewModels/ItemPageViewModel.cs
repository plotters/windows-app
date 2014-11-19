using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyToolkit.Mvvm;

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

        public ItemPageViewModel() { _Article = new ArticleViewModel(); }
        public ItemPageViewModel(ArticleViewModel item) { Article = item; }
    }
}
