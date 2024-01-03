using SeyahatRehberi.Entity;
using SeyahatRehberi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace SeyahatRehberi.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private User _User;
        DataContext _context = new DataContext();
        BlogCrud blogCrud = new BlogCrud();
        CityCrud cityCrud = new CityCrud();
        CommentCrud commentCrud = new CommentCrud();
        UserCrud userCrud = new UserCrud();
        public AdminController()
        {
            _User = userCrud.GetCurrentUser();
            ViewBag.RaportedBlogs = blogCrud.GetReportedBlogs().Count;
            if (_User != null)
            {
                ViewBag.Name = _User.Username;
                ViewBag.Image = _User.ProfileImage;
            }
        }
        public ActionResult Index()
        {
            var blogs = _context.BlogPost
                .Where(b => !b.IsDeleted)
                .Include(b => b.City)
                .Include(b => b.UserBlogPost)
                .ToList();
            return View(blogs);
        }
        public ActionResult RaportedBlogs()
        {
            var blogs = blogCrud.GetReportedBlogs();
            return View(blogs);
        }
        public ActionResult ApproveRaport(int blogId)
        {
            blogCrud.RaportBlog(blogId);
            return RedirectToAction("RaportedBlogs", "Admin");
        }
        public ActionResult EditBlogs(int? blogId)
        {
            BlogPost blogPost = blogCrud.GetBlogById(Convert.ToInt32(blogId));
            ViewBag.City = cityCrud.getCities();
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBlogs(BlogPost blogPost, HttpPostedFileBase ImageFile)
        {

            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                string fileName = Path.GetFileName(ImageFile.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/travel/assets/images/blogImages/"), fileName);
                ImageFile.SaveAs(path);
                blogPost.Image = "/Content/travel/assets/images/blogImages/" + fileName;
            }

            blogPost.CreationDate = DateTime.Now.ToString("dd,MMMM,yyyy");
            blogCrud.UpdateBlog(blogPost);
            return RedirectToAction("Index");
        }
        public ActionResult DeleteBlog(int? blogId)
        {
            BlogPost blogPost = blogCrud.GetBlogById(Convert.ToInt32(blogId));
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        [HttpPost, ActionName("DeleteBlog")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int blogId)
        {
            blogCrud.DeleteBlog(blogId);
            return RedirectToAction("Index");
        }
        //-------------------- USER ---------------
        public ActionResult Users()
        {
            var users = userCrud.GetUsers();
            return View(users);
        }
        public ActionResult EditUser(int userId)
        {
            var user = userCrud.GetUser(userId);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User user, HttpPostedFileBase ImageFile)
        {
            if (ImageFile != null && ImageFile.ContentLength > 0)
            {
                string fileName = Path.GetFileName(ImageFile.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/UserImage/"), fileName);
                ImageFile.SaveAs(path);
                user.ProfileImage = "/Content/UserImage/" + fileName;
            }

            user.CreationDate = DateTime.Now.ToString("dd,MMMM,yyyy");
            userCrud.UpdateUser(user);
            return RedirectToAction("Index");
        }
        public ActionResult DeleteUser(int? userId)
        {
            User user = userCrud.GetUser(Convert.ToInt32(userId));
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUserConfirmed(int? userId)
        {
            userCrud.DeleteUser(Convert.ToInt32(userId));
            return RedirectToAction("Users");
        }
        public ActionResult GetAwaitingCommentList()
        {
            var comments = commentCrud.getWaitingCommentsWithBlogs();
            return View(comments);
        }
        public ActionResult ToggleActivationUser(int userId)
        {
            userCrud.ToggleUserActivation(userId);
            var user=userCrud.GetUser(userId);
            return RedirectToAction("Users");
        }
        //-------------------- COMMENT ---------------

        public ActionResult CommentDetail(int commentId) {
            var comment = commentCrud.getWaitingCommentsWithBlogs(commentId);
            return View(comment);
        }
        public ActionResult ApproveComment(int commentId)
        {
            commentCrud.ApproveComment(commentId);
            return RedirectToAction("GetAwaitingCommentList");
        }
        public ActionResult DeleteComment(int commentId)
        {
            commentCrud.DeleteComment(commentId);
            return RedirectToAction("GetAwaitingCommentList");
        }
    }
}
