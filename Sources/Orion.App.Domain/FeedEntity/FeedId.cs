namespace Orion.App.Domain.FeedEntity;

public sealed record FeedId(Guid Id, string ProviderId)
{
    public override string ToString()
    {
        return Id.ToString("D");
    }
}