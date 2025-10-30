using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Reports.PurchaseReports
{
    public sealed record PurchaseReportsQuery() : IRequest<Result<PurchaseReportsQueryResponse>>;

    public sealed record PurchaseReportsQueryResponse
    {
        public List<DateOnly> Dates { get; set; } = [];
        public List<decimal> Amounts { get; set; } = [];
    }

    internal sealed class PurchaseReportsQueryHandler(
        IInvoiceRepository invoiceRepository)
        : IRequestHandler<PurchaseReportsQuery, Result<PurchaseReportsQueryResponse>>
    {
        public async Task<Result<PurchaseReportsQueryResponse>> Handle(PurchaseReportsQuery request, CancellationToken cancellationToken)
        {

            List<Invoice> invoices = await invoiceRepository
                .Where(p => p.Type == 2)
                .OrderBy(p => p.Date)
                .ToListAsync(cancellationToken);

            PurchaseReportsQueryResponse response = new PurchaseReportsQueryResponse
            {
                Dates = invoices.GroupBy(group => group.Date).Select(s => s.Key).ToList(),
                Amounts = invoices.GroupBy(group => group.Date).Select(s => s.Sum(s => s.Amount)).ToList()
            };

            return response;

        }
    }
}
