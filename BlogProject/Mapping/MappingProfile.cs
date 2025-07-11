using AutoMapper;
using BlogProject.Application.DTOs;
using BlogProject.Application.Models;
using BlogProject.Areas.Admin.Models;
using BlogProject.Domain.Entities;
using BlogProject.Web.ViewModels;

namespace BlogProject.Web.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<ItemPagination<CategoryViewModel>, ItemPagination<CategoryDto>>().ReverseMap();
            CreateMap<CategoryAddViewModel, CategoryAddDto>().ReverseMap();
            CreateMap<CategoryUpdateViewModel, CategoryAddDto>().ReverseMap();

            CreateMap<ItemPagination<RoleViewModel>, ItemPagination<RoleDto>>().ReverseMap();
            CreateMap<ItemPagination<TagViewModel>, ItemPagination<TagDto>>().ReverseMap();

            CreateMap<PostEntity, CreatePostDto>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<CategoryEntity, CategoryDto>().ReverseMap();
            CreateMap<CategoryEntity, CategoryUpdateDto>().ReverseMap();
            CreateMap<TagEntity, TagDto>().ReverseMap();
            CreateMap<TagEntity, TagUpdateDto>().ReverseMap();
            CreateMap<AppUser, ExtendedProfileDto>().ForMember(dest => dest.EmailAddress,opt => opt.MapFrom(src => src.Email));
            CreateMap<ExtendedProfileDto, AppUser>().ForMember(dest => dest.Email,opt => opt.MapFrom(src => src.EmailAddress));
        }

        
    }
}
