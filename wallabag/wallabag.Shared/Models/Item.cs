﻿using System;
using System.Collections.Generic;

namespace wallabag.Models
{
    public class Item
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Uri Url { get; set; }

        public bool IsRead { get; set; }
        public bool IsFavourite { get; set; }

        List<Tag> Tags = new List<Tag>();
    }
}
