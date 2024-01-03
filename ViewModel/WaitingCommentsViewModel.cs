using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeyahatRehberi.ViewModel
{
    public class WaitingCommentsViewModel
    {
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public string Username{ get; set; }
        public string BlogTitle{ get; set; }
        public string Comment{ get; set; }
        public DateTime CommentDate{ get; set; }
    }
}