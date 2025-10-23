using AutoMapper;
using eMuhasebeServer.Application.Features.Banks.Create;
using eMuhasebeServer.Application.Features.Banks.Update;
using eMuhasebeServer.Application.Features.CashRegisters.Create;
using eMuhasebeServer.Application.Features.CashRegisters.Update;
using eMuhasebeServer.Application.Features.Companies.Create;
using eMuhasebeServer.Application.Features.Companies.Update;
using eMuhasebeServer.Application.Features.Customers.Create;
using eMuhasebeServer.Application.Features.Customers.Update;
using eMuhasebeServer.Application.Features.Invoices.Create;
using eMuhasebeServer.Application.Features.Products.Create;
using eMuhasebeServer.Application.Features.Products.Update;
using eMuhasebeServer.Application.Features.Users.CreateUser;
using eMuhasebeServer.Application.Features.Users.UpdateUser;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Enum;

namespace eMuhasebeServer.Application.Mapping
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<CreateUserCommand, AppUser>();
            CreateMap<UpdateUserCommand, AppUser>();

            CreateMap<CreateCompanyCommand, Company>();
            CreateMap<UpdateCompanyCommand, Company>();

            CreateMap<CreateCashRegisterCommand, CashRegister>()
                .ForMember(member => member.CurrencyType, option =>
                {
                    option.MapFrom(map => CurrencyTypeEnum.FromValue(map.CurrencyTypeValue));
                });
            CreateMap<UpdateCashRegisterCommand, CashRegister>()
                .ForMember(member => member.CurrencyType, option =>
                {
                    option.MapFrom(map => CurrencyTypeEnum.FromValue(map.CurrencyTypeValue));
                });

            CreateMap<CreateBankCommand, Bank>()
                .ForMember(member => member.CurrencyType, option =>
                {
                    option.MapFrom(map => CurrencyTypeEnum.FromValue(map.CurrencyTypeValue));
                });
            CreateMap<UpdateBankCommand, Bank>()
                .ForMember(member => member.CurrencyType, option =>
                {
                    option.MapFrom(map => CurrencyTypeEnum.FromValue(map.CurrencyTypeValue));
                });

            CreateMap<CreateCustomerCommand, Customer>()
                .ForMember(member => member.Type, option =>
                {
                    option.MapFrom(map => CustomerTypeEnum.FromValue(map.TypeValue));
                });
            CreateMap<UpdateCustomerCommand, Customer>()
                .ForMember(member => member.Type, option =>
                {
                    option.MapFrom(map => CustomerTypeEnum.FromValue(map.TypeValue));
                });

            CreateMap<CreateProductCommand, Product>();
            CreateMap<UpdateProductCommand, Product>();

            CreateMap<CreateInvoiceCommand, Invoice>()
                .ForMember(member => member.Type, options =>
                {
                    options.MapFrom(map => InvoiceTypeEnum.FromValue(map.TypeValue));
                })
                .ForMember(member => member.Details, options =>
                {
                    // Fatura detaylarını InvoiceDetails içinde topladım.
                    options.MapFrom(map => map.Details.Select(s => new InvoiceDetail()
                    {
                        ProductId = s.ProductId,
                        Quantity = s.Quantity,
                        Price = s.Price,
                    }).ToList());
                }).ForMember(member => member.Amount, options =>
                {
                    // Ürünleri fiyatları ile çarptım ki faturada gösterebileyim.
                    options.MapFrom(map => map.Details.Sum(s => s.Quantity * s.Price));
                });
            //CreateMap<UpdateInvoiceCommand, Invoice>();

        }
    }
}
