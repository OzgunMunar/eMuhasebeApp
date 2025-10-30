using eMuhasebeServer.Application.Features.Reports.ProductProfitabilityReports;
using eMuhasebeServer.Application.Features.Reports.PurchaseReports;
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

        [HttpGet]
        public async Task<IActionResult> PurchaseReports(CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(new PurchaseReportsQuery(), cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

    }
}