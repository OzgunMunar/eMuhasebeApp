using eMuhasebeServer.Domain.Abstractions;
using eMuhasebeServer.Domain.Enum;

namespace eMuhasebeServer.Domain.Entities
{
    public sealed class Bank : Entity
    {
        public string BankName { get; set; } = string.Empty;
        public string IBAN { get; set; } = string.Empty;
        public CurrencyTypeEnum CurrencyType { get; set; } = CurrencyTypeEnum.TL;
        public decimal BankDepositAmount { get; set; } // Giren Para
        public decimal BankWithdrawalAmount { get; set; } // Çıkan Para
        public List<BankDetail>? BankDetails { get; set; }
    }
}
