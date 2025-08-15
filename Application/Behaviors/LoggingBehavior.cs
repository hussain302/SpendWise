using MediatR;
using Serilog;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Application.Behaviors;
public class LoggingBehavior<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var stopwatch = Stopwatch.StartNew();

        var httpContext = httpContextAccessor.HttpContext;
        var user = httpContext?.User.Identity?.Name ?? "Anonymous";
        var ipAddress = httpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

        var requestJson = JsonSerializer.Serialize(request, new JsonSerializerOptions { WriteIndented = true });

        Log.Information("➡️ [START] Handling {RequestName} | User: {User} | IP: {IpAddress} | Request Data: {RequestJson}",
            requestName, user, ipAddress, requestJson);

        try
        {
            var response = await next();
            stopwatch.Stop();

            var responseJson = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });

            if (stopwatch.ElapsedMilliseconds > 2000) // Customize threshold
                Log.Warning("⚠️ [SLOW] {RequestName} took {ElapsedMilliseconds}ms", requestName, stopwatch.ElapsedMilliseconds);

            Log.Information("✅ [END] {RequestName} | Execution Time: {ElapsedMilliseconds}ms | Response Data: {ResponseJson}",
                requestName, stopwatch.ElapsedMilliseconds, responseJson);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            Log.Error(ex, "❌ [ERROR] {RequestName} failed after {ElapsedMilliseconds}ms | Error: {ErrorMessage}",
                requestName, stopwatch.ElapsedMilliseconds, ex.Message);

            throw; // Re-throw to ensure proper error handling
        }
    }
}
