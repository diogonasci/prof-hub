using Microsoft.EntityFrameworkCore;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Infrastructure.PostgresSql;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) : DbContext(options), IUnitOfWork
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
            int result = await base.SaveChangesAsync(cancellationToken);
            return result;
    }
}
