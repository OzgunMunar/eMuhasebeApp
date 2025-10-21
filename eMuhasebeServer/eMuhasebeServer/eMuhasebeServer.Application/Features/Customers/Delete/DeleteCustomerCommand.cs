using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Customers.Delete
{
    public sealed record DeleteCustomerCommand(Guid Id)
        : IRequest<Result<string>>;

    internal sealed class DeleteCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWorkCompany unitOfWorkCompany)
        : IRequestHandler<DeleteCustomerCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            
            Customer? customer = await customerRepository
                .GetByExpressionWithTrackingAsync(p => p.Id  == request.Id, cancellationToken);

            if (customer == null)
            {
                return Result<string>.Failure(404, "Customer not found.");
            }

            customer.IsDeleted = false;

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Customer successfully deleted.");

        }
    }
}
