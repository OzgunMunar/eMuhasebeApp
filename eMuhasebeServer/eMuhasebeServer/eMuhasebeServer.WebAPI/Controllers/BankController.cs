using eMuhasebeServer.Application.Features.Banks.Create;
using eMuhasebeServer.Application.Features.Banks.Delete;
using eMuhasebeServer.Application.Features.Banks.GetAll;
using eMuhasebeServer.Application.Features.Banks.Update;
using eMuhasebeServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace eMuhasebeServer.WebAPI.Controllers
{

    [AllowAnonymous]
    public sealed class BankController(IMediator mediator)
            : CrudController<
        GetAllBanksQuery, CreateBankCommand, UpdateBankCommand, DeleteBankCommand,
        List<Bank>, string, string, string>(mediator)
    {

    }

}
