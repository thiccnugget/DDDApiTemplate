using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Infrastructure.Database.Repositories;
using Infrastructure.Interfaces;

namespace Infrastructure.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;
        private bool _disposed;
        private IUserRepository _userRepository;


        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository { get => _userRepository ??= new UserRepository(_context); }

        public void BeginTransaction()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("A transaction is already in progress");
            }
            _transaction = _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            try
            {
                _transaction?.Commit();
            }
            catch
            {
                this.RollbackTransaction();
                throw;
            }
            finally
            {
                if (_transaction is not null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                if (_transaction is not null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}