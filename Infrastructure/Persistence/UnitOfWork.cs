﻿using Application.Interfaces;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private readonly ILogger<UnitOfWork> _logger;
    private IUserRepository? _userRepository;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext context, ILogger<UnitOfWork> logger)
    {
        _context = context;
        _logger = logger;
    }

    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _transaction?.CommitAsync()!;
        }
        catch(Exception ex)
        {
            _logger.LogError("Rolling back transaction {@transactionId}. Error: {@error}", _transaction?.TransactionId, ex.Message);
            await this.RollbackTransactionAsync();
        }
        finally
        {
            if (_transaction is not null)
            {
                await _transaction.DisposeAsync();
            }
        }
    }

    public async Task RollbackTransactionAsync()
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

    public async Task<int> SaveAsTransactionAsync()
    {
        await this.BeginTransactionAsync();
        try
        {
            var result = await this.SaveChangesAsync();
            await this.CommitTransactionAsync();
            return result;
        }
        catch(Exception ex)
        {
            _logger.LogError("Rolling back transaction {@transactionId}. Error: {@error}", _transaction?.TransactionId, ex.Message);
            await this.RollbackTransactionAsync();
            throw;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}