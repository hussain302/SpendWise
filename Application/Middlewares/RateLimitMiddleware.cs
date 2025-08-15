using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Shared.Exceptions;
using Shared.Messages.Errors;

namespace Application.Middlewares;

public class RateLimitMiddleware(RequestDelegate next,
    IMemoryCache memoryCache,
    ICurrentUser currentUser)
{
    private readonly TimeSpan timeLimit = TimeSpan.FromMinutes(5);
    private readonly int countLimit = 200;

    public async Task Invoke(HttpContext context)
    {
        var key = currentUser.IPAddress;

        memoryCache.TryGetValue(key, out int requestCount);

        if (requestCount > countLimit)
            throw new TooManyRequestException(ErrorMessages.TooManyRequests);
        else
        {
            await next(context);
        }

        requestCount++;
        memoryCache.Set(key, requestCount, timeLimit);
    }
}