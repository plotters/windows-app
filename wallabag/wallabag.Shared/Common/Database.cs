using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace wallabag.Common
{
    public class Database
    {
        private static Database _instance;
        private Database() { }
        public static Database Instance { get { return _instance ?? (_instance = Initialize()); } }

        private static Database Initialize()
        {
            throw new System.NotImplementedException();
        }
        public ObservableCollection<Models.Item> Items { get; set; }
    }
}
