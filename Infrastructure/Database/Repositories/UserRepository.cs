using Infrastructure.Database.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Database.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public async Task<UserEntity?> FindByUsername(string username)
        {
            return await this.FindOne(x => x.Username == username);
        }
    }
}
