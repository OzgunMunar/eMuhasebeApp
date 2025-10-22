using eMuhasebeServer.Application.Features.Customers.Create;
using eMuhasebeServer.Application.Features.Customers.Delete;
using eMuhasebeServer.Application.Features.Customers.GetAll;
using eMuhasebeServer.Application.Features.Customers.Update;
using eMuhasebeServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace eMuhasebeServer.WebAPI.Controllers
{
    [AllowAnonymous]
    public sealed class CustomerController(IMediator mediator)
                : CrudController<
            GetAllCustomerQuery, CreateCustomerCommand, UpdateCustomerCommand, DeleteCustomerCommand,
            List<Customer>, string, string, string>(mediator)
    {

    }
}
