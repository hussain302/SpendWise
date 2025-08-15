namespace Shared.Exceptions;

public class InternalServerErrorException(string error) : Exception
{
    public string Error { get; } = error;
}