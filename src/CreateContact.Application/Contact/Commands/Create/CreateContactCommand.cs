using MediatR;

namespace CreateContact.Application.Contact.Commands.Create;

public sealed record CreateContactCommand(
        string Name,
        int DDDCode,
        string Phone,
        string? Email = null
        ) : IRequest<CreateContactCommandResponse>;