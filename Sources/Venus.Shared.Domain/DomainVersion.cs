namespace Venus.Shared.Domain;

public sealed class DomainVersion: IEquatable<DomainVersion>
{
    public static readonly DomainVersion New = new(1L);
    
    public DomainVersion(long value)
    {
        Value = value;
    }
    
    public long Value { get; }
    
    public static DomainVersion Create(long value)
    {
        return new(value);
    }
    
    public override string ToString()
    {
        return Value.ToString();
    }
    
    public bool Equals(DomainVersion? other)
    {
        throw new NotImplementedException();
    }
}