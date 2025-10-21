using eMuhasebeServer.Application.Features.BankDetails.Create;
using eMuhasebeServer.Application.Features.BankDetails.Delete;
using eMuhasebeServer.Application.Features.BankDetails.GetAll;
using eMuhasebeServer.Application.Features.BankDetails.Update;
using eMuhasebeServer.Domain.Entities;
using MediatR;

namespace eMuhasebeServer.WebAPI.Controllers
{
    public sealed class BankDetailController(IMediator mediator)
        : CrudController<
        GetAllBankDetailsQuery, CreateBankDetailCommand, UpdateBankDetailCommand, DeleteBankDetailCommand,
        Bank, string, string, string>(mediator)
    {
    }

}
