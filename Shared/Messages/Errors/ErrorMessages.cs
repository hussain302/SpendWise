namespace Shared.Messages.Errors;
public static class ErrorMessages
{
    // 4xx: Client Errors
    public const string BadRequest = "The server could not understand the request due to invalid parameters.";
    public const string Unauthorized = "The client must authenticate itself to get the requested response.";
    public const string Forbidden = "The client does not have access rights to the content.";
    public const string NotFound = "The server can not find the requested resource.";
    public const string MethodNotAllowed = "The method is not allowed for the requested URL.";
    public const string Conflict = "The request conflicts with the current state of the server.";
    public const string Gone = "The resource requested is no longer available.";
    public const string UnsupportedMediaType = "The media format of the requested data is not supported by the server.";
    public const string UnprocessableEntity = "The request was well-formed but was unable to be followed due to semantic errors.";
    public const string TooManyRequests = "The user has sent too many requests in a given amount of time.";

    // 5xx: Server Errors
    public const string InternalServerError = "The server has encountered a situation it doesn't know how to handle.";
    public const string NotImplemented = "The server does not support the functionality required to fulfill the request.";
    public const string BadGateway = "The server received an invalid response from the upstream server.";
    public const string ServiceUnavailable = "The server is not ready to handle the request.";
    public const string GatewayTimeout = "The server is acting as a gateway and cannot get a response in time.";
}