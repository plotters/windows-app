using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace wallabag.Common
{
    class Database
    {
        public string Name { get; set; }
               
        public async Task Database(string databaseName)
        {
            this.Name = databaseName;
            await ApplicationData.Current.LocalFolder.CreateFileAsync(Name + ".db");
        }
    }
}
