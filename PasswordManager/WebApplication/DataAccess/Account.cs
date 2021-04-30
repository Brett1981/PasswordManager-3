﻿using System;
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
        public string CategoryId { get; set; }
        public string SessionId { get; set; }
    }
}
