using Ardalis.SmartEnum;

namespace eMuhasebeServer.Domain.Enum
{
    public sealed class CurrencyTypeEnum : SmartEnum<CurrencyTypeEnum>
    {
        public CurrencyTypeEnum(string name, int value) : base(name, value)
        {

        }

        public static readonly CurrencyTypeEnum TL = new("TL", 1);
        public static readonly CurrencyTypeEnum USD = new("USD", 1);
        public static readonly CurrencyTypeEnum EUR = new("EUR", 1);

    }
}
