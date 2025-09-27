using AutoMapper;
using BlogWebApp.Entity.DTOs.Makales;
using BlogWebApp.Entity.Entities;

namespace BlogWebApp.Service.AutoMapper.Makales
{
    public class MakaleProfil:Profile
    {
       public MakaleProfil()
        {
            CreateMap<MakaleDto, Makale>().ReverseMap();
            CreateMap<MakaleGuncelleDto, Makale>().ReverseMap();
            CreateMap<MakaleGuncelleDto, MakaleDto>().ReverseMap();
            CreateMap<MakaleEkleDto, Makale>().ReverseMap();
            CreateMap<MakaleYayinlaDto, Makale>().ReverseMap();
        }
    }
}
