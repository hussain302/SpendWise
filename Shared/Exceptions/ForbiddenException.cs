namespace Shared.Exceptions;

public class ForbiddenException(string error) : Exception
{
    public string Error { get; } = error;
}