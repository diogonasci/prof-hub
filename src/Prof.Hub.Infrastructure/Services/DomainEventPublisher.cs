using MediatR;
using Prof.Hub.SharedKernel;

namespace Prof.Hub.Infrastructure.Services;
public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IMediator _mediator;

    public DomainEventPublisher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task PublishEventsAsync(IEnumerable<Entity> entities, CancellationToken cancellationToken = default)
    {
        var domainEvents = entities
            .SelectMany(e =>
            {
                var events = e.DomainEvents.ToList();
                e.ClearDomainEvents();
                return events;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, cancellationToken);
        }
    }
}
