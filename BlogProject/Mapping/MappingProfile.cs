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
            CreateMap<BlogProject.Areas.Admin.Models.CategoryViewModel, CategoryDto>().ReverseMap();
            CreateMap<CategoryAddViewModel, CategoryAddDto>().ReverseMap();
            CreateMap<CategoryUpdateViewModel, CategoryUpdateDto>().ReverseMap();
            CreateMap<CategoryEntity,CategoryUpdateViewModel>().ReverseMap();
            CreateMap<TagEntity,TagUpdateViewModel>().ReverseMap();
            CreateMap<TagUpdateViewModel, TagUpdateDto>().ReverseMap();

            CreateMap<ItemPagination<RoleViewModel>, ItemPagination<RoleDto>>().ReverseMap();
            CreateMap<ItemPagination<TagViewModel>, ItemPagination<TagDto>>().ReverseMap();

            CreateMap<RoleViewModel, RoleDto>().ReverseMap();
            CreateMap<TagViewModel, TagDto>().ReverseMap();
            CreateMap<TagAddDto,TagAddViewModel>().ReverseMap();

            CreateMap<ConfirmEmailDto, ConfirmEmailViewModel>().ReverseMap();

            CreateMap<ForgetPasswordDto, ForgetPasswordViewModel>().ReverseMap();
            CreateMap<ResetPasswordDto, ResetPasswordViewModel>().ReverseMap();
            CreateMap<PasswordChangeDto, PasswordChangeViewModel>().ReverseMap();

            CreateMap<SignUpDto,SignUpViewModel>().ReverseMap();
            CreateMap<SignInDto, SignInViewModel>().ReverseMap();

            CreateMap<UserViewModel, UserDto>().ReverseMap();
            CreateMap<RoleEditViewModel,RoleEditDto>().ReverseMap();
            CreateMap<RoleUsersViewModel, RoleUsersDto>().ReverseMap();
            CreateMap<PostEntity, CreatePostDto>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<CategoryEntity, CategoryDto>().ReverseMap();
            CreateMap<CategoryEntity, CategoryUpdateDto>().ReverseMap();
            CreateMap<TagEntity, TagDto>().ReverseMap();
            CreateMap<TagEntity, TagUpdateDto>().ReverseMap();
            CreateMap<AppUser, ExtendedProfileDto>().ForMember(dest => dest.EmailAddress,opt => opt.MapFrom(src => src.Email));
            CreateMap<ExtendedProfileDto, AppUser>().ForMember(dest => dest.Email,opt => opt.MapFrom(src => src.EmailAddress));

            CreateMap<ExtendedProfileDto, ExtendedProfileViewModel>().ReverseMap();

            CreateMap<AppUser, VisitorProfileDto>().ReverseMap();
            CreateMap<ExtendedProfileViewModel, VisitorProfileDto>().ReverseMap();

            // VisitorProfileDto -> ExtendedProfileDto
            CreateMap<VisitorProfileDto, ExtendedProfileDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.EmailAddress, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());

            CreateMap<ExtendedProfileDto, VisitorProfileDto>();

            CreateMap<CreatePostDto,CreatePostViewModel>().ReverseMap();

        }

        
    }
}
