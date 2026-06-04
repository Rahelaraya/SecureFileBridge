using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application_Layer.Interfaces;

/// <summary>
/// Service för retry-hantering vid tillfälliga fel.
/// </summary>
public interface IRetryPolicyService
{
 
    Task ExecuteAsync(
        Func<Task> operation,
        string operationName);

    Task<T> ExecuteAsync<T>(
        Func<Task<T>> operation,
        string operationName);
}