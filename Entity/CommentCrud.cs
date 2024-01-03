using SeyahatRehberi.Models;
using SeyahatRehberi.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SeyahatRehberi.Entity
{
    //Content,CommentDate,IsStatus,IsDelete,UserId
    public class CommentCrud
    {
        DataContext _context=new DataContext();
        UserCrud _userCrud = new UserCrud();
        private User User;
        public CommentCrud()
        {
            User = _userCrud.GetCurrentUser();
        }
        public void AddComment(int blogId, Comment comment)
        {
            comment.CommentDate = DateTime.Now;
            comment.UserCommentId = User.Id;
            comment.IsStatus = false;
            _context.Comment.Add(comment);
            _context.SaveChanges(); 
            BlogComment blogComment = new BlogComment()
            {
                BlogPostId = blogId,
                CommentId = comment.Id
            };

            _context.BlogComment.Add(blogComment);
            _context.SaveChanges();
        }
        [Authorize(Roles ="Admin")]
        public void ApproveComment(int commentId)
        {
            _context.Comment.Find(commentId).IsStatus = true;
            _context.SaveChanges();
        }
        public void DeleteComment(int commentId)
        {
            var comment = _context.Comment.Find(commentId);
            BlogComment blogComment=_context.BlogComment.FirstOrDefault(commentblog => commentblog.CommentId == comment.Id);
            _context.BlogComment.Remove(blogComment);
            _context.Comment.Remove(comment);
            _context.SaveChanges();
        }
        public List<Comment> GetCommentsByBlog(int blogId) {
            List<int> blogCommentIds = _context.BlogComment
               .Where(blogComment => blogComment.BlogPostId==blogId)
               .Select(Id => Id.CommentId)
               .ToList();
            if (blogCommentIds.Count() != 0)
            {
                var BlogComments = _context.Comment
                    .Where(comment => blogCommentIds.Contains(comment.Id) && comment.IsStatus )
                        .ToList();
                return BlogComments;
            }
            return null;
        }
        public List<WaitingCommentsViewModel> getWaitingCommentsWithBlogs()
        {
           List<WaitingCommentsViewModel> comments = _context.BlogComment
                                    .Include(x=>x.BlogPost)
                                    .Include(x=>x.Comment)
                                    .Include(x=>x.Comment.UserComment)
                                    .Where(x=>x.Comment.IsStatus==false)
                                    .Select(x=>new WaitingCommentsViewModel { 
                                        CommentId = x.Comment.Id,
                                        Comment=x.Comment.Content,
                                        BlogTitle=x.BlogPost.Title,
                                        CommentDate=x.Comment.CommentDate,
                                        UserId=x.Comment.UserCommentId,
                                        Username=x.Comment.UserComment.Name
                                    }).ToList();
            return comments;
        }
        public WaitingCommentsViewModel getWaitingCommentsWithBlogs(int commentId)
        {
            WaitingCommentsViewModel comments = _context.BlogComment
                                     .Include(x => x.BlogPost)
                                     .Include(x => x.Comment)
                                     .Include(x => x.Comment.UserComment)
                                     .Where(x => x.Comment.IsStatus == false && x.CommentId==commentId)
                                     .Select(x => new WaitingCommentsViewModel
                                     {
                                         CommentId = x.Comment.Id,
                                         Comment = x.Comment.Content,
                                         BlogTitle = x.BlogPost.Title,
                                         CommentDate = x.Comment.CommentDate,
                                         UserId = x.Comment.UserCommentId,
                                         Username = x.Comment.UserComment.Name
                                     }).FirstOrDefault();
            return comments;
        }
        public List<Comment> GetCommentsByUser(int UserId)
        {
            List<Comment> comments = _context.Comment
               .Where(comment => comment.UserCommentId == UserId && comment.IsStatus)
               .ToList();
            if (comments != null)
            {
                return comments;
            }
            return null;
        }
        public List<Comment> GetComments()
        {
            return _context.Comment.ToList();
        }
    }
}