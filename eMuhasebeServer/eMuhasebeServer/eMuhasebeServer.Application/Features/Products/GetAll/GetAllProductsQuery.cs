using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Products.GetAll
{
    public sealed record GetAllProductsQuery()
        : IRequest<Result<List<Product>>>;

    internal sealed class GetAllProductsQueryHandler(
        IProductRepository productRepository)
        : IRequestHandler<GetAllProductsQuery, Result<List<Product>>>
    {
        public async Task<Result<List<Product>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            
            List<Product> products = await productRepository
                .GetAll()
                .OrderBy(p => p.Name)
                .ToListAsync(cancellationToken);

            return products;

        }
    }
}
