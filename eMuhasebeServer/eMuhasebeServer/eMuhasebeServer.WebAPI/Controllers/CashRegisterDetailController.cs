using eMuhasebeServer.Application.Features.CashRegisterDetails.Create;
using eMuhasebeServer.Application.Features.CashRegisterDetails.GetAll;
using eMuhasebeServer.Application.Features.CashRegisterDetails.Update;
using eMuhasebeServer.Application.Features.CashRegisters.Delete;
using eMuhasebeServer.Domain.Entities;
using MediatR;

namespace eMuhasebeServer.WebAPI.Controllers
{
    public sealed class CashRegisterDetailController(IMediator mediator)
        : CrudController<
        GetAllCashRegisterDetailsQuery, CreateCashRegisterDetailCommand, UpdateCashRegisterDetailCommand, DeleteCashRegisterDetailCommand,
        CashRegister, string, string, string>(mediator)
    {
    }

}
