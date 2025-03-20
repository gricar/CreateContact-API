namespace CreateContact.Domain.Exceptions;

public class InvalidPhoneNumberException : DomainException
{
    public InvalidPhoneNumberException(string phone)
        : base($"Phone '{phone}' must be a 9-digit number.")
    { }
}