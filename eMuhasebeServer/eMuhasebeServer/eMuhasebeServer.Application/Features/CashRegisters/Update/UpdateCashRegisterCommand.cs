using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisters.Update
{
    public sealed record UpdateCashRegisterCommand(
        Guid Id,
        string CashRegisterName,
        int CashRegisterTypeValue)
        : IRequest<Result<string>>;

    internal sealed class CreateCashRegisterCommandHandler(
        ICashRegisterRepository cashRegisterRepository,
        IUnitOfWorkCompany unitOfWorkCompany,
        IMapper mapper)
        : IRequestHandler<UpdateCashRegisterCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateCashRegisterCommand request, CancellationToken cancellationToken)
        {

            CashRegister? cashRegister = await cashRegisterRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

            if(cashRegister == null)
            {
                return Result<string>.Failure(404, "Cash Register record not found.");
            }

            if(cashRegister.CashRegisterName != request.CashRegisterName)
            {
                
                bool isNameExists = await cashRegisterRepository
                .AnyAsync(p => p.CashRegisterName == request.CashRegisterName, cancellationToken);

                if (isNameExists)
                {
                    return Result<string>.Failure(409, "Cash Register Name already in use.");
                }

            }

            mapper.Map(request, cashRegister);

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Cash Register record successfully updated.");

        }
    }
}
