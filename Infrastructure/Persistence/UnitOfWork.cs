using Application.Interfaces;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IUserRepository? _userRepository;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransaction()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransaction()
    {
        try
        {
            await _transaction?.CommitAsync()!;
        }
        finally
        {
            if (_transaction is not null)
            {
                await _transaction.DisposeAsync();
            }
        }
    }

    public async Task RollbackTransaction()
    {
        try
        {
            await _transaction?.RollbackAsync()!;
        }
        finally
        {
            if (_transaction is not null)
            {
                await _transaction.DisposeAsync();
            }
        }
    }

    public async Task<int> SaveAsTransaction()
    {
        await this.BeginTransaction();
        try
        {
            var result = await this.SaveChangesAsync();
            await this.CommitTransaction();
            return result;
        }
        catch
        {
            await this.RollbackTransaction();
            throw;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}