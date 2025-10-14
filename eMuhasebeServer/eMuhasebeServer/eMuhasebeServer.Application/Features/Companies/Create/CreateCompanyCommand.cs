using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using eMuhasebeServer.Domain.ValueObjects;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies.Create
{
    public sealed record CreateCompanyCommand(
        string CompanyName,
        string FullAddress,
        string TaxDepartment,
        string TaxNumber,
        Database Database
        )
        : IRequest<Result<string>>;

    internal sealed class CreateCompanyCommandHandler(
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<CreateCompanyCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {

            Company? company = await companyRepository
                .GetByExpressionAsync(com => com.TaxNumber == request.TaxNumber, cancellationToken);

            if (company is not null)
            {
                return Result<string>.Failure(409, "Company is already exist in the database");
            }

            company = mapper.Map<Company>(request);

            await companyRepository.AddAsync(company, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Company successfully created.");

        }
    }
}