using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.DataAccess.Interfaces
{
    public interface IBankRepository : IRepository<Bank, Dbo.Bank>
    {

        public Dbo.Bank GetById(int id);
        public List<Dbo.Bank> GetBySessionId(int sessionId);
        
    }
}
