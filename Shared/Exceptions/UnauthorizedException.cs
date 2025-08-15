namespace Shared.Exceptions;

public class UnauthorizedException(string error) : Exception
{
    public string Error { get; } = error;
}