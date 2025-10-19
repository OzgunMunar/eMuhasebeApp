using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisterDetails.Update
{
    public sealed record UpdateCashRegisterDetailCommand(

        Guid Id,
        Guid CashRegisterId, // hangi kasa ile işlem yapıyorum
        int Type, // giriş mi çıkış mı: Type
        decimal Amount,
        string Description,
        DateOnly OpenedDate
        )
        : IRequest<Result<string>>;

    internal sealed record UpdateCashRegisterDetailCommandHandler(
        
        ICashRegisterRepository cashRegisterRepository,
        ICashRegisterDetailRepository cashRegisterDetailRepository,
        IUnitOfWorkCompany unitOfWorkCompany
        
        )
        : IRequestHandler<UpdateCashRegisterDetailCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateCashRegisterDetailCommand request, CancellationToken cancellationToken)
        {

            CashRegisterDetail? cashRegisterDetail = await cashRegisterDetailRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

            if (cashRegisterDetail == null)
            {
                return Result<string>.Failure(404, "Cash Register Detail not found.");
            }

            CashRegister? cashRegister = await cashRegisterRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == cashRegisterDetail.CashRegisterId, cancellationToken);

            if (cashRegister == null)
            {
                return Result<string>.Failure(404, "Cash Register not found.");
            }

            cashRegister.CashDepositAmount -= cashRegisterDetail.CashDepositAmount;
            cashRegister.CashWithdrawalAmount -= cashRegisterDetail.CashWithdrawalAmount;

            cashRegister.CashDepositAmount += request.Type == 0 ? request.Amount : 0;
            cashRegister.CashWithdrawalAmount += request.Type == 1 ? request.Amount : 0;

            cashRegisterDetail.CashDepositAmount = request.Type == 0 ? request.Amount : 0;
            cashRegisterDetail.CashWithdrawalAmount = request.Type == 0 ? request.Amount : 0;
            cashRegisterDetail.Description = request.Description;
            cashRegisterDetail.OpenedDate = request.OpenedDate;
            
            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Cash Register Detail successfully updated.");

        }
    }
}