using Azure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TS.Result;

namespace eMuhasebeServer.WebAPI.Abstractions
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        public readonly IMediator _mediator;
        protected ApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<IActionResult> SendRequest<T>(
            IRequest<Result<T>> request,
            CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request!, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

    }
}
