using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Products.Create
{
    public sealed record CreateProductCommand(
        string Name)
        : IRequest<Result<string>>;

    internal sealed class CreateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWorkCompany unitOfWorkCompany,
        IMapper mapper)
        : IRequestHandler<CreateProductCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            
            bool isNameExist = await productRepository
                .AnyAsync(p => p.Name == request.Name, cancellationToken);

            if (isNameExist)
            {
                return Result<string>.Failure(409, "Product name is already in use.");
            }

            Product product = mapper.Map<Product>(request);

            await productRepository.AddAsync(product, cancellationToken);
            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Product successfully created.");

        }
    }
}
