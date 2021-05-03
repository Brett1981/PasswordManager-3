using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class UserViewModel
    {
        public string email { get; set; }
        [Required(ErrorMessage = "Please enter student name.")]
        public string login { get; set; }
        [Required]
        public string password { get; set; }

        public string confirmpassword { get; set; }
    }
}
