using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.DataAccess
{
    public class CategoryRepository : Repository<Category, Dbo.Category>, Interfaces.ICategoryRepository
    {
        public CategoryRepository(PasswordManagerContext context, ILogger<AccountRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public Dbo.Category GetById(int id)
        {
            var result = _context.Categories.Where(x => x.Id == id).FirstOrDefault();
            return _mapper.Map<Dbo.Category>(result);
        }

        public int? GetByName(string name)
        {
            var category = _context.Categories.Where(x => x.Name == name).FirstOrDefault();
            if (category == null)
                return null;

            return category.Id;
        }
    }
}