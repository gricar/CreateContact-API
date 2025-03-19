using System.Net;

namespace CreateContact.Application.Common.Exceptions;

public class ApplicationException : Exception
{
    public HttpStatusCode StatusCode { get; }

    protected ApplicationException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    : base(message)
    {
        StatusCode = statusCode;
    }
}
