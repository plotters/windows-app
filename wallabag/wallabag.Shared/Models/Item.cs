using System;
using System.Collections.Generic;
using System.Text;

namespace wallabag.Models
{
    public class Item
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Uri Url { get; set; } 
    }
}
