using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisters.Create
{
    public sealed record CreateCashRegisterCommand(
        string CashRegisterName,
        int CurrencyTypeValue)
        : IRequest<Result<string>>;

    internal sealed class CreateCashRegisterCommandHandler(
        ICashRegisterRepository cashRegisterRepository,
        IUnitOfWorkCompany unitOfWorkCompany,
        IMapper mapper)
        : IRequestHandler<CreateCashRegisterCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateCashRegisterCommand request, CancellationToken cancellationToken)
        {
            
            bool isNameExists = await cashRegisterRepository
                .AnyAsync(p => p.CashRegisterName == request.CashRegisterName, cancellationToken);

            if(isNameExists)
            {
                return Result<string>.Failure(409, "Cash Register Name already in use.");
            }

            CashRegister cashRegister = mapper.Map<CashRegister>(request);

            await cashRegisterRepository.AddAsync(cashRegister, cancellationToken);
            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Cash Register successfully created.");

        }
    }
}
