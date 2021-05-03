using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class AccountViewModel
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        [Display(Name = "Category")]
        public string Category { get; set; }
    }
}
