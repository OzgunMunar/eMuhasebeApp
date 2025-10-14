using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using eMuhasebeServer.Domain.ValueObjects;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies.Update
{
    public sealed record UpdateCompanyCommand(
        Guid Id,
        string CompanyName,
        string FullAddress,
        string TaxNumber,
        string TaxDepartment,
        Database Database
        ) : IRequest<Result<string>>;

    internal sealed class UpdateCompanyCommandHandler(
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IRequestHandler<UpdateCompanyCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {

            Company? company = await companyRepository
                .GetByExpressionWithTrackingAsync(com => com.Id == request.Id, cancellationToken);

            if(company is null)
            {
                return Result<string>.Failure(404, "Company not found.");
            }

            if(company.TaxNumber != request.TaxNumber)
            {
                
                bool isTaxNumberExist = await companyRepository
                    .AnyAsync(com => com.TaxNumber == request.TaxNumber, cancellationToken);

                if(isTaxNumberExist)
                {
                    return Result<string>.Failure(409, "TaxNumber is already in use by different company");
                }

            }

            mapper.Map(request, company);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Company updated successfully.");

        }
    }
}
