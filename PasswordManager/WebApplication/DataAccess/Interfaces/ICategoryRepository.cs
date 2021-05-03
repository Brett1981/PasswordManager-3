using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.DataAccess.Interfaces
{
    public interface ICategoryRepository : IRepository<Category, Dbo.Category>
    {
        public List<Dbo.Account> GetBySessionId(int sessionId);
        public int? GetByName(string name, int sessionId);
    }
}
