using CreateContact.Application.Common.Messaging;
using CreateContact.Application.Common.Messaging.Events;
using MediatR;

namespace CreateContact.Application.Contact.Commands.Create;

public class CreateContactCommandHandler(IEventBus eventBus) : IRequestHandler<CreateContactCommand, CreateContactCommandResponse>
{
    public async Task<CreateContactCommandResponse> Handle(CreateContactCommand command, CancellationToken cancellationToken)
    {
        var contact = new ContactCreatedEvent(
            command.Name,
            command.DDDCode,
            command.Phone,
            command.Email
        );

        await eventBus.PublishAsync(contact, "contact-created");

        return new CreateContactCommandResponse("Contact successfully submitted!");
    }
}
