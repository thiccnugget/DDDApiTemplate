using Infrastructure.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        public Task<UserEntity?> FindByUsername(string username);
    }
}
