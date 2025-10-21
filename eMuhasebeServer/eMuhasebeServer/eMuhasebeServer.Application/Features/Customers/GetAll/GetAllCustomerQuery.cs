using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Customers.GetAll
{
    public sealed record GetAllCustomerQuery()
        : IRequest<Result<List<Customer>>>;

    internal sealed class GetAllCustomerQueryHandler(
        ICustomerRepository customerRepository)
        : IRequestHandler<GetAllCustomerQuery, Result<List<Customer>>>
    {
        public async Task<Result<List<Customer>>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
        {
            
            List<Customer> customers = await customerRepository
                .GetAll()
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);

            return customers;

        }
    }
}
