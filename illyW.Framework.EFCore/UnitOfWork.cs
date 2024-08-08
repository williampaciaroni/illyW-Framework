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
        protected TContext Context { get; set; }

        private readonly IEnumerable<IGenericRepository> _repositories;
        private IDbContextTransaction _activeTransaction;
        private bool _disposed;

        public UnitOfWork(TContext context, IEnumerable<IGenericRepository> repositories)
        {
            ArgumentNullException.ThrowIfNull(context);
            Debug.WriteLine($"UnitOfWork - Constructor - Context hash code: {context.GetHashCode()}");
            Context = context;
            _repositories = repositories;
            BeginTransaction();
        }

        public void BeginTransaction()
        {
            Debug.WriteLine($"UnitOfWork - BeginTransaction - Trying to begin transaction");
            if (_activeTransaction != null)
            {
                Debug.WriteLine($"UnitOfWork - BeginTransaction - Active transaction is not null");

                ExecRollback();
            }
            _activeTransaction = Context.Database.BeginTransaction();
            Debug.WriteLine($"UnitOfWork - ExecRollback - Transaction successfully began");

        }

        public void Commit()
        {
            Context.Database.CommitTransaction();
            _activeTransaction = null;
            Context.Database.BeginTransaction(); 
        }

        public TRepository GetRepository<TRepository>() where TRepository : IGenericRepository
        {
            var r = _repositories.OfType<TRepository>().FirstOrDefault();
            ArgumentNullException.ThrowIfNull(r);
            return r;
        }

        protected virtual void RejectScalarChanges()
        {
            foreach (var entry in Context.ChangeTracker.Entries())
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
            Context.Database.RollbackTransaction();
            _activeTransaction = null;
            Debug.WriteLine($"UnitOfWork - ExecRollback - Rollback executed");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                Context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
