using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeyahatRehberi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ProfileImage { get; set; }
        public string CreationDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete{ get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public List<BlogPost> BlogPostList { get; set; }
        public List<Comment> CommentList { get; set; }
        public List<BlogLike> BlogLikeList { get; set; }
    }
}