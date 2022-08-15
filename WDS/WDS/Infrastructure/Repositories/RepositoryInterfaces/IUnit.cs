using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDS.Infrastructure.Repositories.RepositoryInterfaces
{
    public interface IUnit
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        void Commit();
        void Dispose();
    }
}
