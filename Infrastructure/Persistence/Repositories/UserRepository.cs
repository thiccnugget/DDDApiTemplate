using Domain.Entities;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
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
