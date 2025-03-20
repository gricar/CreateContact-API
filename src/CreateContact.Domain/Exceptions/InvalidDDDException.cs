namespace CreateContact.Domain.Exceptions;

public class InvalidDDDException : DomainException
{
    public InvalidDDDException(int dddCode)
        : base($"DDD code '{dddCode}' is invalid or does not belong to any region.")
    { }
}
