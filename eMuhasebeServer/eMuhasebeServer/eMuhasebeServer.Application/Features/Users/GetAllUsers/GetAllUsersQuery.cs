using eMuhasebeServer.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace eMuhasebeServer.Application.Features.Users.GetAllUsers
{
    public sealed record GetAllUsersQuery()
        :IRequest<Result<List<AppUser>>>;

    internal sealed class GetAllUsersQueryHandler(
        UserManager<AppUser> userManager)
        : IRequestHandler<GetAllUsersQuery, Result<List<AppUser>>>
    {
        public async Task<Result<List<AppUser>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {

            List<AppUser> users = await userManager.Users
                .Include(p => p.CompanyUsers!)
                .ThenInclude(p => p.Company)
                .OrderBy(user => user.FirstName)
                .ToListAsync(cancellationToken);

            return users;

        }
    }
}
