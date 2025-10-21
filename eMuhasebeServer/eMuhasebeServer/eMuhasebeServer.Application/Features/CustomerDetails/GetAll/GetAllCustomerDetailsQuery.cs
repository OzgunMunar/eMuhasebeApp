using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.CustomerDetails.GetAll
{
    public sealed record GetAllCustomerDetailsQuery(
        Guid CustomerId)
        : IRequest<Result<Customer>>;

    internal sealed class GetAllCustomerDetailsQueryHandler(
        ICustomerRepository customerRepository)
        : IRequestHandler<GetAllCustomerDetailsQuery, Result<Customer>>
{
        public async Task<Result<Customer>> Handle(GetAllCustomerDetailsQuery request, CancellationToken cancellationToken)
        {

            Customer? customer = await customerRepository
                .GetByExpressionAsync(p => p.Id == request.CustomerId, cancellationToken);

            if(customer is null)
            {
                return Result<Customer>.Failure(404, "Customer not found.");
            }

            return customer;

        }
    }
}
