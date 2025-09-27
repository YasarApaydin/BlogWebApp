using AutoMapper;
using BlogWebApp.Entity.DTOs.Users;
using BlogWebApp.Entity.Entities;

namespace BlogWebApp.Service.AutoMapper.Kullanicis
{
    public class KullaniciProfil:Profile
    {
        public KullaniciProfil()
        {
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<AppUser, UserAddDto>().ReverseMap();
            CreateMap<AppUser, UserUpdateDto>().ReverseMap();
            CreateMap<AppUser, UserProfileDto>().ReverseMap();
            CreateMap<AppUser, UserProfileEditDto>().ReverseMap();
            CreateMap<AppUser, UserRegisterDto>().ReverseMap();
            CreateMap<AppUser, UserProfileEditUpdateDto>().ReverseMap();
        
        }
    }
}
