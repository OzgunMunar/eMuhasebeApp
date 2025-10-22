using eMuhasebeServer.Application.Features.Products.Create;
using eMuhasebeServer.Application.Features.Products.Delete;
using eMuhasebeServer.Application.Features.Products.GetAll;
using eMuhasebeServer.Application.Features.Products.Update;
using eMuhasebeServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace eMuhasebeServer.WebAPI.Controllers
{
    [AllowAnonymous]
    public sealed class ProductController(IMediator mediator)
                : CrudController<
            GetAllProductsQuery, CreateProductCommand, UpdateProductCommand, DeleteProductCommand,
            List<Product>, string, string, string>(mediator)
    {

    }
}
