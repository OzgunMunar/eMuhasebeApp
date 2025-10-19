using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Banks.Create
{
    public sealed record CreateBankCommand(
        string BankName,
        string IBAN)
        : IRequest<Result<string>>;

    internal sealed class CreateBankCommandHandler(
        IBankRepository bankRepository,
        IUnitOfWorkCompany unitOfWorkCompany,
        IMapper mapper)
        : IRequestHandler<CreateBankCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateBankCommand request, CancellationToken cancellationToken)
        {

            Bank? bank = await bankRepository
                .GetByExpressionAsync(p => p.IBAN == request.IBAN, cancellationToken);

            if(bank is not null)
            {
                return Result<string>.Failure(409, "There is already another bank with the same IBAN number.");
            }

            Bank newBank = mapper.Map<Bank>(request);

            await bankRepository.AddAsync(newBank, cancellationToken);
            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Bank is created with the provided IBAN number.");

        }
    }
}
