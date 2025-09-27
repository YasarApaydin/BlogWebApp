using AutoMapper;
using BlogWebApp.Entity.DTOs.Kategoris;
using BlogWebApp.Entity.Entities;

namespace BlogWebApp.Service.AutoMapper.Kategoris
{
    public class KategoriProfil:Profile
    {
        public KategoriProfil()
        {
            CreateMap<KategoriDto, Kategori>().ReverseMap();
            CreateMap<KategoriEkleDto, Kategori>().ReverseMap();
            CreateMap<KategoriGuncelleDto, Kategori>().ReverseMap();
        }
    }
}
