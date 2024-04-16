using System;

namespace illyW.Framework.Core.RepositoryPattern
{
    public interface IUnitOfWork : IDisposable
    {
        TRepository GetRepository<TRepository>()
            where TRepository : IGenericRepository;

        void BeginTransaction();

        void Commit();

        void Rollback();
    }
}
