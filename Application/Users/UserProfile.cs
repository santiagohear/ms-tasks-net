using Application.Users.CreateUser;
using Application.Users.Shared;
using AutoMapper;
using Domain.Entities;

namespace Application.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserCommand, User>();
            CreateMap<User, UserDto>();
        }
    }
}
