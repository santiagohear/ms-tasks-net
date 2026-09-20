using AutoMapper;
using Domain.Entities;
using Domain.Ports.Services;
using MediatR;

namespace Application.Users.CreateUser
{
    public class CreateUserHandler(IUserService service, IMapper mapper) : IRequestHandler<CreateUserCommand>
    {
        private readonly IUserService _service = service;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _service.CreateUserAsync(_mapper.Map<User>(request));
        }
    }
}
