using SeyahatRehberi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SeyahatRehberi.Entity
{
    public class BlogCrud
    {
        DataContext _context=new DataContext();
        UserCrud _userCrud = new UserCrud();
        private User User;
        public BlogCrud()
        {
            User = _userCrud.GetCurrentUser();
        }
        public List<BlogPost> GetBlogs()
        {
            return _context.BlogPost.Include("City").Where(blog=>!blog.IsDeleted).ToList();
        }

        public BlogPost GetBlogById(int blogId)
        {
            return _context.BlogPost.Find(blogId);
        }

        public List<BlogPost> GetBlogsByLatest() {
            return _context.BlogPost.Where(blog=> !blog.IsDeleted ).OrderByDescending(blog => blog.Id).Take(4).ToList(); 
        }
        public List<BlogPost> GetBlogsTopLike()
        {
            if (_context.BlogPost!=null)
            {

            var topLikedBlogIds = _context.BlogLike
                .GroupBy(likedBlog => likedBlog.BlogPostId)
                .Select(group => new
                {
                    BlogPostId = group.Key,
                    TotalLikes = group.Count()
                })
                .OrderByDescending(newBlog => newBlog.TotalLikes)
                .Select(blogId => blogId.BlogPostId)
                .ToList();
            if (topLikedBlogIds.Count()>=0)
            {
            var topLikedBlogs = _context.BlogPost
                .Where(blog => topLikedBlogIds.Contains(blog.Id)  && !blog.IsDeleted)
                    .Take(6)
                    .ToList();
            return topLikedBlogs;
            }
            }
            return null;
        }

        public void RaportBlog(int blogId)
        {
            BlogPost selectedBlog = GetBlogById(blogId);
            selectedBlog.IsRaported = selectedBlog.IsRaported?false: true; // doğruysa değil yap değilse doğru yap
            _context.SaveChanges();
        }
        public List<BlogPost> GetReportedBlogs()
        {
            return _context.BlogPost.Where(blog=>blog.IsRaported).Include(b => b.City)
                .Include(b => b.UserBlogPost).ToList();
        }

        public List<BlogPost> GetBlogsByCities(int cityId)
        {
            return _context.BlogPost.Where(blog=> blog.CityId.Equals(cityId)  &&!blog.IsDeleted).ToList();
        }
        public int GetBlogCountByCities(int cityId)
        {
            return _context.BlogPost
                .Count(blog => blog.CityId == cityId && !blog.IsDeleted);
        }

        public void AddBlog(BlogPost blog)
        { 
            if (User.Id != -1)
            {
            blog.UserBlogPostId = User.Id;
            blog.CreationDate = DateTime.Now.ToString("dd,MMMM yyyy");
            blog.IsRaported = false;
            _context.BlogPost.Add(blog);
            _context.SaveChanges();
            }
        }
        public void DeleteBlog(int Blogid)
        {
           
           if (User.Id!=-1)
            {
            BlogPost selectedBlog=GetBlogById(Blogid);
            selectedBlog.IsDeleted = true;
            List<BlogLike> RemoveLikes= _context.BlogLike.Where(like => like.BlogPostId == Blogid).ToList();
            List<BlogComment> Removecomments= _context.BlogComment.Where(comment=>comment.BlogPostId==Blogid).ToList(); 
            _context.BlogLike.RemoveRange(RemoveLikes);
            _context.BlogComment.RemoveRange(Removecomments);
            _context.SaveChanges();
            }
        }
        public void UpdateBlog(BlogPost blog)
        {
            BlogPost PreviousPost = GetBlogById(blog.Id);
            PreviousPost.CreationDate = DateTime.Now.ToString("dd,MMMM,yyyy");
            PreviousPost.CityId=blog.CityId;
            PreviousPost.Image=blog.Image;
            PreviousPost.Title=blog.Title;
            PreviousPost.Content=blog.Content;
            PreviousPost.Summary=blog.Summary;
            _context.SaveChanges();
        }

    }
}