namespace Prof.Hub.SharedKernel;
public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity()
    {
    }

    public List<IDomainEvent> DomainEvents => [.. _domainEvents];

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
