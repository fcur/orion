namespace Venus.Shared.Domain;

public abstract class DomainEntity<TId> : IEquatable<DomainEntity<TId>>
{ 
    protected DomainEntity(TId id, DomainVersion version)
    {
        ArgumentNullException.ThrowIfNull(id);
        ArgumentNullException.ThrowIfNull(version);

        Id = id;
        Version = version;
        DomainEvents = new List<DomainEvent>();
    }
    
    public TId Id { get; }

    public DomainVersion Version { get; private set; }

    private List<DomainEvent> DomainEvents { get; }

    
    public bool Equals(DomainEntity<TId>? other)
    {
        throw new NotImplementedException();
    }
    
    public void AddEvent(DomainEvent domainEvent)
    {
        DomainEvents.Add(domainEvent);
    }

    public IReadOnlyCollection<DomainEvent> GrabEvents()
    {
        var result = DomainEvents.ToArray();
        DomainEvents.Clear();
        return result;
    }
    
    protected DomainVersion NextVersion => DomainVersion.Create(Version.Value + 1);
    
    protected void IncrementVersion()
    {
        Version = NextVersion;
    }
}