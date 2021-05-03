using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.DataAccess.Interfaces
{
    public interface ICategoryRepository : IRepository<Category, Dbo.Category>
    {
        public List<Dbo.Category> GetById(int id);
        public int? GetByName(string name);
    }
}
