using eMuhasebeServer.Domain.Abstractions;

namespace eMuhasebeServer.Domain.Entities
{
    public sealed class CashRegisterDetail : Entity
    {
        public Guid CashRegisterId { get; set; }
        public DateOnly OpenedDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal CashDepositAmount { get; set; }  // Giriş Rakamı
        public decimal CashWithdrawalAmount { get; set; }  // Çıkış Rakamı
        public Guid? CashRegisterDetailId { get; set; }
        public CashRegisterDetail? CashRegisterDetailOpposite { get; set; }
    }
}
