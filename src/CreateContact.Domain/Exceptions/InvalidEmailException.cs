namespace CreateContact.Domain.Exceptions;

public class InvalidEmailException : DomainException
{
    public InvalidEmailException(string email)
        : base($"Email '{email}' must be a valid format.")
    { }
}