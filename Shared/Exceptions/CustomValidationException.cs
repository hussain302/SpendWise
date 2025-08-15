namespace Shared.Exceptions;
public class CustomValidationException : Exception
{
    public IEnumerable<string> Errors { get; }

    public CustomValidationException(IEnumerable<string> errors)
        : base(string.Join("; ", errors))
    {
        Errors = errors;
    }
}
