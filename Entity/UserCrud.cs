using SeyahatRehberi.Models;
using SeyahatRehberi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeyahatRehberi.Entity
{
    public class UserCrud
    {
        DataContext _context=new DataContext();
        RoleCrud roleCrud=new RoleCrud();
        public List<User> GetUsers()
        {
            int roleId = roleCrud.GetRoleIdByName("Admin");
            List<User> userList = _context.User.Where(user=>!user.IsDelete && user.RoleId!= roleId).ToList();
            if (userList != null)
            {
                return userList;
            }
            return null;
        }
        public User GetUser(int userId)
        {
            User user = _context.User.FirstOrDefault(x=>x.Id==userId&& !x.IsDelete);
            if (user != null)
            {
                return user;
            }
            return null;
        }
        public User GetCurrentUser()
        {
            User currentUser= _context.User.FirstOrDefault(user => user.Email == HttpContext.Current.User.Identity.Name);
            if (currentUser!=null)
            {
                return currentUser;
            }
            else { return null; }
        }
        public User GetLoginUser(LoginViewModel currentUser)
        {
            return _context.User.FirstOrDefault(user=>user.Email== currentUser.Email && user.Password== currentUser.Password && !user.IsDelete && user.IsActive);
        }
        public void DeleteUser(int userId)
        {
            User user = GetUser(userId);
            user.IsDelete = true;
            _context.SaveChanges();
        }
        public List<User> GetAdmins()
        {
            int roleId = roleCrud.GetRoleIdByName("Admin");
            return _context.User.Where(x => x.RoleId == roleId).ToList();
        }
        public void UpdateUser(User user)
        {
            User _user = GetUser(user.Id);
            if (_user!=null)
            {
                _user.Username = user.Username;
                _user.Name = user.Name;
                _user.Email = user.Email;
                _user.CreationDate = user.CreationDate;
                _user.ProfileImage = user.ProfileImage;
                _context.SaveChanges();
            }
        }
        public bool AddUser(SignInViewModel user) {
            int roleId=_context.Role.FirstOrDefault(role => role.Name == "User").Id;

            if (!_context.User.Any(x => x.Email == user.Email) && user.IsAgree)
                {

            User _user =new User();
            _user.Username = user.Name+"-"+DateTime.Now.ToString("dMMyyyy");
            _user.Name = user.Name;
            _user.Email=user.Email;
            _user.Password=user.Password;
            _user.CreationDate = DateTime.Now.ToString("d,MMMM,yyyy");
            _user.IsActive = true;
            _user.IsDelete = false;
            _user.RoleId = roleId;
            _user.ProfileImage = "~/content/UserImage/default_profile.jpg";
            _context.User.Add(_user);
            _context.SaveChanges();
                return true;
            }
            return false;
        }
        public void ToggleUserActivation(int userId)
        {
            this.GetUser(userId).IsActive=this.GetUser(userId).IsActive? false:true;
            _context.SaveChanges();
        }
    }
}