//using Microsoft.AspNetCore.Http;
//using Shared.Exceptions;
//using Shared.Messages.Errors;

//namespace Application.Middlewares;

//public class HttpResponseMiddleware(RequestDelegate next)
//{
//    private readonly RequestDelegate _next = next;

//    public async Task Invoke(HttpContext context)
//    {
//        try
//        {
//            await _next(context);

//            switch (context.Response.StatusCode)
//            {
//                case StatusCodes.Status401Unauthorized:
//                    throw new UnauthorizedException(ErrorMessages.Unauthorized);
//                case StatusCodes.Status403Forbidden:
//                    throw new ForbiddenException(ErrorMessages.Forbidden);
//                case StatusCodes.Status404NotFound:
//                    throw new NotFoundException(ErrorMessages.NotFound);
//                case StatusCodes.Status405MethodNotAllowed:
//                    throw new MethodNotAllowedException(ErrorMessages.MethodNotAllowed);
//                case StatusCodes.Status500InternalServerError:
//                    throw new InternalServerErrorException(ErrorMessages.InternalServerError);
//            }
//        }
//        catch (Exception ex)
//        {
//            await HandleExceptionAsync(context, ex);
//        }
//    }

//    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
//    {
//        Console.WriteLine($"Exception: {exception.Message}");

//        context.Response.ContentType = "application/json";
//        context.Response.StatusCode = exception switch
//        {
//            UnauthorizedException => StatusCodes.Status401Unauthorized,
//            ForbiddenException => StatusCodes.Status403Forbidden,
//            NotFoundException => StatusCodes.Status404NotFound,
//            MethodNotAllowedException => StatusCodes.Status405MethodNotAllowed,
//            InternalServerErrorException => StatusCodes.Status500InternalServerError,
//            _ => StatusCodes.Status500InternalServerError
//        };

//        return context.Response.WriteAsync(new
//        {
//            context.Response.StatusCode,
//            exception.Message
//        }.ToString() ?? "");
//    }
//}
