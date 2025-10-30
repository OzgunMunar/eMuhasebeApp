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
        public List<string> Months { get; set; } = [];
        public List<decimal> Amounts { get; set; } = [];
    }

    internal sealed class PurchaseReportsQueryHandler(
        IInvoiceRepository invoiceRepository)
        : IRequestHandler<PurchaseReportsQuery, Result<PurchaseReportsQueryResponse>>
    {
        public async Task<Result<PurchaseReportsQueryResponse>> Handle(PurchaseReportsQuery request, CancellationToken cancellationToken)
        {

            List<Invoice> invoices = await invoiceRepository
                .Where(i => i.Type == 2)
                .OrderBy(i => i.Date)
                .ToListAsync(cancellationToken);

            var groupedByMonth = invoices
                .GroupBy(i => new { i.Date.Year, i.Date.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => new
                {
                    Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy MMMM"),
                    Total = g.Sum(x => x.Amount)
                })
                .ToList();

            PurchaseReportsQueryResponse purchaseReportsQueryResponse = new()
            {
                Months = groupedByMonth.Select(g => g.Month).ToList(),
                Amounts = groupedByMonth.Select(g => g.Total).ToList()
            };

            return purchaseReportsQueryResponse;

        }
    }
}
