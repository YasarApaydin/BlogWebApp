using BlogWebApp.Entity.DTOs.Kategoris;
using BlogWebApp.Entity.Entities;

namespace BlogWebApp.Entity.DTOs.Makales
{
    public class MakaleDto
    {
        public Guid Id { get; set; }
        public string Icerik { get; set; }
        public string Ozet { get; set; }
        public KategoriDto Kategori { get; set; }
        public bool IsDeleted { get; set; }
        public string Baslik { get; set; }
        public int Goruntuleme { get; set; }
        public Resim Resim { get; set; }
        public string OlusturanKullanici { get; set; }
        public DateTime YayinlanmaTarihi { get; set; }
        public string Slug { get; set; }
        public bool YorumlaraIzinVer { get; set; }
        public IList<Yorum> Yorums { get; set; }
        public IList<Tag> Tags { get; set; } 
        public AppUser User { get; set; }

    }
}
