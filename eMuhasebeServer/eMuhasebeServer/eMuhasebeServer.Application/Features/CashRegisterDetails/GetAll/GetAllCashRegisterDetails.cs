using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisterDetails.GetAll
{
    public sealed record GetAllCashRegisterDetailsQuery(
        Guid CashRegisterId,
        DateOnly StartDate,
        DateOnly EndDate
        )
        : IRequest<Result<CashRegister>>;

    internal sealed class GetAllCashRegisterDetailsQueryHandler(
        ICashRegisterRepository cashRegisterRepository)
        : IRequestHandler<GetAllCashRegisterDetailsQuery, Result<CashRegister>>
    {
        public async Task<Result<CashRegister>> Handle(GetAllCashRegisterDetailsQuery request, CancellationToken cancellationToken)
        {
            
            CashRegister? cashRegister = await cashRegisterRepository
                .Where(p => p.Id == request.CashRegisterId)
                .Include(p => p.CashRegisterDetails!.Where(
                    p => 
                    p.OpenedDate >= request.StartDate
                    &&
                    p.OpenedDate <= request.EndDate))
                .FirstOrDefaultAsync(cancellationToken);

            if(cashRegister == null)
            {
                return Result<CashRegister>.Failure(404, "Cash Register not found.");
            }

            return cashRegister;

        }
    }

}
