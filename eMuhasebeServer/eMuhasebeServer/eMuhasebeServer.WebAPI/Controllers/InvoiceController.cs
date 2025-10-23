using eMuhasebeServer.Application.Features.Invoices.Create;
using eMuhasebeServer.Application.Features.Invoices.Delete;
using eMuhasebeServer.Application.Features.Invoices.GetAll;
using eMuhasebeServer.Application.Features.Invoices.Update;
using eMuhasebeServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace eMuhasebeServer.WebAPI.Controllers
{
    [AllowAnonymous]
    public sealed class InvoiceController(IMediator mediator)
                : CrudController<
            GetAllInvoicesQuery, CreateInvoiceCommand, UpdateInvoiceCommand, DeleteInvoiceCommand,
            List<Invoice>, string, string, string>(mediator)
    {

    }
}
