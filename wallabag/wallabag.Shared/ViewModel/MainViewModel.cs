using System.Collections.ObjectModel;
using System.Threading.Tasks;
using wallabag.Common;
using Windows.Web.Http;

namespace wallabag.ViewModel
{
    public class MainViewModel : viewModelBase
    {
        private ObservableCollection<ItemViewModel> _Items = new ObservableCollection<ItemViewModel>();
        public ObservableCollection<ItemViewModel> Items { get { return _Items; } }

        private void Refresh()
        {
            HttpClient client = new HttpClient();
            var test = client.GetStringAsync(new System.Uri("http://v2.wallabag.org/api/entries"));
            var test2 = test.Status;
        }

        public MainViewModel()
        {
            Refresh();
        }
    }
}