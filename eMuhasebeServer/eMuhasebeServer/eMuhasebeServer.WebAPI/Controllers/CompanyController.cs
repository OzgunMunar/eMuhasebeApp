using eMuhasebeServer.Application.Features.Companies.Create;
using eMuhasebeServer.Application.Features.Companies.Delete;
using eMuhasebeServer.Application.Features.Companies.GetAllCompanies;
using eMuhasebeServer.Application.Features.Companies.MigrateAllCompanies;
using eMuhasebeServer.Application.Features.Companies.Update;
using eMuhasebeServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eMuhasebeServer.WebAPI.Controllers
{
    [AllowAnonymous]
    public sealed class CompanyController(IMediator mediator)
                : CrudController<
            GetAllCompaniesQuery, CreateCompanyCommand, UpdateCompanyCommand, DeleteCompanyByIdCommand,
            List<Company>, string, string, string>(mediator)
    {
        [HttpPost]
        public async Task<IActionResult> MigrateAll(MigrateAllCompaniesCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

    }
}
