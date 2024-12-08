using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        public Task<UserEntity?> FindByUsernameAsync(string username, bool trackChanges = true);
    }
}
