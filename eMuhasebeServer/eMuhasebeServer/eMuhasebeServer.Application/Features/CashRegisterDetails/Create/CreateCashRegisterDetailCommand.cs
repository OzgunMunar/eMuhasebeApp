using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CashRegisterDetails.Create
{
    public sealed record CreateCashRegisterDetailCommand(
        
        Guid CashRegisterId, // hangi kasa ile işlem yapıyorum
        DateOnly OpenedDate, // işlem tarihi
        int Type, // giriş mi çıkış mı: Type
        decimal Amount, 
        Guid? OppositeCashRegisterId,    // başka bir kasaya aktarımsa
        decimal OppositeAmount,    // TL'den EUR'a gönderirsem şayet!
        string Description

        )
        : IRequest<Result<string>>;

    internal sealed class CreateCashRegisterDetailCommandHandler(
        
        ICashRegisterRepository cashRegisterRepository,
        ICashRegisterDetailRepository cashRegisterDetailRepository,
        IUnitOfWorkCompany unitOfWorkCompany
        
        )
        : IRequestHandler<CreateCashRegisterDetailCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateCashRegisterDetailCommand request, CancellationToken cancellationToken)
        {

            CashRegister cashRegister = await cashRegisterRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == request.CashRegisterId, cancellationToken);

            cashRegister.CashDepositAmount += (request.Type == 0 ? request.Amount : 0);
            cashRegister.CashWithdrawalAmount += (request.Type == 1 ? request.Amount : 0);

            CashRegisterDetail cashRegisterDetail = new()
            {

                OpenedDate = request.OpenedDate,
                CashDepositAmount = request.Type == 0 ? request.Amount : 0,
                CashWithdrawalAmount = request.Type == 1 ? request.Amount : 0,
                Description = request.Description,
                CashRegisterId = request.CashRegisterId

            };

            await cashRegisterDetailRepository.AddAsync(cashRegisterDetail, cancellationToken);

            if(request.OppositeCashRegisterId is not null)
            {

                CashRegister oppositeCashRegister = await cashRegisterRepository
                    .GetByExpressionWithTrackingAsync
                    (p => p.Id == request.OppositeCashRegisterId, cancellationToken);

                oppositeCashRegister.CashDepositAmount += (request.Type == 1? request.OppositeAmount : 0);
                oppositeCashRegister.CashWithdrawalAmount += (request.Type == 0 ? request.OppositeAmount : 0);

                CashRegisterDetail oppositeCashRegisterDetail = new()
                {

                    OpenedDate = request.OpenedDate,
                    CashDepositAmount = request.Type == 1 ? request.OppositeAmount : 0,
                    CashWithdrawalAmount = request.Type == 0 ? request.OppositeAmount : 0,
                    CashRegisterDetailId = cashRegisterDetail.CashRegisterDetailId,
                    Description = request.Description,
                    CashRegisterId = (Guid)request.OppositeCashRegisterId

                };

                cashRegisterDetail.CashRegisterDetailId = oppositeCashRegisterDetail.Id;

                await cashRegisterDetailRepository.AddAsync(oppositeCashRegisterDetail, cancellationToken);

            }

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Cash Register Detail successfully created.");

        }
    }
}
