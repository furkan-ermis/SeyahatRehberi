using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeyahatRehberi.ViewModel
{
    public class UserCommentViewModel
    {
        public string UserImage { get; set; }
        public string UserName { get; set; }
        public string CommentContent { get; set; }
        public DateTime CommentDate { get; set; }
    }
}