using Application.Tasks.CreateTask;
using Application.Tasks.Shared;
using AutoMapper;
using Domain.Entities;

namespace Application.Tasks
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<CreateTaskCommand, TaskItem>();

            CreateMap<TaskItem, TaskDto>()
                .ForMember(dest => dest.AssignedToUserName, opt => opt.MapFrom(src => src.AssignedToUser.UserName))
                .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedByUser.UserName));
        }
    }
}
