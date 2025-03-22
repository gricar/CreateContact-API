using CreateContact.Application.Common.Exceptions;
using Newtonsoft.Json;
using System.Net;
using ApplicationException = CreateContact.Application.Common.Exceptions.ApplicationException;

namespace CreateContact.API.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = ex switch
        {
            BadRequestException => HttpStatusCode.BadRequest,
            DuplicateContactException => HttpStatusCode.Conflict,
            DuplicateEmailException => HttpStatusCode.Conflict,
            ApplicationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError,
        };

        var response = new
        {
            statusCode = (int)statusCode,
            error = ex.Message,
            details = ex is BadRequestException badRequestException
                ? badRequestException.ErrorDetails
                : null
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
