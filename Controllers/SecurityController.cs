using SeyahatRehberi.Entity;
using SeyahatRehberi.Models;
using SeyahatRehberi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SeyahatRehberi.Controllers
{
    public class SecurityController : Controller
    {
        UserCrud userCrud=new UserCrud();
        RoleCrud roleCrud=new RoleCrud();
        // GET: Security
        public ActionResult Login()
        {
            return View();
        }

        // POST: Security
        [HttpPost]
        public ActionResult Login(LoginViewModel user)
        {
            User _user = userCrud.GetLoginUser(user);
            int userRoleId=roleCrud.GetRoleIdByName("User");
            int AdminRoleId=roleCrud.GetRoleIdByName("Admin");
            if (_user!=null && _user.RoleId== userRoleId)
            {
                FormsAuthentication.SetAuthCookie(user.Email, false);
                return RedirectToAction("index","Home");
            }
            else if (_user != null && _user.RoleId == AdminRoleId)
            {
                FormsAuthentication.SetAuthCookie(user.Email, false);
                return RedirectToAction("Index", "Admin");
            }
            ViewBag.Message = "Kullanıcı Bilgileri Hatalı";
            return View(user);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("index","Home");
        }
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(SignInViewModel signInUser)
        {
            bool add = userCrud.AddUser(signInUser);
            if (add  ) { 
            return RedirectToAction("Index","Home");
            }
            return View();
        }
    }
}