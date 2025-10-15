using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisters.GetAllCashRegisters
{
    public sealed record GetAllCashRegistersQuery()
        : IRequest<Result<List<CashRegister>>>;

    internal sealed class GetAllCashRegistersQueryHandler(
        ICashRegisterRepository cashRegisterRepository)
        : IRequestHandler<GetAllCashRegistersQuery, Result<List<CashRegister>>>
    {
        public async Task<Result<List<CashRegister>>> Handle(GetAllCashRegistersQuery request, CancellationToken cancellationToken)
        {

            List<CashRegister> cashRegisters = await cashRegisterRepository
                .GetAll()
                .OrderBy(p => p.CashRegisterName)
                .ToListAsync(cancellationToken);

            return cashRegisters;

        }
    }
}
