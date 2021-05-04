using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.DataAccess
{
    public partial class Bank
    {
        public int Id { get; set; }
        public string NumberCard { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Cvc { get; set; }
        public int SessionId { get; set; }
    }
}
