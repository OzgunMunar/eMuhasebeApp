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
        ICustomerDetailRepository customerDetailRepository,
        ICustomerRepository customerRepository,
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

            if (cashRegisterDetail.CustomerDetailId != null)
            {

                CustomerDetail? customerDetail = await customerDetailRepository
                    .GetByExpressionWithTrackingAsync(p => p.Id == cashRegisterDetail.CustomerDetailId, cancellationToken);

                if (customerDetail == null)
                {
                    return Result<string>.Failure(404, "Customer not found.");
                }

                Customer? customer = await customerRepository
                    .GetByExpressionWithTrackingAsync(p => p.Id == customerDetail.CustomerId, cancellationToken);

                if (customerDetail == null)
                {
                    return Result<string>.Failure(404, "Customer not found.");
                }

                customer.DepositAmount -= customerDetail.DepositAmount;
                customer.WithdrawalAmount -= customerDetail.WithdrawalAmount;

                customerDetailRepository.Delete(customerDetail);

            }

            cashRegisterDetailRepository.Delete(cashRegisterDetail);

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Cash Register Detail record is deleted.");

        }
    }
}
