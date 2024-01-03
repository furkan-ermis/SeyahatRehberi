using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeyahatRehberi.Models
{
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public string Image { get; set; }
        public List<BlogPost> blogPostList { get; set; }
    }
}