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
        if (other == null)
        {
            return false;
        }

        if (this == other)
        {
            return true;
        }
        
        return EqualityComparer<TId>.Default.Equals(Id, other!.Id);
    }

    public override bool Equals(object? other)
    {
        if (other == null)
        {
            return false;
        }
        
        if (other!.GetType() == GetType())
        {
            return Equals((DomainEntity<TId>)other);
        }
        
        return false;
    }
    
    public override int GetHashCode()
    {
        return EqualityComparer<TId>.Default.GetHashCode(Id!);
    }

    protected void AddEvent(DomainEvent domainEvent)
    {
        DomainEvents.Add(domainEvent);
    }

    public IReadOnlyCollection<DomainEvent> GrabEvents()
    {
        var result = DomainEvents.ToArray();
        DomainEvents.Clear();
        return result;
    }
    
    protected void IncrementVersion()
    {
        Version =  DomainVersion.Create(Version.Value + 1L);
    }
    
    public static bool operator ==(DomainEntity<TId>? a, DomainEntity<TId>? b)
    {
        return Equals(a, b);
    }

    public static bool operator !=(DomainEntity<TId>? a, DomainEntity<TId>? b)
    {
        return !Equals(a, b);
    }
}