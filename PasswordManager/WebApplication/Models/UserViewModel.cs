using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class UserViewModel
    {
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }


        [Required(ErrorMessage = "Username is  required")]
        public string login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }

        public string confirmpassword { get; set; }
    }
}
