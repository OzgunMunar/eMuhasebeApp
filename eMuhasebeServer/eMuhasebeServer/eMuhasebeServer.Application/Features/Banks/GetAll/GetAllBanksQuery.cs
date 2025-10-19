using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Banks.GetAll
{
    public sealed record GetAllBanksQuery()
        : IRequest<Result<List<Bank>>>;

    internal sealed class GetAllBanksQueryHandler(
        IBankRepository bankRepository)
        : IRequestHandler<GetAllBanksQuery, Result<List<Bank>>>
    {
        public async Task<Result<List<Bank>>> Handle(GetAllBanksQuery request, CancellationToken cancellationToken)
        {

            List<Bank> banks = await bankRepository
                .GetAll()
                .OrderBy(p => p.BankName)
                .ToListAsync(cancellationToken);

            return banks;

        }
    }

}
