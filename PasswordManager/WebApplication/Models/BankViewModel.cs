using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class BankViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Card number")]
        [StringLength(16)]
        public string NumberCard { get; set; }
        public string Name { get; set; }
        [StringLength(5)]
        public string Date { get; set; }
        [StringLength(3)]
        public string Cvc { get; set; }
    }
}
