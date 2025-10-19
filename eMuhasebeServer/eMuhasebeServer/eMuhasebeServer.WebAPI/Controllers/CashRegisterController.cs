using eMuhasebeServer.Application.Features.CashRegisters.Create;
using eMuhasebeServer.Application.Features.CashRegisters.Delete;
using eMuhasebeServer.Application.Features.CashRegisters.GetAllCashRegisters;
using eMuhasebeServer.Application.Features.CashRegisters.Update;
using eMuhasebeServer.Domain.Entities;
using MediatR;

namespace eMuhasebeServer.WebAPI.Controllers
{
    public sealed class CashRegisterController(IMediator mediator)
                : CrudController<
            GetAllCashRegistersQuery, CreateCashRegisterCommand, UpdateCashRegisterCommand, DeleteCashRegisterDetailCommand,
            List<CashRegister>, string, string, string>(mediator)
    {
    }
}
