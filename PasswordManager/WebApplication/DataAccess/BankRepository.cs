using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;


namespace WebApplication.DataAccess
{
    public class BankRepository : Repository<Bank, Dbo.Bank>, Interfaces.IBankRepository
    {
        public BankRepository(PasswordManagerContext context, ILogger<BankRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }
        public List<Dbo.Bank> GetById(int id)
        {
            var result = _context.Banks.Where(x => x.Id == id).ToList();
            return _mapper.Map<List<Dbo.Bank>>(result);
        }

        public List<Dbo.Bank> GetBySessionId(int sessionId)
        {
            var result = _context.Banks.Where(x => x.SessionId == sessionId).ToList();
            return _mapper.Map<List<Dbo.Bank>>(result);
            
        }

       
    }
}
