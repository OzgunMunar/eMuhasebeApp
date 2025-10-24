using AutoMapper;
using eMuhasebeServer.Domain.Dtos;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Enum;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using System.Text;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Invoices.Create
{
    public sealed record class CreateInvoiceCommand(

        int TypeValue,
        DateOnly Date,
        string InvoiceNumber,
        Guid CustomerId,
        List<InvoiceDetailDto> Details

    ) : IRequest<Result<string>>;

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

            string dayOfToday = DateTime.Now.Day.ToString("D2");
            string monthOfToday = DateTime.Now.Month.ToString("D2");
            string yearOfToday = DateTime.Now.Year.ToString();
            string lastEightCharactersOfRandomGuid = Guid.NewGuid().ToString("N")[^8..];

            string invoiceNumber = $"{dayOfToday}{monthOfToday}{yearOfToday}-{lastEightCharactersOfRandomGuid}";

            var updatedRequest = request with { InvoiceNumber = invoiceNumber };

            Invoice invoice = mapper.Map<Invoice>(updatedRequest);

            await invoiceRepository.AddAsync(invoice, cancellationToken);

            #endregion

            #region Customer Kısmı

            Customer? customer = await customerRepository
                .GetByExpressionAsync(p => p.Id == request.CustomerId, cancellationToken);

            if (customer == null)
            {
                return Result<string>.Failure(404, "Customer not found.");
            }

            // Müşterinin cebine giren kısım. TypeValue 2 ise alış faturası.
            customer.DepositAmount += request.TypeValue == 2 ? invoice.Amount : 0;
            // üsttekinin tersi
            customer.WithdrawalAmount += request.TypeValue == 1 ? invoice.Amount : 0;

            customerRepository.Update(customer);

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

            foreach (var item in request.Details)
            {

                Product product = await productRepository
                    .GetByExpressionAsync(p => p.Id == item.ProductId, cancellationToken);

                product.Deposit += request.TypeValue == 1 ? item.Quantity : 0;
                product.Withdrawal += request.TypeValue == 2 ? item.Quantity : 0;

                productRepository.Update(product);

                ProductDetail productDetail = new()
                {

                    ProductId = product.Id,
                    Date = request.Date,
                    Description = invoice.InvoiceNumber + " Numaralı " + invoice.Type.Name,
                    Deposit = request.TypeValue == 1 ? item.Quantity : 0,
                    Withdrawal = request.TypeValue == 2 ? item.Quantity : 0,
                    InvoiceId = invoice.Id

                };

                await productDetailRepository.AddAsync(productDetail, cancellationToken);

            }

            #endregion

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed("Invoice successfully created.");

        }
    }
}