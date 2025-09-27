using BlogWebApp.Entity.DTOs.Kategoris;
using BlogWebApp.Entity.Entities;
using Microsoft.AspNetCore.Http;

namespace BlogWebApp.Entity.DTOs.Makales
{
    public class MakaleGuncelleDto
    {
        public Guid Id { get; set; }
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public string Ozet { get; set; }
        public Guid KategoriId { get; set; }
        public Resim Resim { get; set; }
        public IFormFile? Foto { get; set; }
        public ICollection<KategoriDto> Kategories { get; set; }
    }
}
