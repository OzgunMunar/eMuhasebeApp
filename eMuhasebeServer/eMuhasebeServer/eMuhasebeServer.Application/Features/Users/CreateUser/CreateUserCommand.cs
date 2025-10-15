using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using eMuhasebeServer.Domain.Repositories;
using GenericRepository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Users.CreateUser
{
    public sealed record CreateUserCommand(
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        string Password,
        List<Guid> CompanyIds,
        bool IsAdmin)
        : IRequest<Result<string>>;

    internal sealed class CreateUserCommandHandler(
        UserManager<AppUser> userManager,
        IMapper mapper,
        ICompanyUserRepository companyUserRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CreateUserCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            bool isUserNameExist = await userManager.Users
                .AnyAsync(p => p.UserName == request.UserName, cancellationToken);

            if(isUserNameExist)
            {
                return Result<string>.Failure("Bu kullanıcı adı daha önce kullanılmış.");
            }

            bool isEmailExist = await userManager.Users
                .AnyAsync(p => p.Email == request.Email, cancellationToken);

            if(isEmailExist)
            {
                return Result<string>.Failure("Bu mail adresi daha önce kullanılmış.");
            }

            AppUser appUser = mapper.Map<AppUser>(request);

            IdentityResult identityResult = await userManager.CreateAsync(appUser, request.Password);

            if(!identityResult.Succeeded)
            {
                return Result<string>.Failure(identityResult.Errors.Select(s => s.Description).ToList());
            }

            List<CompanyUser> companyUsers = request.CompanyIds.Select(s => new CompanyUser
            {
                AppUserId = appUser.Id,
                CompanyId = s
            }).ToList();

            await companyUserRepository.AddRangeAsync(companyUsers, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return "Kullanıcı kaydı başarıyla tamamlandı.";

        }
    }
}
