using Domain.Entities;
using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Queries.UserQueries;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : BaseRepository<UserEntity, Guid>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public async Task<UserEntity?> FindByUsernameAsync(string username, bool trackChanges = true)
        {
            return await (
                trackChanges
                    ? UserQueries.FindUserByUsernameAsync(this._context, username)
                    : UserQueries.FindUserByUsernameNoTrackingAsync(this._context, username));
        }
    }
}
