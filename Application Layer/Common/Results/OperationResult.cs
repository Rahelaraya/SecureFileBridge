namespace Application_Layer.Common.Results;

/// <summary>
/// Standardiserad klass för att returnera resultat från services och handlers.
/// </summary>
public class OperationResult<T>
{
   
    public bool Success { get; init; }

    public string? Message { get; init; }

    public T? Data { get; init; }

    public List<string> Errors { get; init; } = [];

    public static OperationResult<T> Ok(
        T data,
        string? message = null)
    {
        return new()
        {
            Success = true,
            Data = data,
            Message = message
        };
    }
    public static OperationResult<T> Fail(
        params string[] errors)
    {
        return new()
        {
            Success = false,
            Errors = errors.ToList()
        };
    }
}