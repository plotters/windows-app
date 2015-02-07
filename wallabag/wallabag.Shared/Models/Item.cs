using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace wallabag.Models
{
    public class Item
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("is_read")]
        public int IsRead { get; set; }
        [JsonProperty("is_fav")]
        public int IsFavourite { get; set; }
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("user_id")]
        public int UserId { get; set; }
    }
}
