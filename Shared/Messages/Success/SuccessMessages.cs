namespace Shared.Messages.Success;
public static class SuccessMessages
{
    // 1xx: Informational
    public const string Continue = "The server has received the request headers, and the client should proceed to send the request body.";
    public const string SwitchingProtocols = "The requester has asked the server to switch protocols.";

    // 2xx: Success
    public const string OK = "The request has succeeded.";
    public const string Created = "The request has been fulfilled, resulting in the creation of a new resource.";
    public const string Accepted = "The request has been accepted for processing, but the processing has not been completed.";
    public const string NoContent = "The server successfully processed the request and is not returning any content.";

    // 3xx: Redirection
    public const string MovedPermanently = "The resource has been permanently moved to a new location.";
    public const string Found = "The resource is temporarily located at a different URI.";
    public const string NotModified = "The resource has not been modified since the last request.";

}
