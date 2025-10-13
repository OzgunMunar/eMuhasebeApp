using AutoMapper;
using eMuhasebeServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Users.UpdateUser
{
    public sealed record UpdateUserCommand(
        Guid Id,
        string FirstName,
        string LastName,
        string UserName,
        string Email,
        string? Password)
        : IRequest<Result<string>>;

    internal sealed class UpdateUserCommandHandler(
        UserManager<AppUser> userManager,
        IMapper mapper)
        : IRequestHandler<UpdateUserCommand, Result<string>>
    {
        public async Task<Result<string>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {

            AppUser? appUser = await userManager
                .FindByIdAsync(request.Id.ToString());

            bool isMailChanged = false;

            if (appUser == null)
            {
                return Result<string>.Failure("Kullanıcı bulunamadı.");
            }

            if(appUser.UserName != request.UserName)
            {
                bool isUserNameExist = await userManager.Users
                .AnyAsync(p => p.UserName == request.UserName, cancellationToken);

                if (isUserNameExist)
                {
                    return Result<string>.Failure("Bu kullanıcı adı daha önce kullanılmış.");
                }
            }

            if(appUser.Email != request.Email)
            {
                bool isEmailExist = await userManager.Users
                .AnyAsync(p => p.Email == request.Email, cancellationToken);

                if (isEmailExist)
                {
                    return Result<string>.Failure("Bu mail adresi daha önce kullanılmış.");
                }

                isMailChanged = true;
                appUser.EmailConfirmed = false; 
            }

            // Onay maili gönder eğer mail adresi değişmişse.

            mapper.Map(request, appUser);

            IdentityResult identityResult = await userManager.UpdateAsync(appUser);

            if (!identityResult.Succeeded)
            {
                return Result<string>.Failure(identityResult.Errors.Select(s => s.Description).ToList());
            }

            if(request.Password is not null)
            {
                string token = await userManager.GeneratePasswordResetTokenAsync(appUser);

                identityResult = await userManager.ResetPasswordAsync(appUser, token, request.Password);

                if (!identityResult.Succeeded)
                {
                    return Result<string>.Failure(identityResult.Errors.Select(s => s.Description).ToList());
                }
            }

            if(isMailChanged)
            {
                // Tekrar onay maili gönder.
            }

            return "Kullanıcı başarıyla güncellendi.";

        }
    }
}
