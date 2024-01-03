using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeyahatRehberi.Models
{
    public class BlogLike
    {
        public int Id { get; set; }
        public int UserBlogLikeId { get; set; }
        public User UserBlogLike { get; set; }
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }

    }
}