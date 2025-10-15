using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisters.Delete
{
    public sealed record DeleteCashRegisterCommand(
        Guid Id
        ) : IRequest<Result<string>>;

    internal sealed record UpdateCashRegisterCommandHandler(
        ICashRegisterRepository cashRegisterRepository,
        IUnitOfWorkCompany unitOfWorkCompany,
        IMapper Mapper)
        : IRequestHandler<DeleteCashRegisterCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteCashRegisterCommand request, CancellationToken cancellationToken)
        {

            CashRegister? cashRegister = await cashRegisterRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

            if ( cashRegister is null )
            {
                return Result<string>.Failure(404, "Cash Register record not found.");
            }

            cashRegister.IsDeleted = true;

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Cash Register record is deleted.");

        }
    }
}
