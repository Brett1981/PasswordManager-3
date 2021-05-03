using System;
using System.Collections.Generic;

#nullable disable

namespace WebApplication.DataAccess
{
    public partial class Category
    {
        public Category()
        {
            Accounts = new HashSet<Account>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
