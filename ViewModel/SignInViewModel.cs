using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SeyahatRehberi.ViewModel
{
    public class SignInViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAgree { get; set; }
    }
}