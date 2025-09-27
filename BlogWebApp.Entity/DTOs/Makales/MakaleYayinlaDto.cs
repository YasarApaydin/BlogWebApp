using BlogWebApp.Entity.DTOs.Kategoris;
using BlogWebApp.Entity.Enums;
using Microsoft.AspNetCore.Http;

namespace BlogWebApp.Entity.DTOs.Makales
{
    public class MakaleYayinlaDto
    {

        public string Baslik { get; set; }
        public string Icerik { get; set; }

        public string Ozet { get; set; }
        public Guid KategoriId { get; set; }

        public bool YorumIzin { get; set; }
        public IFormFile? Photo { get; set; }
        public ICollection<KategoriDto> Kategories { get; set; }
        public BlogDurumu Durum { get; set; }
        public string Tags { get; set; }
    }
}
