using System;
using System.Collections.Generic;
using System.Text;

namespace wallabag.Models
{
    public class Article
    {
        public string Title { get; set; }
        public bool Favourite { get; set; }
        public bool Read { get; set; }
        public string Content { get; set; }
        public Uri Url { get; set; }
        public DateTime PublishedDate { get; set; }       
    }
}
