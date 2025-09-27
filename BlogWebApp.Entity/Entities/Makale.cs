using BlogWebApp.Core.Entities;
using BlogWebApp.Entity.Enums;

namespace BlogWebApp.Entity.Entities
{
    public class Makale:EntityBase
    {
        public Makale()
        {
            
        }
        public Makale(string icerik,string ozet,string baslik, Guid userId, string olusturanKullanici, Guid kategoriId, Guid resimId,bool yorumlaraIzinVer)
        {
            Icerik = icerik;
            Ozet = ozet;
            Baslik = baslik;
            UserId = userId;
            KategoriId = kategoriId;
            ResimId = resimId;
            OlusturanKullanici = olusturanKullanici;
            YorumlaraIzinVer = yorumlaraIzinVer;
            
        }


        public string Icerik { get; set;  }


        public string Ozet { get; set; }



        public string Baslik { get; set; }
        public int Goruntuleme { get; set; } = 0;
        public Guid KategoriId { get; set; }
        public Kategori Kategori { get; set; }
       
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
        public DateTime YayinlanmaTarihi { get; set; } = DateTime.Now;

        public Guid? ResimId { get; set; }
         
        public Resim Resim { get; set; }

        public BlogDurumu Durum { get; set; }

        public bool YorumlaraIzinVer { get; set; } = true;
        public string Slug { get; set; }
        public ICollection<Yorum> Yorums { get; set; } = new List<Yorum>();
        public ICollection<ArticleVisitor> ArticleVisitors { get; set; }

        public ICollection<MakaleTag> MakaleTags { get; set; } = new List<MakaleTag>();

    }
}
