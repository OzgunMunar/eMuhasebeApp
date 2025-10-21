using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.BankDetails.Update
{
    public sealed record UpdateBankDetailCommand(

        Guid Id,
        Guid BankId, // hangi kasa ile işlem yapıyorum
        int Type, // giriş mi çıkış mı: Type
        decimal Amount,
        string Description,
        DateOnly OpenedDate
        )
        : IRequest<Result<string>>;

    internal sealed class UpdateBankDetailCommandHandler(
        IBankRepository bankRepository,
        IBankDetailRepository bankDetailRepository,
        IUnitOfWorkCompany unitOfWorkCompany)
        : IRequestHandler<UpdateBankDetailCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateBankDetailCommand request, CancellationToken cancellationToken)
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

            bank.BankDepositAmount += request.Type == 0 ? request.Amount : 0;
            bank.BankWithdrawalAmount += request.Type == 1 ? request.Amount : 0;

            bankDetail.BankDepositAmount = request.Type == 0 ? request.Amount : 0;
            bankDetail.BankWithdrawalAmount = request.Type == 0 ? request.Amount : 0;
            bankDetail.Description = request.Description;
            bankDetail.OpenedDate = request.OpenedDate;

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Bank Detail successfully updated.");

        }
    }
}
