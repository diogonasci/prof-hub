using Microsoft.EntityFrameworkCore;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Infrastructure.PostgresSql;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IDateTimeProvider dateTimeProvider) : DbContext(options), IUnitOfWork
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Ocorreu uma exceção de concorrência.", ex);
        }
    }
}
