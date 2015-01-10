using System;
using SQLite;

namespace wallabag.Models
{
    [Table("Items")]
    public class Item
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Uri Url { get; set; }

        public bool IsRead { get; set; }
        public bool IsFavourite { get; set; }
    }
}
