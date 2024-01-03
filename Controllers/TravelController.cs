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
    public class TravelController : Controller
    {
        CityCrud cityCrud = new CityCrud();
        BlogCrud blogCrud = new BlogCrud();
        CommentCrud commentCrud = new CommentCrud();
        LikeCrud likeCrud = new LikeCrud();
        UserCrud userCrud = new UserCrud();
        public ActionResult City()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult CityPartial()
        {
            List<PopulerCitiesViewModel> populerCitiesViewModel = new List<PopulerCitiesViewModel>();
            PopulerCitiesViewModel populerCity;
            foreach (var item in cityCrud.getCities())
            {
                populerCity = new PopulerCitiesViewModel()
                {
                    CityId=item.Id,
                    CityName = item.CityName,
                    BlogCount = blogCrud.GetBlogCountByCities(item.Id),
                    Image = item.Image,
                };
                populerCitiesViewModel.Add(populerCity);
            }
            return PartialView("~/Views/Shared/CitiesPartial/CitiesPartial.cshtml", populerCitiesViewModel.OrderByDescending(x=>x.BlogCount).ToList());

        }
        public ActionResult BlogList(int? CityId)
        {
            if (CityId.HasValue && CityId != -1)
            {
                return View(CityId);
            }
            return View(0);
        }
        [ChildActionOnly]
        public ActionResult BlogsPartial(int cityId)
        {

            List<BlogCardViewModel> blogCardViewModel = new List<BlogCardViewModel>();
            List<Comment> commentCount;
            List<BlogPost> Model=blogCrud.GetBlogs();
            string CityName = "";
            if (cityId != 0)
            {
                Model=blogCrud.GetBlogsByCities(cityId);
                 CityName = cityCrud.GetName(Convert.ToInt32(cityId));
            }
            ViewBag.Blogs = CityName+" Bloglar";
            foreach (var item in Model)
            {
                commentCount = commentCrud.GetCommentsByBlog(item.Id);
                    BlogCardViewModel blogCard = new BlogCardViewModel()
                {
                    BlogId = item.Id,
                    Description = item.Summary,
                    City = cityCrud.GetName(item.CityId),
                    Comments = (commentCount != null ? commentCount.Count() : 0),
                    Author = userCrud.GetUser(Convert.ToInt32(item.UserBlogPostId)).Name,
                    Likes = likeCrud.CountLikesByBlog(item.Id),
                    Image = item.Image,
                    Title = item.Title,
                };
                blogCardViewModel.Add(blogCard);

            }
            return PartialView("~/Views/Shared/BlogPartial/BlogListPartial.cshtml", blogCardViewModel);
        }
        [Authorize]
        public ActionResult BlogDetail(int blogId)
        {
            BlogPost blog=blogCrud.GetBlogById(blogId);
            
            ViewBag.Author=userCrud.GetUser(Convert.ToInt32(blog.UserBlogPostId)).Name;
            @ViewBag.AuthorImage = userCrud.GetUser(Convert.ToInt32(blog.UserBlogPostId)).ProfileImage;
            blog.BlogLikeList= likeCrud.GetLikesByBlog(blogId);
            ViewBag.LikeCount=blog.BlogLikeList.Count;
            ViewBag.IsLiked = "false";
            ViewBag.IsRaport = "false";
            if (blogCrud.GetBlogById(blogId).IsRaported)
            {
                ViewBag.IsRaport = "true";

            }
            if (likeCrud.IsLiked(blogId))
            {
            ViewBag.IsLiked = "true";
            }
            return View(blog);
        }

        [ChildActionOnly]
        public ActionResult AddCommentPartial(int blogId)
        {
            var commentModel = new CommentViewModel()
            {
                BlogId = blogId,
            };
            
            return PartialView("~/Views/Shared/BlogDetailPartial/LeaveComment.cshtml", commentModel);

        }

        [HttpPost]
        [Authorize]
        public ActionResult PostComment(int blogId, CommentViewModel _comment)
        {
            Comment newComment = new Comment() { 
            Content= _comment.Comment
            };
            commentCrud.AddComment(blogId, newComment);
            return RedirectToAction("BlogDetail", new { blogId = blogId });
        }

        [ChildActionOnly]
        public ActionResult AuthorPartial(int userId) {
            User user =userCrud.GetUser(userId);
            return PartialView("~/Views/Shared/BlogDetailPartial/AuthorDetailPartial.cshtml", user);
        }
        [ChildActionOnly]
        public ActionResult CommentsPartial(int blogId)
        {
            List<Comment> comments = commentCrud.GetCommentsByBlog(blogId);
            List<UserCommentViewModel> commentViewModelList=new List<UserCommentViewModel>();
            User user;
            ViewBag.CommentCount = 0;
            if (comments!=null)
            {
                foreach (var item in comments)
                {
                    user = userCrud.GetUser(item.UserCommentId);
                    UserCommentViewModel commentViewModel = new UserCommentViewModel()
                    {
                        UserImage = user.ProfileImage,
                        CommentContent = item.Content,
                        CommentDate = item.CommentDate,
                        UserName = user.Username
                    };
                    commentViewModelList.Add(commentViewModel);
                }
                ViewBag.CommentCount = comments.Count();
            }
            return PartialView("~/Views/Shared/BlogDetailPartial/CommentsPartial.cshtml", commentViewModelList);
        }

        [HttpPost]
        public ActionResult BlogLike(int blogId)
        {
            if(likeCrud.IsLiked(blogId))
            {
                likeCrud.DeleteLike(blogId);
            }
            else
            {
                likeCrud.LikeBlog(blogId);
            }
            return RedirectToAction("BlogDetail", new { blogId = blogId });

        }
        [HttpPost]
        public ActionResult RaportBlog(int blogId)
        {
           blogCrud.RaportBlog(blogId);
            return RedirectToAction("BlogDetail", new { blogId = blogId });
        }
    }
}