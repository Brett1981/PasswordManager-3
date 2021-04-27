using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.DataAccess.Interfaces
{
    public interface IRepository<DBEntity, ModelEntity>

    {
        Task<IEnumerable<ModelEntity>> Get(string includeTables = "");
        Task<ModelEntity> Insert(ModelEntity entity);
        Task<ModelEntity> Update(ModelEntity entity);
        Task<bool> Delete(int idEntity);

    }
}
