using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.BankDetails.GetAll
{
    public sealed record GetAllBankDetailsQuery(
        Guid BankId,
        DateOnly StartDate,
        DateOnly EndDate)
        : IRequest<Result<Bank>>;

    internal sealed class GetAllBankDetailsQueryHandler(
        IBankRepository bankRepository)
        : IRequestHandler<GetAllBankDetailsQuery, Result<Bank>>
    {
        public async Task<Result<Bank>> Handle(GetAllBankDetailsQuery request, CancellationToken cancellationToken)
        {

            Bank? bank = await bankRepository
                .Where(p => p.Id == request.BankId)
                .Include(p => p.BankDetails!.Where(
                    p =>
                    p.OpenedDate >= request.StartDate
                    &&
                    p.OpenedDate <= request.EndDate))
                .FirstOrDefaultAsync(cancellationToken);

            if (bank == null)
            {
                return Result<Bank>.Failure(404, "Bank not found.");
            }

            return bank;

        }
    }
}
