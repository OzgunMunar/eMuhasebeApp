using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Customers.Update
{
    public sealed record UpdateCustomerCommand(
        Guid Id,
        string Name,
        int TypeValue,
        string FullAddress,
        string TaxDepartment,
        string TaxNumber)
    : IRequest<Result<string>>;

    internal sealed class UpdateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWorkCompany unitOfWorkCompany,
        IMapper mapper)
        : IRequestHandler<UpdateCustomerCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {

            Customer? customer = await customerRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

            if (customer == null)
            {
                return Result<string>.Failure(404, "Customer not found.");
            }

            mapper.Map(request, customer);

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Customer updated successfully.");

        }
    }
}
