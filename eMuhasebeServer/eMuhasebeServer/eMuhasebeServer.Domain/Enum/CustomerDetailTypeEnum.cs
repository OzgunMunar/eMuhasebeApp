using Ardalis.SmartEnum;

namespace eMuhasebeServer.Domain.Enum
{
    public sealed class CustomerDetailTypeEnum : SmartEnum<CustomerDetailTypeEnum>
    {
        public static readonly CustomerDetailTypeEnum Bank = new("Banka", 1);
        public static readonly CustomerDetailTypeEnum CashRegister = new("Kasa", 1);
        public static readonly CustomerDetailTypeEnum PurchaseInvoice = new("Alış Faturası", 1);
        public static readonly CustomerDetailTypeEnum SellInvoice = new("Satış Faturası", 1);
        public CustomerDetailTypeEnum(string name, int value) : base(name, value)
        {
        }
    }
}
