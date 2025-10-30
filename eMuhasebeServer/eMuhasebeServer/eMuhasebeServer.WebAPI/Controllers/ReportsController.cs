using eMuhasebeServer.Application.Features.Auth.ChangeCompany;
using eMuhasebeServer.Application.Features.Auth.Login;
using eMuhasebeServer.Application.Features.Reports.ProductProfitabilityReports;
using eMuhasebeServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMuhasebeServer.WebAPI.Controllers
{
    [AllowAnonymous]
    public sealed class ReportsController(IMediator mediator) : ApiController(mediator)
    {

        [HttpGet]
        public async Task<IActionResult> ProductProfitabilityReport(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new ProductProfitabilityReportsQuery(), cancellationToken);
            return StatusCode(response.StatusCode, response);
        }
    }
}