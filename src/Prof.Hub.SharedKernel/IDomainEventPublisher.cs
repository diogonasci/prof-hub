namespace Prof.Hub.SharedKernel;
public interface IDomainEventPublisher
{
    Task PublishEventsAsync(IEnumerable<Entity> entities, CancellationToken cancellationToken = default);
}
