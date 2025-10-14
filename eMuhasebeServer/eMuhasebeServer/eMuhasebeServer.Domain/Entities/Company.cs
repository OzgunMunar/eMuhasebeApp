using eMuhasebeServer.Domain.Abstractions;
using eMuhasebeServer.Domain.ValueObjects;

namespace eMuhasebeServer.Domain.Entities
{
    public sealed class Company : Entity
    {
        public string CompanyName { get; set; } = default!;
        public string FullAddress { get; set; } = default!;
        public Database Database { get; set; } = default!;
        public string TaxDepartment { get; set; } = default!;
        public string TaxNumber { get; set; } = default!;
    }
}
