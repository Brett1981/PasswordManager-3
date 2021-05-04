using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Dbo
{
    public class Bank : IObjectWithId
    {
        public int Id { get; set; }
        public string NumberCard { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Cvc { get; set; }
        public int SessionId { get; set; }
    }
}
