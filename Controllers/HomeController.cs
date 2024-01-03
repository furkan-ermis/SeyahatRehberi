using SeyahatRehberi.Entity;
using SeyahatRehberi.Models;
using SeyahatRehberi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeyahatRehberi.Controllers
{
    public class HomeController : Controller
    {
        CommentCrud commentCrud=new CommentCrud();
        LikeCrud likeCrud=new LikeCrud();
        CityCrud cityCrud=new CityCrud();
        BlogCrud blogCrud = new BlogCrud(); 
        UserCrud userCrud=new UserCrud();
        RoleCrud roleCrud=new RoleCrud();

        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult IndexPartialBlogs()
        {
            ViewBag.Blogs = "Popüler Bloglar";
            List<BlogCardViewModel> blogCardViewModel=new List<BlogCardViewModel>();
            List<Comment> commentCount ;
            foreach (var item in blogCrud.GetBlogsTopLike())
            {
                commentCount = commentCrud.GetCommentsByBlog(item.Id);
                BlogCardViewModel blogCard= new BlogCardViewModel()
                {
                    BlogId = item.Id,
                    Description=item.Summary,
                    Author = userCrud.GetUser(Convert.ToInt32(item.UserBlogPostId)).Name,
                    City = cityCrud.GetName(item.CityId),
                    Comments=(commentCount!=null?commentCount.Count():0) ,
                    Likes= likeCrud.CountLikesByBlog(item.Id),
                    Image=item.Image,
                    Title=item.Title,
                };
                blogCardViewModel.Add(blogCard);
            }
            return PartialView("~/Views/Shared/BlogPartial/BlogListPartial.cshtml", blogCardViewModel);
        }
        [ChildActionOnly]
        public ActionResult IndexPartialCountStats()
        {
       
            List<int> Counts = new List<int>() {  likeCrud.GetLikesCount(), userCrud.GetUsers().Count(), blogCrud.GetBlogs().Count(), commentCrud.GetComments().Count()};
            return PartialView("~/Views/Shared/_HomePartial/Index/Stats.cshtml", Counts);
        }
        [ChildActionOnly]
        public ActionResult IndexPartialPopulerCities()
        {
            List<PopulerCitiesViewModel> populerCitiesViewModel=new List<PopulerCitiesViewModel>();
            PopulerCitiesViewModel populerCity;
            foreach (var item in cityCrud.getCitiesByMostBlogs())
            {
                populerCity = new PopulerCitiesViewModel() {
                    CityId=item.Id,
                    CityName=item.CityName,
                    BlogCount= blogCrud.GetBlogCountByCities(item.Id),
                    Image =item.Image,
                };
                populerCitiesViewModel.Add(populerCity);
            }
            return PartialView("~/Views/Shared/_HomePartial/Index/Cities.cshtml", populerCitiesViewModel);
        }

        [ChildActionOnly]
        public ActionResult IndexPartialLatestBlogs()
        {
            List<Comment> commentCount;
            List<BlogCardViewModel> blogCardViewModel = new List<BlogCardViewModel>();
            foreach (var item in blogCrud.GetBlogsByLatest())
            {
                commentCount = commentCrud.GetCommentsByBlog(item.Id);
                BlogCardViewModel blogCard = new BlogCardViewModel()
                {
                    BlogId = item.Id,
                    Description = item.Summary,
                    City = cityCrud.GetName(item.CityId),
                    Comments = (commentCount != null ? commentCount.Count() : 0),
                    Likes = likeCrud.CountLikesByBlog(item.Id),
                    Image = item.Image,
                    Title = item.Title,
                };
                blogCardViewModel.Add(blogCard);

            }
            return PartialView("~/Views/Shared/_HomePartial/Index/LatestBlogs.cshtml", blogCardViewModel);
        }
        
        public ActionResult About()
        {
            List<User> Admins =userCrud.GetAdmins();
            return View(Admins);
        }
        [ChildActionOnly]
        public ActionResult AboutPartialStats()
        {
            List<int> stats= new List<int>() ;
            var users = userCrud.GetUsers();
            var blogs =blogCrud.GetBlogs();
            if (users.Count!=0 && blogs.Count != 0)
            {
                stats.Add(users.Count);
                stats.Add(blogs.Count);
            }
            else { 
                stats.Add(0);
                stats.Add(0);
            }
            return PartialView("~/Views/Shared/AboutPartial/StatsPartial.cshtml", stats);
        }
        public ActionResult Contact()
        {
            return View();
        }
        
    }
}