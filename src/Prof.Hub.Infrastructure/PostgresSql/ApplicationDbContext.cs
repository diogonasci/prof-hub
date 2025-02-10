using Microsoft.EntityFrameworkCore;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Infrastructure.PostgresSql;

public sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly IDomainEventPublisher? _domainEventPublisher;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IDomainEventPublisher? domainEventPublisher = null)
        : base(options)
    {
        _domainEventPublisher = domainEventPublisher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entities = ChangeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        if (_domainEventPublisher != null && entities.Count != 0)
        {
            await _domainEventPublisher.PublishEventsAsync(entities, cancellationToken);
        }

        return result;
    }
}
