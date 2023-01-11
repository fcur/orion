namespace Venus.Shared.Domain;

public abstract class DomainEvent
{
    protected DomainEvent(DomainVersion version, DateTimeOffset occutedAt)
    {
        ArgumentNullException.ThrowIfNull(version);
        
        Version = version;
        OccuredAt = occutedAt;
    }
    
    public DomainVersion Version { get; }
    
    public DateTimeOffset OccuredAt { get; }
}