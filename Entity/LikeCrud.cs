using SeyahatRehberi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeyahatRehberi.Entity
{
    public class LikeCrud
    {
        DataContext _context=new DataContext();
        UserCrud _userCrud=new UserCrud();
        private User User;
            public LikeCrud()
        {
            User = _userCrud.GetCurrentUser();
        }
        public void LikeBlog(int BlogId)
        {
            BlogLike blogLike = new BlogLike()
            {
                UserBlogLikeId = User.Id,
                BlogPostId = BlogId,
            };
            _context.BlogLike.Add(blogLike);
            _context.SaveChanges();
        }
        public void DeleteLike(int BlogId)
        {
            _context.BlogLike.Remove(_context.BlogLike.FirstOrDefault(x => x.BlogPostId == BlogId&& x.UserBlogLikeId==User.Id));
            _context.SaveChanges();
        }
        public int CountLikesByBlog(int BlogId)
        {
           return _context.BlogLike.Where(blog=>blog.BlogPostId==BlogId).Count();
        }
        public int CountLikesByUser()
        {
            return _context.BlogLike.Where(blog => blog.UserBlogLikeId == User.Id).Count();

        }
        public List<BlogLike> GetLikesByBlog(int blogId) {
            return _context.BlogLike.Where(x=>x.BlogPostId==blogId).ToList();
        }
        public List<BlogPost> getBlogsByLikesOfUser()
        {
            List<int> userLikedBlogIds = _context.BlogLike
               .Where(likedBlog => likedBlog.UserBlogLikeId == User.Id)
               .Select(blogId => blogId.BlogPostId)
               .ToList();
            if (userLikedBlogIds!=null)
            {
                List<BlogPost> userLikedBlogs = _context.BlogPost
                   .Where(blog => userLikedBlogIds.Contains(blog.Id) && !blog.IsRaported)
                   .ToList();

            return userLikedBlogs;
            }
            return null;
        }
        public int GetLikesCount()
        {
            return _context.BlogLike.Count();
        }
        public bool IsLiked(int blogId)
        {
            BlogLike like = _context.BlogLike.FirstOrDefault(x => x.BlogPostId == blogId && x.UserBlogLikeId == User.Id);
            if (like!=null)
            {
                return true;
            }
            return false;
        }

    }
}