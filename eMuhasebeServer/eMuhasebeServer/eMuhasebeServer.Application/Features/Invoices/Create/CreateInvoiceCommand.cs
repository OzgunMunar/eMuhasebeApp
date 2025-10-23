using AutoMapper;
using eMuhasebeServer.Domain.Dtos;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Enum;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Invoices.Create
{
    public sealed record CreateInvoiceCommand(
        int TypeValue,
        DateOnly Date,
        string InvoiceNumber,
        Guid CustomerId,
        List<InvoiceDetailDto> InvoiceDetails)
        : IRequest<Result<string>>;

    internal sealed class CreateInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        IProductRepository productRepository,
        IProductDetailRepository productDetailRepository,
        ICustomerRepository customerRepository,
        ICustomerDetailRepository customerDetailRepository,
        IUnitOfWorkCompany unitOfWorkCompany,
        IMapper mapper)
        : IRequestHandler<CreateInvoiceCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {

            #region Fatura ve Detay Kısmı

            Invoice invoice = mapper.Map<Invoice>(request);

            await invoiceRepository.AddAsync(invoice, cancellationToken);

            #endregion

            #region Customer Kısmı

            Customer? customer = await customerRepository
                .GetByExpressionWithTrackingAsync(p => p.Id == request.CustomerId, cancellationToken);

            if (customer == null)
            {
                return Result<string>.Failure(404, "Customer not found.");
            }

            // Müşterinin cebine giren kısım. TypeValue 2 ise alış faturası.
            customer.DepositAmount += request.TypeValue == 2 ? invoice.Amount : 0;
            // üsttekinin tersi
            customer.WithdrawalAmount += request.TypeValue == 1 ? invoice.Amount : 0;

            CustomerDetail customerDetail = new()
            {
                CustomerId = customer.Id,
                Date = request.Date,
                DepositAmount = request.TypeValue == 2 ? invoice.Amount : 0,
                WithdrawalAmount = request.TypeValue == 1 ? invoice.Amount : 0,
                Description = invoice.InvoiceNumber + " Numaralı " + invoice.Type.Name,
                Type = request.TypeValue == 1 ? CustomerDetailTypeEnum.PurchaseInvoice : CustomerDetailTypeEnum.SellInvoice,
                InvoiceId = invoice.Id
                
            };

            await customerDetailRepository.AddAsync(customerDetail, cancellationToken);

            #endregion

            #region Product Kısmı

            foreach (var item in request.InvoiceDetails)
            {

                Product product = await productRepository
                    .GetByExpressionWithTrackingAsync(p => p.Id == item.ProductId, cancellationToken);

                product.Deposit += request.TypeValue == 1 ? item.Quantity : 0;
                product.Withdrawal += request.TypeValue == 2 ? item.Quantity : 0;

                ProductDetail productDetail = new()
                {

                    ProductId = product.Id,
                    Date = request.Date,
                    Description = invoice.InvoiceNumber + " Numaralı " + invoice.Type.Name,
                    Deposit = request.TypeValue == 1 ? item.Quantity : 0,
                    Withdrawal = request.TypeValue == 2 ? item.Quantity : 0,
                    InvoiceId = invoice.Id

                };

                await productRepository.AddAsync(product, cancellationToken);
                await productDetailRepository.AddAsync(productDetail, cancellationToken);

            }

            #endregion

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Invoice successfully created.");

        }
    }
}