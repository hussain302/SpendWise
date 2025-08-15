namespace Shared.Exceptions;

public class SqlDomainException(string error, Dictionary<string, string[]> Errors) : Exception
{
    public string Error { get; } = error;
    public Dictionary<string, string[]> Errors { get; } = Errors;
}