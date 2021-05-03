using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.DataAccess
{
    public class AccountRepository : Repository<Account, Dbo.Account>, Interfaces.IAccountRepository
    {
        public AccountRepository(PasswordManagerContext context, ILogger<AccountRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public List<Dbo.Account> GetById(int id)
        {
            var result = _context.Accounts.Where(x => x.Id == id).ToList();
            return _mapper.Map<List<Dbo.Account>>(result);
        }

        public List<Dbo.Account> GetBySessionId(int sessionId)
        {
            var result = _context.Accounts.Where(x => x.SessionId == sessionId).ToList();
            return _mapper.Map<List<Dbo.Account>>(result);
        }

        public List<Dbo.Category> GetCategoriesBySessionId(int sessionId)
        {
            var categoriesList = new List<Dbo.Category>();
            var categories = _context.Categories.ToList();
            foreach (var category in categories)
            {
                var isCategoryInSession = category.Accounts.Any(x => x.SessionId == sessionId);
                if (isCategoryInSession)
                    categoriesList.Add(_mapper.Map<Dbo.Category>(category));
            }
            return categoriesList;
        }

        public int? GetCategoryByName(string name, int sessionId)
        {
            var account = _context.Accounts.Where(x => x.SessionId == sessionId && x.Category.Name == name).FirstOrDefault();
            if (account == null)
                return null;

            return account.CategoryId;
        }
    }
}