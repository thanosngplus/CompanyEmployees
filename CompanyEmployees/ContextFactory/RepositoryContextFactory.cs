using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace CompanyEmployees.ContextFactory
{
    // used to create a derived DbContext during design time, for migrations
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<RepositoryContext>()
                .UseSqlServer(configuration.GetConnectionString("sqlconnection"),
                // required because migration assembly is in the Repository project
                // more at https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/projects?tabs=dotnet-core-cli
                b => b.MigrationsAssembly("CompanyEmployees"));

            return new RepositoryContext(builder.Options);
        }
    }
}
