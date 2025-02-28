using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Prof.Hub.Infrastructure.PostgresSql;
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        // Configure the context to use Postgres
        optionsBuilder.UseNpgsql(
            "Host=postgres;Database=profhub;Username=postgres;Password=postgres123",
            b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
