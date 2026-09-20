using MediatR;

namespace Application.Users.CreateUser
{
    public record CreateUserCommand(
        string UserName,
        string Email
    ) : IRequest;
}
