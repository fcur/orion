using Venus.Shared.Domain;

namespace Orion.App.Domain.FeedEntity;

public sealed record FeedValidationError: DomainError
{
    public FeedValidationError(string errorType, string errorMessage) :
        base(errorType, errorMessage)
    {
    }

    public FeedValidationError(string errorType, string errorMessage, Dictionary<string, object> data) 
        : base(errorType, errorMessage, data)
    {
    }

    public static FeedValidationError InvalidStartTime =
        new("InvalidStartTime", "Start time should be greater than the current time.");
}