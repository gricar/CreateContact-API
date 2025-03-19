using CreateContact.Application.Common.Exceptions;
using FluentValidation;
using MediatR;

namespace CreateContact.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) =>
        _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any()) return await next();

        var context = new ValidationContext<TRequest>(request);


        var validationResults = await Task.WhenAll(
                _validators.Select(validator => validator.ValidateAsync(context, cancellationToken))
            );

        var errors = validationResults
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure != null)
            .Select(failure => failure.ErrorMessage)
            .Distinct()
            .ToArray();

        if (errors.Any())
            throw new BadRequestException(errors);

        return await next();
    }
}

