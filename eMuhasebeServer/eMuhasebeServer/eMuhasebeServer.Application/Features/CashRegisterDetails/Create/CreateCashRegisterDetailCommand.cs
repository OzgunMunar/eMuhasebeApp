using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Enum;
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
        Guid? OppositeBankId,
        Guid? OppositeCustomerId,
        decimal OppositeAmount,    // TL'den EUR'a gönderirsem şayet!
        string Description

        )
        : IRequest<Result<string>>;

    internal sealed class CreateCashRegisterDetailCommandHandler(
        
        ICustomerRepository customerRepository,
        ICustomerDetailRepository customerDetailRepository,
        ICashRegisterRepository cashRegisterRepository,
        ICashRegisterDetailRepository cashRegisterDetailRepository,
        IBankRepository bankRepository,
        IBankDetailRepository bankDetailRepository,
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
                    CashRegisterDetailId = cashRegisterDetail.Id,
                    Description = request.Description,
                    CashRegisterId = (Guid)request.OppositeCashRegisterId

                };

                cashRegisterDetail.CashRegisterDetailId = oppositeCashRegisterDetail.Id;

                await cashRegisterDetailRepository.AddAsync(oppositeCashRegisterDetail, cancellationToken);

            }

            if (request.OppositeBankId is not null)
            {

                Bank oppositeBank = await bankRepository
                    .GetByExpressionWithTrackingAsync
                    (p => p.Id == request.OppositeBankId, cancellationToken);

                oppositeBank.BankDepositAmount += (request.Type == 1 ? request.OppositeAmount : 0);
                oppositeBank.BankWithdrawalAmount += (request.Type == 0 ? request.OppositeAmount : 0);

                BankDetail oppositeBankDetail = new()
                {

                    OpenedDate = request.OpenedDate,
                    BankDepositAmount = request.Type == 1 ? request.OppositeAmount : 0,
                    BankWithdrawalAmount = request.Type == 0 ? request.OppositeAmount : 0,
                    CashRegisterDetailId = cashRegisterDetail.Id,
                    Description = request.Description,
                    BankId = (Guid)request.OppositeBankId

                };

                cashRegisterDetail.BankDetailId = oppositeBankDetail.Id;

                await bankDetailRepository.AddAsync(oppositeBankDetail, cancellationToken);

            }

            if (request.OppositeCustomerId is not null)
            {
                Customer? customer = await customerRepository
                    .GetByExpressionWithTrackingAsync(p => p.Id == request.OppositeCustomerId, cancellationToken);

                if (customer is null)
                {
                    return Result<string>.Failure(404, "Customer not found.");
                }

                customer.DepositAmount += request.Type == 1 ? request.Amount : 0;
                customer.WithdrawalAmount += request.Type == 0 ? request.Amount : 0;

                CustomerDetail customerDetail = new()
                {
                    CustomerId = customer.Id,
                    CashRegisterDetailId = cashRegisterDetail.Id,
                    Date = request.OpenedDate,
                    Description = request.Description,
                    DepositAmount = request.Type == 1 ? request.Amount : 0,
                    WithdrawalAmount = request.Type == 0 ? request.Amount : 0,
                    Type = CustomerDetailTypeEnum.CashRegister
                };

                cashRegisterDetail.CustomerDetailId = customerDetail.Id;

                await customerDetailRepository.AddAsync(customerDetail, cancellationToken);

            }

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Cash Register Detail successfully created.");

        }
    }
}
