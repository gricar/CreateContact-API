using CreateContact.Application.Contact.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace CreateContact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IMediator _dispatcher;

        public ContactsController(IMediator dispatcher)
        {
            _dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateContactCommandResponse), Status202Accepted)]
        [ProducesResponseType(Status400BadRequest)]
        [ProducesResponseType(Status409Conflict)]
        public async Task<ActionResult<CreateContactCommandResponse>> PostContact(
            [FromBody] CreateContactCommand command,
            CancellationToken cancellationToken)
        {
            var response = await _dispatcher.Send(command, cancellationToken);
            return Accepted(response);
        }
    }
}
