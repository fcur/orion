using Venus.Shared.Domain;

namespace Orion.App.Domain.SiriusEntity;

public sealed record FeatureValidationError: DomainError
{
    public FeatureValidationError(string errorType, string errorMessage) :
        base(errorType, errorMessage)
    {
    }

    public FeatureValidationError(string errorType, string errorMessage, Dictionary<string, object> data) 
        : base(errorType, errorMessage, data)
    {
    }

    public static FeatureValidationError InvalidStartTime => 
        new("InvalidStartTime", "Start time should be greater than the current time.");

    public static FeatureValidationError NoChanges => 
        new("NoChanges", "There are no changes");
}