using AutoMapper;
using BlogProject.Areas.Admin.Models;
using BlogProject.Models.ViewModels;
using BlogProject.src.Infra.Entitites;

namespace BlogProject.Services.DTOs.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<CreatePostDto, PostEntity>();
            CreateMap<UpdatePostDto, PostEntity>().ReverseMap();
            CreateMap<PostEntity, CreatePostViewModel>().ReverseMap();
            CreateMap<AppUser, UserViewModel>().ReverseMap();
            CreateMap<CategoryEntity, CategoryViewModel>().ReverseMap();
            CreateMap<CategoryEntity, CategoryUpdateViewModel>().ReverseMap();
            CreateMap<TagEntity, TagViewModel>().ReverseMap();
            CreateMap<TagEntity, TagUpdateViewModel>().ReverseMap();
            CreateMap<AppUser, ExtendedProfileViewModel>().ForMember(dest => dest.EmailAddress,opt => opt.MapFrom(src => src.Email));
            CreateMap<ExtendedProfileViewModel, AppUser>().ForMember(dest => dest.Email,opt => opt.MapFrom(src => src.EmailAddress));
        }

        
    }
}
