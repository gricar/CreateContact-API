namespace CreateContact.Application.Common.Messaging.Events;

public record ContactCreatedEvent(
    string Name,
    int DDDCode,
    string Phone,
    string? Email
    );
