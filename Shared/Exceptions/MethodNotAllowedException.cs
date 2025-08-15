namespace Shared.Exceptions;

public class MethodNotAllowedException(string error) : Exception
{
    public string Error { get; } = error;
}