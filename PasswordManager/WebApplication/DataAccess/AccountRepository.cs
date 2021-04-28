using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.DataAccess
{
    public class AccountRepository : Repository<Account, Dbo.Account>, Interfaces.IAccountsRepository
    {
        public AccountRepository(PasswordManagerContext context, ILogger<AccountRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public List<Dbo.Account> GetById(int id)
        {
            var result = _context.Accounts.Where(x => x.Id == id).ToList();
            return _mapper.Map<List<Dbo.Account>>(result);
        }

        public List<Dbo.Account> GetAll()
        {
            var result = _context.Accounts.ToList();
            return _mapper.Map<List<Dbo.Account>>(result);
        }
    }
}