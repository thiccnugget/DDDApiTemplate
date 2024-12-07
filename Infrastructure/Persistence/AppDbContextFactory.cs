using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        // This method is used by the EF Core tools to create a new instance of the AppDbContext, since the constructor
        // requires a DbContextOptions object that is constructed inside the Infrastructure project.
        public AppDbContext CreateDbContext(string[] args)
        {
            // When running the EF Core tools, the connection string should be passed as a CLI argument to avoid hardcoding it.
            // It is required when running commands that interact directly with the database, such as Update-Database.
            string connectionString = args.FirstOrDefault(string.Empty);

            DbContextOptionsBuilder<AppDbContext> optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(
                connectionString,
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
