using CreateContact.Application.Common.Exceptions;
using CreateContact.Application.Common.Messaging;
using CreateContact.Application.Common.Messaging.Events;
using CreateContact.Application.Persistence;
using MediatR;

namespace CreateContact.Application.Contact.Commands.Create;

public class CreateContactCommandHandler(
    IContactRepository _contactRepository, IEventBus eventBus)
    : IRequestHandler<CreateContactCommand, CreateContactCommandResponse>
{
    public async Task<CreateContactCommandResponse> Handle(CreateContactCommand command, CancellationToken cancellationToken)
    {
        await EnsureContactIsUniqueAsync(command);

        var contact = Domain.Entities.Contact.Create(
            command.Name, command.DDDCode, command.Phone, command.Email);

        var contactEvent = new ContactCreatedEvent(
            contact.Name,
            contact.Region.DddCode,
            contact.Phone,
            contact.Email
        );

        await eventBus.PublishAsync(contactEvent, "contact-created");

        return new CreateContactCommandResponse("Contact creation request accepted and is being processed.");
    }

    private async Task EnsureContactIsUniqueAsync(CreateContactCommand command)
    {
        await CheckForUniqueContactAsync(command.DDDCode, command.Phone);

        if (!string.IsNullOrEmpty(command.Email))
        {
            await CheckForUniqueEmailAsync(command.Email);
        }
    }

    private async Task CheckForUniqueEmailAsync(string email)
    {
        if (await _contactRepository.IsEmailUniqueAsync(email))
        {
            throw new DuplicateEmailException(email!);
        }
    }

    private async Task CheckForUniqueContactAsync(int dddCode, string phone)
    {
        if (await _contactRepository.IsDddAndPhoneUniqueAsync(dddCode, phone))
        {
            throw new DuplicateContactException(dddCode, phone);
        }
    }
}
