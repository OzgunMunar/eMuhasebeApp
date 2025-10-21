using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Customers.Create
{
    public sealed record CreateCustomerCommand(
        string Name,
        int TypeValue,
        string City,
        string Town,
        string FullAddress,
        string TaxDepartment,
        string TaxNumber)
        : IRequest<Result<string>>;

    internal sealed class CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        IUnitOfWorkCompany unitOfWorkCompany,
        IMapper mapper)
        : IRequestHandler<CreateCustomerCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {

            Customer customer = mapper.Map<Customer>(request);

            await customerRepository.AddAsync(customer, cancellationToken);
            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Customer successfully saved.");

        }
    }
}
