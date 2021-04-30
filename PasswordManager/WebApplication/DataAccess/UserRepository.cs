using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.DataAccess
{
    public class UserRepository : Repository<User, Dbo.User>, Interfaces.IUserRepository
    {
        public UserRepository(PasswordManagerContext context, ILogger<UserRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public string GetEmail(string email)
        {
            var result = _context.Users.Where(x => x.Email == email).FirstOrDefault();
            if (result == null)
                return null;
            return result.Email;
        }
        public bool GetUser(string login,string password)
        {
            var user = _context.Users.Where(x => x.Username == login).FirstOrDefault();
            if (user == null)
                return false;
            bool verified = BCrypt.Net.BCrypt.Verify(password, user.Password);
            if (!verified)
                return false;
            return true;
        }

    }
}
