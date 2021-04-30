using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.DataAccess.Interfaces
{
    public interface IUserRepository : IRepository<User,Dbo.User>
    {
        public string GetEmail(string email);
        public User GetUser(string login, string password);
    }
}
