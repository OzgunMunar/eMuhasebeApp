using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisterDetails.Delete
{
    public sealed record DeleteCashRegisterDetailById(
        Guid Id)
        : IRequest<Result<string>>;

    internal sealed class DeleteCashRegisterDetailByIdHandler(
        IBankRepository bankRepository,
        IBankDetailRepository bankDetailRepository,
        ICashRegisterRepository cashRegisterRepository,
        ICashRegisterDetailRepository cashRegisterDetailRepository,
        IUnitOfWorkCompany unitOfWorkCompany)
        : IRequestHandler<DeleteCashRegisterDetailById, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteCashRegisterDetailById request, CancellationToken cancellationToken)
        {
            
            CashRegisterDetail? cashRegisterDetail = await cashRegisterDetailRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

            if (cashRegisterDetail == null)
            {
                return Result<string>.Failure(404, "Cash Register Detail not found.");
            }

            CashRegister? cashRegister = await cashRegisterRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == cashRegisterDetail.Id, cancellationToken);

            if (cashRegister == null)
            {
                return Result<string>.Failure(404, "Cash Register not found.");
            }

            cashRegister.CashDepositAmount -= cashRegisterDetail.CashDepositAmount;
            cashRegister.CashWithdrawalAmount -= cashRegisterDetail.CashWithdrawalAmount;

            if(cashRegisterDetail.CashRegisterDetailId != null)
            {

                CashRegisterDetail? oppositeCashRegisterDetail = await cashRegisterDetailRepository
                    .GetByExpressionWithTrackingAsync(p => p.Id == cashRegisterDetail.CashRegisterDetailId, cancellationToken);

                if (oppositeCashRegisterDetail == null)
                {
                    return Result<string>.Failure(404, "Cash Register Detail not found.");
                }

                CashRegister? oppositeCashRegister = await cashRegisterRepository
                    .GetByExpressionWithTrackingAsync(p => p.Id == oppositeCashRegisterDetail.CashRegisterId, cancellationToken);

                if (cashRegister == null)
                {
                    return Result<string>.Failure(404, "Cash Register not found.");
                }

                oppositeCashRegister.CashDepositAmount -= oppositeCashRegisterDetail.CashDepositAmount;
                oppositeCashRegister.CashWithdrawalAmount -= oppositeCashRegisterDetail.CashWithdrawalAmount;

                cashRegisterDetailRepository.Delete(oppositeCashRegisterDetail);

            }

            if (cashRegisterDetail.BankDetailId != null)
            {

                BankDetail? oppositeBankDetail = await bankDetailRepository
                    .GetByExpressionWithTrackingAsync(p => p.Id == cashRegisterDetail.BankDetailId, cancellationToken);

                if (oppositeBankDetail == null)
                {
                    return Result<string>.Failure(404, "Opposite Bank Detail not found.");
                }

                Bank? oppositeBank = await bankRepository
                    .GetByExpressionWithTrackingAsync(p => p.Id == oppositeBankDetail.BankId, cancellationToken);

                if (oppositeBank == null)
                {
                    return Result<string>.Failure(404, "Opposite Bank not found.");
                }

                oppositeBank.BankDepositAmount -= oppositeBankDetail.BankDepositAmount;
                oppositeBank.BankWithdrawalAmount -= oppositeBankDetail.BankWithdrawalAmount;

                bankDetailRepository.Delete(oppositeBankDetail);

            }

            cashRegisterDetailRepository.Delete(cashRegisterDetail);

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Cash Register Detail successfully deleted.");

        }
    }
}
