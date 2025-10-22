using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Enum;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.BankDetails.Create
{
    public sealed record CreateBankDetailCommand(
        
        Guid BankId, // hangi banka ile işlem yapıyorum
        DateOnly OpenedDate, // işlem tarihi
        int Type, // giriş mi çıkış mı: Type
        decimal Amount,
        Guid? OppositeBankId,    // başka bir kasaya aktarımsa
        Guid? OppositeCashRegisterId,
        Guid? OppositeCustomerId,
        decimal OppositeAmount,    // TL'den EUR'a gönderirsem, type değişirse şayet
        string Description
        )
        : IRequest<Result<string>>;

    internal sealed class CreateBankDetailCommandHandler(
        
        IBankRepository bankRepository,
        IBankDetailRepository bankDetailRepository,
        ICashRegisterRepository cashRegisterRepository,
        ICashRegisterDetailRepository cashRegisterDetailRepository,
        ICustomerDetailRepository customerDetailRepository,
        ICustomerRepository customerRepository,
        IUnitOfWorkCompany unitOfWorkCompany
        
        )
        : IRequestHandler<CreateBankDetailCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateBankDetailCommand request, CancellationToken cancellationToken)
        {

            Bank bank = await bankRepository
               .GetByExpressionWithTrackingAsync(p => p.Id == request.BankId, cancellationToken);

            bank.BankDepositAmount += (request.Type == 0 ? request.Amount : 0);
            bank.BankWithdrawalAmount += (request.Type == 1 ? request.Amount : 0);

            BankDetail bankDetail = new()
            {

                OpenedDate = request.OpenedDate,
                BankDepositAmount = request.Type == 0 ? request.Amount : 0,
                BankWithdrawalAmount = request.Type == 1 ? request.Amount : 0,
                Description = request.Description,
                BankId = request.BankId

            };

            await bankDetailRepository.AddAsync(bankDetail, cancellationToken);

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
                    BankDetailId = bankDetail.BankDetailId,
                    Description = request.Description,
                    BankId = (Guid)request.OppositeBankId

                };

                bankDetail.BankDetailId = oppositeBankDetail.Id;

                await  bankDetailRepository.AddAsync(oppositeBankDetail, cancellationToken);

            }

            if (request.OppositeCashRegisterId is not null)
            {

                CashRegister oppositeCashRegister = await cashRegisterRepository
                    .GetByExpressionWithTrackingAsync
                    (p => p.Id == request.OppositeCashRegisterId, cancellationToken);

                oppositeCashRegister.CashDepositAmount += (request.Type == 1 ? request.Amount : 0);
                oppositeCashRegister.CashWithdrawalAmount += (request.Type == 0 ? request.Amount : 0);

                CashRegisterDetail oppositeCashRegisterDetail = new()
                {

                    OpenedDate = request.OpenedDate,
                    CashDepositAmount = request.Type == 1 ? request.Amount : 0,
                    CashWithdrawalAmount = request.Type == 0 ? request.Amount : 0,
                    BankDetailId = bankDetail.Id,
                    Description = request.Description,
                    CashRegisterId = (Guid)request.OppositeCashRegisterId

                };

                bankDetail.CashRegisterDetailId = oppositeCashRegisterDetail.Id;

                await cashRegisterDetailRepository.AddAsync(oppositeCashRegisterDetail, cancellationToken);

            }

            if (request.OppositeCustomerId is not null)
            {
                Customer? customer = await customerRepository
                    .GetByExpressionWithTrackingAsync(p => p.Id == request.OppositeCustomerId, cancellationToken);

                if(customer is null)
                {
                    return Result<string>.Failure(404, "Customer not found.");
                }

                customer.DepositAmount += request.Type == 1 ? request.Amount : 0;
                customer.WithdrawalAmount += request.Type == 0 ? request.Amount : 0;

                CustomerDetail customerDetail = new()
                {
                    CustomerId = customer.Id,
                    BankDetailId = bankDetail.Id,
                    Date = request.OpenedDate,
                    Description = request.Description,
                    DepositAmount = request.Type == 1 ? request.Amount : 0,
                    WithdrawalAmount = request.Type == 0 ? request.Amount : 0,
                    Type = CustomerDetailTypeEnum.Bank
                };

                bankDetail.CustomerDetailId = customerDetail.Id;

                await customerDetailRepository.AddAsync(customerDetail, cancellationToken);

            }

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Bank Detail successfully created.");

        }
    }
}
