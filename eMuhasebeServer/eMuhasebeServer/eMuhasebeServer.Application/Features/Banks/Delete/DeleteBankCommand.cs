using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Banks.Delete
{
    public sealed record DeleteBankCommand(Guid Id)
        : IRequest<Result<string>>;

    internal sealed class DeleteBankCommandHandler(
        IBankRepository bankRepository,
        IUnitOfWorkCompany unitOfWorkCompany)
        : IRequestHandler<DeleteBankCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteBankCommand request, CancellationToken cancellationToken)
        {

            Bank? bank = await bankRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == request.Id, cancellationToken);

            if (bank is null)
            {
                return Result<string>.Failure(404, "Bank not found");
            }

            bank.IsDeleted = true;

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Bank deleted successfully.");

        }
    }
}
