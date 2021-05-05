using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class SearchViewModel
    {
        public string Value { get; set; }
        public string SearchBy { get; set; }
    }
}
