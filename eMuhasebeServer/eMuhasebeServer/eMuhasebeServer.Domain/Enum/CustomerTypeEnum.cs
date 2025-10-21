using Ardalis.SmartEnum;

namespace eMuhasebeServer.Domain.Enum
{
    public sealed class CustomerTypeEnum : SmartEnum<CustomerTypeEnum>
    {

        public static readonly CustomerTypeEnum Buyers = new("Ticari Alıcılar", 1);
        public static readonly CustomerTypeEnum Shippers = new("Ticari Satıcılar", 2);
        public static readonly CustomerTypeEnum Personnal = new("Personel", 3);
        public static readonly CustomerTypeEnum Partners = new("Şirket Ortakları", 4);
        
        public CustomerTypeEnum(string name, int value) : base(name, value)
        {
        }

    }
}
