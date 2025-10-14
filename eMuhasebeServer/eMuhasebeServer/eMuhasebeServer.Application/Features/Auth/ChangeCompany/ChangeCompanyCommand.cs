using eMuhasebeServer.Application.Features.Auth.Login;
using eMuhasebeServer.Application.Services;
using eMuhasebeServer.Domain.Dtos;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Auth.ChangeCompany
{
    public sealed record ChangeCompanyCommand(Guid CompanyId)
        : IRequest<Result<LoginCommandResponse>>;

    internal sealed class ChangeCompanyCommandHandler(
        ICompanyUserRepository companyUserRepository,
        UserManager<AppUser> userManager,
        IHttpContextAccessor httpContextAccessor,
        IJwtProvider jwtProvider)
        : IRequestHandler<ChangeCompanyCommand, Result<LoginCommandResponse>>
    {
        public async Task<Result<LoginCommandResponse>> Handle(ChangeCompanyCommand request, CancellationToken cancellationToken)
        {
            
            if(httpContextAccessor.HttpContext is null)
            {
                return Result<LoginCommandResponse>.Failure("User is unauthorized to make this action.");
            }

            string? userIdString = httpContextAccessor.HttpContext.User
                .FindFirstValue("Id");

            if(string.IsNullOrEmpty(userIdString))
            {
                return Result<LoginCommandResponse>.Failure("User is unauthorized to make this action.");
            }

            AppUser? appUser = await userManager.FindByIdAsync(userIdString);

            if(appUser == null)
            {
                return Result<LoginCommandResponse>.Failure(404, "User not found.");
            }

            List<CompanyUser> companyUsers = await companyUserRepository
                .Where(p => p.AppUserId == appUser.Id)
                .Include(p => p.Company)
                .ToListAsync(cancellationToken);

            List<CompanyTokenDTO> companies = companyUsers
                .Select(s => new CompanyTokenDTO
                {
                    UserId = s.AppUserId,
                    CompanyId = s.CompanyId,
                    CompanyName = s.Company!.CompanyName,
                    TaxDepartment = s.Company!.TaxDepartment,
                    TaxNumber = s.Company!.TaxNumber,
                    FullAddress = s.Company!.FullAddress
                }).ToList();

            var response = await jwtProvider.CreateToken(appUser, request.CompanyId, companies);

            return response;

        }
    }
}
