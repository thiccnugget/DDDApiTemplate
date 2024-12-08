using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Queries.UserQueries;

// Use compilated queries to improve performance when querying frequently used data
internal static class UserQueries
{
    public static readonly Func<DbContext, string, Task<UserEntity?>> FindUserByUsernameAsync = EF.CompileAsyncQuery((DbContext context, string username) =>
        context.Set<UserEntity>().FirstOrDefault(x => x.Username == username));

    public static readonly Func<DbContext, string, Task<UserEntity?>> FindUserByUsernameNoTrackingAsync = EF.CompileAsyncQuery((DbContext context, string username) =>
        context.Set<UserEntity>().AsNoTracking().FirstOrDefault(x => x.Username == username));
}

