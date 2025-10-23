using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.ProductDetails.GetAll
{
    public sealed record GetAllProductDetailsQuery(Guid Id)
        : IRequest<Result<Product>>;

    internal sealed class GetAllProductDetailsQueryHandler(
        IProductRepository productRepository)
        : IRequestHandler<GetAllProductDetailsQuery, Result<Product>>
    {
        public async Task<Result<Product>> Handle(GetAllProductDetailsQuery request, CancellationToken cancellationToken)
        {
            
            Product? product = await productRepository
                .Where(p => p.Id == request.Id)
                .OrderBy(p => p.Name)
                .Include(p => p.Details!)
                .FirstOrDefaultAsync(cancellationToken);

            if(product == null)
            {
                return Result<Product>.Failure(404, "Product not found.");
            }

            return product;

        }
    }
}
