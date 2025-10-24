using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Invoices.Delete
{
    public sealed record DeleteInvoiceCommand(Guid Id)
        : IRequest<Result<string>>;

    internal sealed class DeleteInvoiceCommandHandler(
        IInvoiceRepository invoiceRepository,
        ICustomerRepository customerRepository,
        ICustomerDetailRepository customerDetailRepository,
        IProductRepository productRepository,
        IProductDetailRepository productDetailRepository,
        IUnitOfWorkCompany unitOfWorkCompany)
        : IRequestHandler<DeleteInvoiceCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
        {

            Invoice? invoice = await invoiceRepository
                .Where(p => p.Id == request.Id)
                .Include(p => p.Details)
                .FirstOrDefaultAsync(cancellationToken);

            if (invoice == null)
            {
                return Result<string>.Failure(404, "Invoice not found.");
            }

            // Eğer faturayı bulursam, öncelikle fatura bilgime göre gidip detaydaki rakamları silip
            // fatura kadarını hem customer'dan hem de product'tan liste olarak oluşturayım.

            CustomerDetail? customerDetail = await customerDetailRepository
                .Where(p => p.InvoiceId == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (customerDetail != null)
            {
                customerDetailRepository.Delete(customerDetail);
            }
            
            Customer? customer = await customerRepository
                .Where(p => p.Id == invoice.CustomerId)
                .FirstOrDefaultAsync(cancellationToken);

            if (customer != null)
            {
                // Firma kendine giriş bana da çıkış yazması gerek. Alış faturası ise 0, değilse 
                // invoice'daki amount'u yazsın.
                customer.DepositAmount -= invoice.Type.Value == 1 ? 0 : invoice.Amount;
                customer.WithdrawalAmount -= invoice.Type.Value == 2 ? 0 : invoice.Amount;

                customerRepository.Update(customer);
            }

            List<ProductDetail> productDetails = await productDetailRepository
                .Where(p => p.InvoiceId == invoice.Id)
                .ToListAsync(cancellationToken);

            foreach (var detail in productDetails)
            {
                Product? product = await productRepository
                    .GetByExpressionWithTrackingAsync(p => p.Id == detail.ProductId, cancellationToken);

                if(product != null)
                {
                    product.Deposit -= detail.Deposit;
                    product.Withdrawal -= detail.Withdrawal;

                    productRepository.Update(product);
                }
            }

            invoiceRepository.Delete(invoice);
            productDetailRepository.DeleteRange(productDetails);

            await unitOfWorkCompany.SaveChangesAsync(cancellationToken);

            return Result<string>.Succeed(invoice.Type.Name.ToString() + " successfully saved.");

        }
    }
}