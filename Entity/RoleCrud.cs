using SeyahatRehberi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeyahatRehberi.Entity
{
    public class RoleCrud
    {
        DataContext _context = new DataContext();
        public int GetRoleIdByName(string roleName)
        {
            Role role = _context.Role.FirstOrDefault(x => x.Name == roleName);
            if (role != null)
            {
                return role.Id;
            }
            return 0;
        }
        public Role GetRoleByName(string roleName)
        {
            Role role = _context.Role.FirstOrDefault(x => x.Name == roleName);
            return role;
        }
    }
}