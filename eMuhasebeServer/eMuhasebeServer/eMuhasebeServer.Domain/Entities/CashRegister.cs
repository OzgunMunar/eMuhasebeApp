using eMuhasebeServer.Domain.Abstractions;
using eMuhasebeServer.Domain.Enum;

namespace eMuhasebeServer.Domain.Entities
{
    public sealed class CashRegister : Entity
    {
        public string CashRegisterName { get; set; } = default!;
        public CurrencyTypeEnum CurrencyType { get; set; } = CurrencyTypeEnum.TL;
        public decimal CashDepositAmount { get; set; }  // Giriş Rakamı
        public decimal CashWithdrawalAmount { get; set; }  // Çıkış Rakamı
        public List<CashRegisterDetail>? CashRegisterDetails { get; set; }
    }
}
