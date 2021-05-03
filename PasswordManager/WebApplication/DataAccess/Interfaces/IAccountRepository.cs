using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.DataAccess.Interfaces
{
    public interface IAccountRepository : IRepository<Account, Dbo.Account>
    {
        public List<Dbo.Account> GetById(int id);
        public List<Dbo.Account> GetBySessionId(int sessionId);
        public List<Dbo.Category> GetCategoriesBySessionId(int sessionId);
        public int? GetCategoryByName(string name, int sessionId);
    }
}
