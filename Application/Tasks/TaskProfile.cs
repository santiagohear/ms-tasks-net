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
            CreateMap<CreateTaskCommand, TaskItem>()
                .ForMember(dest => dest.AdditionalInfoJson, opt => opt.MapFrom(src => TaskAdditionalInfo.MergePriorityAndTags(src.AdditionalInfoJson, src.Priority, src.Tags)));

            CreateMap<TaskItem, TaskDto>()
                .ForMember(dest => dest.AssignedToUserName, opt => opt.MapFrom(src => src.AssignedToUser.UserName))
                .ForMember(dest => dest.CreatedByUserName, opt => opt.MapFrom(src => src.CreatedByUser.UserName))
                .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => TaskAdditionalInfo.Parse(src.AdditionalInfoJson).Priority))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => TaskAdditionalInfo.Parse(src.AdditionalInfoJson).Tags));
        }
    }
}
