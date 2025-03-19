namespace CreateContact.Application.Common.Exceptions;

public class BadRequestException : ApplicationException
{
    public BadRequestException(string[] errorDetails)
    : base("Validation errors occurred. See error details.")
    {
        ErrorDetails = errorDetails;
    }

    public string[] ErrorDetails { get; }
}
