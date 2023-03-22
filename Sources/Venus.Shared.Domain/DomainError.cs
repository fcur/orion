namespace Venus.Shared.Domain;

public abstract record DomainError
{
    protected DomainError(string errorType, string errorMessage)
    {
        ErrorType = errorType;
        ErrorMessage = errorMessage;
    }

    protected DomainError(string errorType, string errorMessage, Dictionary<string, object>  data)
    {
        ErrorType = errorType;
        ErrorMessage = errorMessage;
        Data = data;
    }
    
    public string ErrorType { get; }
    public string ErrorMessage { get; }
    public Dictionary<string, object> Data { get; } = new();
}
