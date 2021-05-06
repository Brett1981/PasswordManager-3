using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class PasswordViewModel
    {
        public string Password { get; set; }
        public string Length { get; set; }
        [Display(Name = "Allow uppercases")]
        public bool Uppercase { get; set; }
        [Display(Name = "Allow numbers")]
        public bool Number { get; set; }
        [Display(Name = "Allow symbols")]
        public bool Symbol { get; set; }
    }
}
