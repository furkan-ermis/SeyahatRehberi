using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeyahatRehberi.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Summary{ get; set; }
        public string Image{ get; set; }
        public string CreationDate { get; set; }
        public bool IsRaported { get; set; }
        public bool IsDeleted { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
        public int UserBlogPostId { get; set; }
        public User UserBlogPost { get; set; }
        public List<BlogComment> BlogCommentList { get; set; }
        public List<BlogLike> BlogLikeList { get; set; }
    }
}