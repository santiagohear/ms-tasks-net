using Application.Users.Shared;
using MediatR;

namespace Application.Users.GetUsers
{
    public record GetUsersQuery() : IRequest<IEnumerable<UserDto>>;
}
