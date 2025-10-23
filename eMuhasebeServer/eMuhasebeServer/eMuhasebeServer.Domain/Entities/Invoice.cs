using eMuhasebeServer.Domain.Abstractions;
using eMuhasebeServer.Domain.Enum;

namespace eMuhasebeServer.Domain.Entities
{
    public sealed class Invoice : Entity
    {
        public DateOnly Date { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public InvoiceTypeEnum Type { get; set; } = InvoiceTypeEnum.Purchase;
        public Customer? Customer { get; set; }
        public decimal Amount { get; set; }
        public List<InvoiceDetail>? Details { get; set; }

    }

}
