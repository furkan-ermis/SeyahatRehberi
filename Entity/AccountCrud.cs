using SeyahatRehberi.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SeyahatRehberi.Entity
{
    public class AccountCrud
    {
        UserCrud _userCrud = new UserCrud();
        private User User;
        private DataContext _context = new DataContext();
        public AccountCrud()
        {
            User = _userCrud.GetCurrentUser();
        }

        public List<BlogPost> GetBlogs()
        {
            var userBlogs = _context.BlogPost
                .Where(b => b.UserBlogPostId == User.Id && !b.IsDeleted)
                .Include(b => b.City)
                .Include(b => b.UserBlogPost)
                .ToList();
            
            return userBlogs;
           

        }
        
    }
}
