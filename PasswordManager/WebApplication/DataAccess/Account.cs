using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.DataAccess
{
    public partial class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }
        public int? CategoryId { get; set; }
        public int SessionId { get; set; }

        public virtual Category Category { get; set; }
        public virtual User Session { get; set; }
    }
}
