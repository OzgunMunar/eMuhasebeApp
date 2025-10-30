using Bogus;
using eMuhasebeServer.Application.Features.Customers.Create;
using eMuhasebeServer.Application.Features.Customers.GetAll;
using eMuhasebeServer.Application.Features.Invoices.Create;
using eMuhasebeServer.Application.Features.Products.Create;
using eMuhasebeServer.Application.Features.Products.GetAll;
using eMuhasebeServer.Domain.Dtos;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TS.Result;

namespace eMuhasebeServer.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedDataController : ApiController
    {
        public SeedDataController(IMediator mediator) : base(mediator)
        {
        }

        //[HttpGet]
        //public Task<IActionResult> Create()
        //{

            //Faker faker = new();
            //Random random = new Random();

            //Customers
            //for (int i = 0; i < 164; i++)
            //{
            //    int customerTypeValue = random.Next(1, 5);

            //    CreateCustomerCommand customer = new(

            //        faker.Company.CompanyName(),
            //        customerTypeValue,
            //        faker.Address.FullAddress(),
            //        faker.Address.City() + " Tax Department",
            //        faker.Company.Random.ULong(1111111111, 999999999999).ToString()

            //        );

            //    await _mediator.Send(customer);

            //}

            // Products
            //for (int i = 0; i < 346; i++)
            //{

            //    CreateProductCommand product = new(
            //        faker.Commerce.ProductName());

            //    await _mediator.Send(product);
            //}

            //Invoices
            //var customersResults = await _mediator.Send(new GetAllCustomerQuery());
            //var customers = customersResults.Data;

            //var productsResults = await _mediator.Send(new GetAllProductsQuery());
            //var products = productsResults.Data;

            //for (int i = 0; i < 290; i++)
            //{

            //    if (products is null) continue;
            //    if (customers is null) continue;

            //    List<InvoiceDetailDto> invoiceDetails = new();

            //    for (int j = 0; j < random.Next(1, 11); j++)
            //    {

            //        InvoiceDetailDto invoiceDetailDto = new()
            //        {
            //            ProductId = products[random.Next(1, products.Count)].Id,
            //            Price = random.Next(10, 1000),
            //            Quantity = random.Next(11, 100)
            //        };

            //        invoiceDetails.Add(invoiceDetailDto);

            //    }

            //    var customer = customers[random.Next(0, customers.Count)];

            //    CreateInvoiceCommand invoice = new(
            //        random.Next(1, 3),
            //        new DateOnly(2025, random.Next(1, 13), random.Next(1, 29)),
            //        faker.Random.ULong(3, 16).ToString(),
            //        customer.Id,
            //        invoiceDetails);

            //    await _mediator.Send(invoice);

            //}

            //return Ok(Result<string>.Succeed("Seed data veriler oluşturuldu"));
        //}
    }
}