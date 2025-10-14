using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Companies.Delete
{
    public sealed record DeleteCompanyByIdCommand(
        Guid Id)
        : IRequest<Result<string>>;

    internal sealed class DeleteCompanyCommandHandler(
        ICompanyRepository companyRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<DeleteCompanyByIdCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteCompanyByIdCommand request, CancellationToken cancellationToken)
        {

            Company? company = await companyRepository
                .GetByExpressionWithTrackingAsync(com => com.Id == request.Id, cancellationToken);

            if(company is null)
            {
                return Result<string>.Failure(404, "Company not found");
            }

            company.IsDeleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<string>.Succeed("Company deleted successfully.");

        }
    }
}
