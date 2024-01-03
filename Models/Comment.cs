using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SeyahatRehberi.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CommentDate { get; set; }
        public bool IsStatus { get; set; }
        public int UserCommentId { get; set; }
        public User UserComment { get; set; }
        public List<BlogComment> BlogCommentList { get; set; }

    }
}