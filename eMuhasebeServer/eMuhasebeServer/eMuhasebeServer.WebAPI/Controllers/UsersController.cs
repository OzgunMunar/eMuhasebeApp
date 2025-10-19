using eMuhasebeServer.Application.Features.Users.CreateUser;
using eMuhasebeServer.Application.Features.Users.DeleteUserById;
using eMuhasebeServer.Application.Features.Users.GetAllUsers;
using eMuhasebeServer.Application.Features.Users.UpdateUser;
using eMuhasebeServer.Domain.Entities;
using MediatR;

namespace eMuhasebeServer.WebAPI.Controllers
{
    public sealed class UsersController(IMediator mediator)
        : CrudController<
        GetAllUsersQuery, CreateUserCommand, UpdateUserCommand, DeleteUserByIdCommand,
        List<AppUser>, string, string, string>(mediator)
    {
    }
}
