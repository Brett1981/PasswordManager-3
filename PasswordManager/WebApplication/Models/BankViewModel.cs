using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class BankViewModel
    {
        public int Id { get; set; }
        public string NumberCard { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Cvc { get; set; }
    }
}
