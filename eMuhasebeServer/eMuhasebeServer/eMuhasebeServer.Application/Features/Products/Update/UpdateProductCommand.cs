using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Products.Update
{
    public sealed record UpdateProductCommand(
        Guid Id,
        string Name)
        : IRequest<Result<string>>;

    internal sealed class UpdateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWorkCompany unitOfWorkCompany,
        IMapper mapper)
        : IRequestHandler<UpdateProductCommand, Result<string>>
{
        public async Task<Result<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            
            Product? product = await productRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == request.Id);

            if(product == null)
            {
                return Result<string>.Failure(404, "Product not found");
            }

            if(product.Name != request.Name)
            {
                bool isNameExist = await productRepository
                    .AnyAsync(p => p.Name == request.Name);

                if(isNameExist)
                {
                    return Result<string>.Failure("Product Name is in use.");
                }

            }

            mapper.Map(request, product);

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Product successfully saved.");

        }
    }
}
