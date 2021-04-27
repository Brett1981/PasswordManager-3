using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.Dbo
{
    public class Account : IObjectWithId
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
