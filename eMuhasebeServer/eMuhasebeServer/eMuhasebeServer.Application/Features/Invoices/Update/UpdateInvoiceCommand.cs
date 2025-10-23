using eMuhasebeServer.Domain.Dtos;
using MediatR;
using TS.Result;
// Faturayı silip tekrardan oluşturacağım.
namespace eMuhasebeServer.Application.Features.Invoices.Update
{
    public sealed record UpdateInvoiceCommand(
        Guid Id,
        int TypeValue,
        DateOnly Date,
        string InvoiceNumber,
        Guid CustomerId,
        List<InvoiceDetailDto> InvoiceDetails)
    : IRequest<Result<string>>;

    internal sealed class UpdateInvoiceCommandHandler(
        //IInvoiceRepository invoiceRepository,
        //IProductRepository productRepository,
        //IProductDetailRepository productDetailRepository,
        //ICustomerRepository customerRepository,
        //ICustomerDetailRepository customerDetailRepository,
        //IUnitOfWorkCompany unitOfWorkCompany,
        //IMapper mapper
        )
        : IRequestHandler<UpdateInvoiceCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
        {

            //Invoice? invoice = await invoiceRepository
            //    .Where(p => p.Id == request.Id)
            //    .Include(p => p.Details)
            //    .FirstOrDefaultAsync(cancellationToken);

            //if (invoice == null)
            //{
            //    return Result<string>.Failure(404, "Invoice not found");
            //}

            return await Task.FromResult(Result<string>.Failure(500, "Invoice Update operation is not implemented."));

        }

    }

}