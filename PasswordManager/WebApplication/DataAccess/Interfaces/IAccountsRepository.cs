using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.DataAccess.Interfaces
{
    public interface IAccountsRepository : IRepository<Account, Dbo.Account>
    {
        public List<Dbo.Account> GetById(int id);
        public List<Dbo.Account> GetAll();
    }
}
