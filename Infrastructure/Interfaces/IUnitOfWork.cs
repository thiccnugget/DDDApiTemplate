using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        Task<int> SaveChangesAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
