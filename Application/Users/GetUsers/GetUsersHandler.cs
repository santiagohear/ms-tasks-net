using AutoMapper;
using Application.Users.Shared;
using Domain.Ports.Services;
using MediatR;

namespace Application.Users.GetUsers
{
    public class GetUsersHandler(IUserService service, IMapper mapper) : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserService _service = service;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _service.GetUsersAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
    }
}
