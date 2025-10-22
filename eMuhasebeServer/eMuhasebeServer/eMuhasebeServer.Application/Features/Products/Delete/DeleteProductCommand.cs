using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Products.Delete
{
    public sealed record DeleteProductCommand(Guid Id)
        : IRequest<Result<string>>;

    internal sealed class DeleteProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWorkCompany unitOfWorkCompany)
        : IRequestHandler<DeleteProductCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            
            Product? product = await productRepository
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (product == null)
            {
                return Result<string>.Failure(404, "Product not found.");
            }

            product.IsDeleted = true;

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Product successfully deleted.");

        }
    }
}
