using AutoMapper;
using BlogProject.src.Infra.Entitites;

namespace BlogProject.Services.DTOs.MappingProfile
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<CreatePostDto, PostEntity>();
            CreateMap<UpdatePostDto, PostEntity>().ReverseMap();
        }
    }
}
