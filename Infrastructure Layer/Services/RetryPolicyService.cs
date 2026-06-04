using Application_Layer.Interfaces;
using Microsoft.Extensions.Logging;
using Polly;

namespace Infrastructure_Layer.Services;

/// <summary>
/// Service för att köra operationer med retry-policy.
/// </summary>
public class RetryPolicyService : IRetryPolicyService
{
    private readonly ILogger<RetryPolicyService> _logger;

    public RetryPolicyService(
        ILogger<RetryPolicyService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Kör en operation utan returvärde med retry-logik.
    /// </summary>
    public async Task ExecuteAsync(
        Func<Task> operation,
        string operationName)
    {
        await ExecuteAsync<object?>(async () =>
        {
            await operation();
            return null;
        }, operationName);
    }

    /// <summary>
    /// Kör en operation med returvärde med retry-logik.
    /// </summary>
    public async Task<T> ExecuteAsync<T>(
        Func<Task<T>> operation,
        string operationName)
    {
        // Retry-policy: försöker 3 gånger vid exception
        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (exception, delay, attempt, _) =>
                {
                    _logger.LogWarning(
                        exception,
                        "Retry {Attempt} for {OperationName} after {DelaySeconds} seconds",
                        attempt,
                        operationName,
                        delay.TotalSeconds);
                });

        try
        {
            _logger.LogInformation(
                "Starting operation: {OperationName}",
                operationName);

            // Kör operationen med retry-policy
            var result = await policy.ExecuteAsync(operation);

            _logger.LogInformation(
                "Operation completed: {OperationName}",
                operationName);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Operation failed after retries: {OperationName}",
                operationName);

            throw;
        }
    }
}