using AutoMapper;
using BlogWebApp.Entity.DTOs.Yorums;
using BlogWebApp.Entity.Entities;

namespace BlogWebApp.Service.AutoMapper.Yorums
{
    public class YorumProfil:Profile
    {

        public YorumProfil()
        {
            CreateMap<MakaleYorumEkleDto, Yorum>().ReverseMap();
            CreateMap<YorumsDto, Yorum>().ReverseMap();
        }
    }
}
