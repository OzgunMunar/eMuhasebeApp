using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.BankDetails.Delete
{
    public sealed record DeleteBankDetailCommand(
    Guid Id)
    : IRequest<Result<string>>;

    internal sealed class DeleteBankDetailCommandHandler(
        IBankRepository bankRepository,
        IBankDetailRepository bankDetailRepository,
        ICashRegisterRepository cashRegisterRepository,
        ICashRegisterDetailRepository cashRegisterDetailRepository, 
        IUnitOfWorkCompany unitOfWorkCompany)
        : IRequestHandler<DeleteBankDetailCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteBankDetailCommand request, CancellationToken cancellationToken)
        {

            BankDetail? bankDetail = await bankDetailRepository
               .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

            if (bankDetail == null)
            {
                return Result<string>.Failure(404, "Bank Detail not found.");
            }

            Bank? bank = await bankRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == bankDetail.BankId, cancellationToken);

            if (bank == null)
            {
                return Result<string>.Failure(404, "Bank not found.");
            }

            bank.BankDepositAmount -= bankDetail.BankDepositAmount;
            bank.BankWithdrawalAmount -= bankDetail.BankWithdrawalAmount;

            if (bankDetail.BankDetailId != null)
            {

                BankDetail? oppositeBankDetail = await bankDetailRepository
                    .GetByExpressionWithTrackingAsync(p => p.Id == bankDetail.BankDetailId, cancellationToken);

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

            if (bankDetail.CashRegisterDetailId != null)
            {

                CashRegisterDetail? oppositeCashRegisterDetail = await cashRegisterDetailRepository
                    .GetByExpressionWithTrackingAsync(p => p.Id == bankDetail.CashRegisterDetailId, cancellationToken);

                if (oppositeCashRegisterDetail == null)
                {
                    return Result<string>.Failure(404, "Cash Register Detail not found.");
                }

                CashRegister? oppositeCashRegister = await cashRegisterRepository
                    .GetByExpressionWithTrackingAsync(p => p.Id == oppositeCashRegisterDetail.CashRegisterId, cancellationToken);

                if (oppositeCashRegister == null)
                {
                    return Result<string>.Failure(404, "Cash Register not found.");
                }

                oppositeCashRegister.CashDepositAmount -= oppositeCashRegisterDetail.CashDepositAmount;
                oppositeCashRegister.CashWithdrawalAmount -= oppositeCashRegisterDetail.CashWithdrawalAmount;

                cashRegisterDetailRepository.Delete(oppositeCashRegisterDetail);

            }

            bankDetailRepository.Delete(bankDetail);

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Bank Detail successfully deleted.");

        }
    }
}
