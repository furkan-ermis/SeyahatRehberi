using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SeyahatRehberi;
using SeyahatRehberi.Entity;
using SeyahatRehberi.Models;

namespace SeyahatRehberi.Controllers
{
    [Authorize(Roles="User")]
    public class AccountController : Controller
    {
        AccountCrud accountCrud = new AccountCrud();
        BlogCrud blogCrud = new BlogCrud();
        CityCrud cityCrud = new CityCrud();
        private User _User ;
            public AccountController()
        {
            UserCrud userCrud = new UserCrud();
            _User= userCrud.GetCurrentUser();
            ViewBag.userName = _User.Username;
            ViewBag.userImage = _User.ProfileImage;

        }
        public ActionResult Index()
        {
            if (accountCrud.GetBlogs() != null) 
                return View(accountCrud.GetBlogs());
                return RedirectToAction("Index","Home");
        }


        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(cityCrud.getCities(), "Id", "CityName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BlogPost blogPost, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/travel/assets/images/blogImages/"), fileName);
                    ImageFile.SaveAs(path);
                    blogPost.Image = "/Content/travel/assets/images/blogImages/" + fileName;
                }
                blogPost.UserBlogPostId =_User.Id;
                blogCrud.AddBlog(blogPost);
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        public ActionResult Edit(int blogId)
        {
            BlogPost blogPost = blogCrud.GetBlogById(blogId);
            ViewBag.City = cityCrud.getCities();
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( BlogPost blogPost, HttpPostedFileBase ImageFile)
        {

                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    string path = Path.Combine(Server.MapPath("~/Content/travel/assets/images/blogImages/"), fileName);
                    ImageFile.SaveAs(path);
                    blogPost.Image = "/Content/travel/assets/images/blogImages/" + fileName;
                }
                blogPost.UserBlogPostId=_User.Id;
            blogPost.CreationDate = DateTime.Now.ToString("dd,MMMM,yyyy");
                blogCrud.UpdateBlog(blogPost);
               return RedirectToAction("Index");
        }

        public ActionResult Delete(int blogId)
        {
            BlogPost blogPost = blogCrud.GetBlogById(blogId);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int blogId)
        {
            blogCrud.DeleteBlog(blogId);
            return RedirectToAction("Index");
        }

    }
}
