using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [StringLength(30, ErrorMessage = "Email can be no larger than 30 characters")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }


        [Required(ErrorMessage = "Username is  required")]
        [StringLength(30, ErrorMessage = "Username can be no larger than 30 characters")]
        public string login { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Password can be no larger than 30 characters")]
        public string confirmpassword { get; set; }
    }
}
