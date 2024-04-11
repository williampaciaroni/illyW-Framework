namespace illyW.Framework.Core
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
