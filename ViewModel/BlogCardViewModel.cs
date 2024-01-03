using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeyahatRehberi.ViewModel
{
    public class BlogCardViewModel
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author{ get; set; }
        public string Image { get; set; }
        public string City { get; set; }
        public int Comments { get; set; }
        public int Likes { get; set; }

    }
}