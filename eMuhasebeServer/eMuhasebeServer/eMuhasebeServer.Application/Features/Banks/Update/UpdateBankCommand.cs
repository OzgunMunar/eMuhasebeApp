using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Banks.Update
{
    public sealed record UpdateBankCommand(
        Guid Id,
        string BankName,
        string IBAN)
        : IRequest<Result<string>>;

    internal sealed class UpdateBankCommandHandler(
        IBankRepository bankRepository,
        IUnitOfWorkCompany unitOfWorkCompany,
        IMapper mapper)
        : IRequestHandler<UpdateBankCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateBankCommand request, CancellationToken cancellationToken)
        {

            Bank? bank = await bankRepository
                .GetByExpressionAsync(p => p.Id == request.Id, cancellationToken);

            if(bank is null)
            {
                return Result<string>.Failure(404, "Bank not found.");
            }

            if(bank.IBAN != request.IBAN)
            {
                bool isIbanExist = await bankRepository
                    .AnyAsync(p => p.IBAN == request.IBAN, cancellationToken);

                if (isIbanExist)
                {
                    return Result<string>.Failure(409, "There is already another bank with the same IBAN number.");
                }
            }

            mapper.Map(request, bank);

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Bank successfully updated.");

        }
    }
}
