using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HackerNewsAPI.Models
{
    public class Story
    {
        public string By { get; set; }
        public int Descendants { get; set; }
        public long Id { get; set; }
        public List<long> Kids { get; set; }
        public long Score { get; set; }
        public string Time { get; set; }
        public string Title { get; set; }
        public string StoryType { get; set; }
        public string URL { get; set; }
    }
    
}