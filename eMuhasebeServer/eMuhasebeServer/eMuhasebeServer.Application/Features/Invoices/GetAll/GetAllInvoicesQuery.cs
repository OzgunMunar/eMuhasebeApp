using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Invoices.GetAll
{
    public sealed record GetAllInvoicesQuery(int Type)
        : IRequest<Result<List<Invoice>>>;

    internal sealed class GetAllInvoicesQueryHandler(
        IInvoiceRepository invoiceRepository)
        : IRequestHandler<GetAllInvoicesQuery, Result<List<Invoice>>>
    {
        public async Task<Result<List<Invoice>>> Handle(GetAllInvoicesQuery request, CancellationToken cancellationToken)
        {

            List<Invoice> invoices = await invoiceRepository
                .Where(p => p.Type == request.Type)
                .OrderBy(p => p.Date)
                .ToListAsync(cancellationToken);

            return invoices;

        }
    }
}
