using AutoMapper;
using Microsoft.EntityFrameworkCore.Design;
using TaskManagementSystem.Core.Models;
using TaskManagementSystem.Infrastructure.ViewModel;

namespace TaskManagementSystem.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<TaskItem, TaskItemViewModel> ().ReverseMap();
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<UserViewModel, RegisterUserViewModel>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            CreateMap<TaskItemViewModel, CreateTaskItemViewModel>().ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
            CreateMap<TaskItem, DisplayTaskItemViewModel>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<TaskItemViewModel, DisplayTaskItemViewModel>();
        }
    }
}
