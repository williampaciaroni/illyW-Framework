using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bscframework.Core
{
    public interface IUnitOfWork
    {
        TRepository GetRepository<TRepository>()
            where TRepository : IGenericRepository;

        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}
