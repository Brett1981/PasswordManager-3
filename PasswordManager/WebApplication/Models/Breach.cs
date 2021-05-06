using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Breach
    {
        public string Domain { get; set; }
        public DateTime BreachDate { get; set; }
        public List<string> DataClasses { get; set; }
    }
}
