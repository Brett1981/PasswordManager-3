using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.DataAccess.Interfaces
{
    public interface IAccountRepository : IRepository<Account, Dbo.Account>
    {
        public Dbo.Account GetById(int id);
        public List<Dbo.Account> GetBySessionId(int sessionId);
        public List<Dbo.Category> GetCategoriesBySessionId(int sessionId);
    }
}
