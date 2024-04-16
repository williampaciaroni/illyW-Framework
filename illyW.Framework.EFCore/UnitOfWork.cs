using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using illyW.Framework.Core.RepositoryPattern;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace illyW.Framework.EFCore
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        protected TContext _Context { get; set; }

        private IEnumerable<IGenericRepository> _Repositories;
        private IDbContextTransaction _ActiveTransaction;
        private bool disposed = false;

        public UnitOfWork(TContext context, IEnumerable<IGenericRepository> repositories)
        {
            ArgumentNullException.ThrowIfNull(context);
            Debug.WriteLine($"UnitOfWork - Constructor - Context hash code: {context.GetHashCode()}");
            _Context = context;
            _Repositories = repositories;
            BeginTransaction();
        }

        public void BeginTransaction()
        {
            Debug.WriteLine($"UnitOfWork - BeginTransaction - Trying to begin transaction");
            if (_ActiveTransaction != null)
            {
                Debug.WriteLine($"UnitOfWork - BeginTransaction - Active transaction is not null");

                ExecRollback();
            }
            _ActiveTransaction = _Context.Database.BeginTransaction();
            Debug.WriteLine($"UnitOfWork - ExecRollback - Transaction successfully began");

        }

        public void Commit()
        {
            _Context.Database.CommitTransaction();
            _ActiveTransaction = null;
            _Context.Database.BeginTransaction(); 
        }

        public TRepository GetRepository<TRepository>() where TRepository : IGenericRepository
        {
            var r = _Repositories.OfType<TRepository>().FirstOrDefault();
            ArgumentNullException.ThrowIfNull(r);
            return r;
        }

        protected virtual void RejectScalarChanges()
        {
            foreach (var entry in _Context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        public void Rollback()
        {
            ExecRollback();
            BeginTransaction(); 
        }

        protected virtual void ExecRollback()
        {
            Debug.WriteLine($"UnitOfWork - ExecRollback - Trying to execute rollback");
            RejectScalarChanges();
            _Context.Database.RollbackTransaction();
            _ActiveTransaction = null;
            Debug.WriteLine($"UnitOfWork - ExecRollback - Rollback executed");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
