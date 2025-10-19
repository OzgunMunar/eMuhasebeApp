using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisters.Delete
{
    public sealed record DeleteCashRegisterDetailCommand(
        Guid Id
        ) : IRequest<Result<string>>;

    internal sealed record DeleteCashRegisterDetailCommandHandler(
        ICashRegisterDetailRepository cashRegisterDetailRepository,
        IUnitOfWorkCompany unitOfWorkCompany,
        IMapper Mapper)
        : IRequestHandler<DeleteCashRegisterDetailCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteCashRegisterDetailCommand request, CancellationToken cancellationToken)
        {

            CashRegisterDetail? cashRegisterDetail = await cashRegisterDetailRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

            if (cashRegisterDetail is null)
            {
                return Result<string>.Failure(404, "Cash Register Detail record not found.");
            }

            cashRegisterDetail.IsDeleted= true;

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Cash Register Detail record is deleted.");

        }
    }
}
