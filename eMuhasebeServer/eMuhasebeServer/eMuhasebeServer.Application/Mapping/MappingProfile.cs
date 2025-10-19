using AutoMapper;
using eMuhasebeServer.Application.Features.Banks.Create;
using eMuhasebeServer.Application.Features.Banks.Update;
using eMuhasebeServer.Application.Features.CashRegisters.Create;
using eMuhasebeServer.Application.Features.CashRegisters.Update;
using eMuhasebeServer.Application.Features.Companies.Create;
using eMuhasebeServer.Application.Features.Companies.Update;
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

            CreateMap<CreateBankCommand, Bank>();
            CreateMap<UpdateBankCommand, Bank>();

        }
    }
}
