using System;
using System.Collections.Generic;

namespace wallabag.Models
{
    public class Item
    {
        public int id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public int is_read { get; set; }
        public int is_fav { get; set; }
        public string content { get; set; }
        public int user_id { get; set; }
    }
}
