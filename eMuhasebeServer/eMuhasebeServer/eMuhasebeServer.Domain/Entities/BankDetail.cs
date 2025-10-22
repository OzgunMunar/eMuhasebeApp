using eMuhasebeServer.Domain.Abstractions;

namespace eMuhasebeServer.Domain.Entities
{
    public sealed class BankDetail : Entity
    {
        public Guid BankId { get; set; }
        public DateOnly OpenedDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal BankDepositAmount { get; set; }  // Giriş Rakamı
        public decimal BankWithdrawalAmount { get; set; }  // Çıkış Rakamı
        public Guid? BankDetailId { get; set; }
        public Guid? CashRegisterDetailId { get; set; }  // Bankadan kasaya aktarım.
        public Guid? CustomerDetailId { get; set; }
    }
}
