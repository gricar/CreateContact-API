using System.Net;

namespace CreateContact.Application.Common.Exceptions;

public class DuplicateContactException : ApplicationException
{
    public DuplicateContactException(int dddCode, string phone)
        : base($"A contact with the same DDD '{dddCode}' and phone '{phone}' already exists.",
            HttpStatusCode.Conflict)
    { }
}
