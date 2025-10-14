using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies.GetAllCompanies
{
    public sealed record GetAllCompaniesQuery()
        : IRequest<Result<List<Company>>>;

    internal sealed class GetAllCompaniesQueryHandler(
        ICompanyRepository companyRepository)
        : IRequestHandler<GetAllCompaniesQuery, Result<List<Company>>>
    {
        public async Task<Result<List<Company>>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
        {

            List<Company> companies = await companyRepository
                .GetAll()
                .OrderBy(company => company.CompanyName)
                .ToListAsync(cancellationToken);

            return companies;

        }
    }
}
