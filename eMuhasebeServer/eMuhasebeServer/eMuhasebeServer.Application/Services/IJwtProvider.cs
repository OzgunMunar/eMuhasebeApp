using eMuhasebeServer.Application.Features.Auth.Login;
using eMuhasebeServer.Domain.Dtos;
using eMuhasebeServer.Domain.Entities;

namespace eMuhasebeServer.Application.Services
{
    public interface IJwtProvider
    {
        Task<LoginCommandResponse> CreateToken(AppUser user, Guid? companyId, List<CompanyTokenDTO> companies);
    }
}
