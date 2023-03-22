namespace Orion.App.Domain.SiriusEntity;

public sealed record FeatureId(Guid Value)
{
    public override string ToString()
    {
        return Value.ToString("D");
    }

    public static FeatureId New => new FeatureId(Guid.NewGuid());
}